using Microsoft.AspNetCore.SignalR.Client;
using MVVM.Helpers;
using MVVM.Models;
using MVVM.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVVM.Services
{
    public class HttpClientService
    {
        public string token;
        static string url = (DeviceInfo.Platform == DevicePlatform.Android) ? APIResources.URLForHUBAndroid : APIResources.URLForHUB;
        HttpClient client;
        static HubConnection connection;
        static HubConnectionBuilder hubConnectionBuilder;

        public HttpClientService()
        {
        }

        public static async void InitChatService()
        {
            try
            {
                hubConnectionBuilder = new HubConnectionBuilder();
                connection = hubConnectionBuilder
                    .WithUrl(url, (opts) =>
                    {
                        opts.AccessTokenProvider = async () =>
                        {
                            return await SecureStorage.GetAsync("token");
                        };
                        opts.HttpMessageHandlerFactory = (message) =>
                        {
                            if (message is HttpClientHandler clientHandler)
                                // bypass SSL certificate
                                clientHandler.ServerCertificateCustomValidationCallback +=
                                        (sender, certificate, chain, sslPolicyErrors) => { return true; };
                            return message;
                        };
                    })
                    .Build();
                await connection.StartAsync();

                connection.On<string, string>("ReceiveMessage", (user, message) =>
                {
                    ReceiveMessage(user, message);
                });

                connection.On<string, string>("PrivateMessage", (user, message) =>
                {
                    ReceiveMessage(user, message);
                });

                connection.On<List<User>>("ConnectedUsers", (sender) =>
                {
                    //user connected/disconnected
                    var itemPosition = sender.FindIndex(x => x.connectionId == connection.ConnectionId);
                    sender.RemoveAt(itemPosition);
                    Utils.UsersList = sender;
                    MessagingCenter.Send<object>(Utils.Connected.Connected, "UserConnected");
                });
            }
            catch (Exception e)
            {

            }
        }

        public async Task<string> UpdateAndSendTokenToHubAsync(bool validateToken, string userName = null, string savedToken = null)
        {
            var uri = (DeviceInfo.Platform == DevicePlatform.Android) ? APIResources.BaseURLAndroid : APIResources.BaseURL;
            if (validateToken)
            {
                var tokenValidated = await ValidateToken(uri, savedToken, userName);
                if (string.IsNullOrEmpty(tokenValidated))
                {
                    return null;
                }
                else
                {
                    return tokenValidated;
                }
            }
            else
            {
                Request request = new Request()
                {
                    UserName = userName
                };
                var result = await PostServerCall<Request, Request>(uri, APIResources.Authenticate, request);
                token = result.Token;
                return token;
            }
        }

        private async Task<string> ValidateToken(string uri, string savedToken, string userName)
        {
            Request request = new Request()
            {
                UserName = userName
            };
            try
            {
                var isValid = await PostServerCall<string, bool>(uri, APIResources.Validate, savedToken, savedToken);
                if (!isValid)
                {
                    token = await PostServerCall<Request, string>(uri, APIResources.Authenticate, request);
                    return token;
                }
                else
                {
                    return savedToken;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<User>> GetUsersListAsync()
        {
            var usersList = new List<User>();
            var uri = (DeviceInfo.Platform == DevicePlatform.Android) ? APIResources.BaseURLAndroid : APIResources.BaseURL;
            usersList = await GetServerCall<List<User>>(uri, APIResources.GetUsersId) ?? new List<User>();
            return usersList;
        }

        internal static void Disconnect()
        {
            connection.DisposeAsync();
        }

        public static async Task SendMessage(string user, string message)
        {
            await connection.InvokeAsync("SendMessage", user, message);
        }

        public static async Task SendMessageToUser(string userId, string name, string message)
        {
            ReceiveMessage(name, message);
            await connection.InvokeAsync("SendMessageToUser", userId, name, message);
        }

        public static void ReceiveMessage(string user, string message)
        {
            MessagingCenter.Send(user + "|" + message, "ReceiveMessage");
        }

        public async Task<T> GetServerCall<T>(string uri, string endPoint)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            client = new HttpClient(httpClientHandler);
            using var httpClient = client;
            httpClient.BaseAddress = new Uri(uri);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //GET Method  
            HttpResponseMessage response = await httpClient.GetAsync(endPoint);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
        
        public async Task<TResult> PostServerCall<TSender, TResult>(string uri, string endPoint, TSender parameters, string savedToken = null)
        {
            try
            {
                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                client = new HttpClient(httpClientHandler);
                using var httpClient = client;
                httpClient.BaseAddress = new Uri(uri);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                var jsonSerialized = await Task.Run(() => JsonConvert.SerializeObject(parameters));
                HttpResponseMessage response = await httpClient.PostAsync(endPoint, new StringContent(jsonSerialized, Encoding.UTF8, "application/json"));
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<TResult>(json);
                return result;
            }
            catch(Exception e)
            {
                throw new NullReferenceException();
            }
        }
    }
}
