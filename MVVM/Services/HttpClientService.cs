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
        public string token = "";
        string url = (DeviceInfo.Platform == DevicePlatform.Android) ? APIResources.URLForHUBAndroid : APIResources.URLForHUB;
        HttpClient client;
        HubConnection connection;
        HubConnectionBuilder hubConnectionBuilder;

        public HttpClientService()
        {
        }

        public async void InitChatService()
        {
            try
            {
                hubConnectionBuilder = new HubConnectionBuilder();
                connection = hubConnectionBuilder
                    .WithUrl(url, (opts) =>
                    {
                        opts.AccessTokenProvider = async () =>
                        {
                            return token;
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

                connection.On<List<string>>("ConnectedUsers", (sender) =>
                {
                    //user connected/disconnected
                    sender.Remove(connection.ConnectionId);
                    Utils.UsersList = sender;
                    MessagingCenter.Send<object>(Utils.Connected.Connected, "UserConnected");
                });
            }
            catch (Exception e)
            {

            }
        }

        public async Task<string> UpdateAndSendTokenToHubAsync()
        {
            var uri = (DeviceInfo.Platform == DevicePlatform.Android) ? APIResources.BaseURLAndroid : APIResources.BaseURL;
            var token = await PostServerCall<string>(uri, APIResources.Authenticate);
            return token;
        }

        public async Task<List<string>> GetUsersListAsync()
        {
            var usersList = new List<string>();
            var uri = (DeviceInfo.Platform == DevicePlatform.Android) ? APIResources.BaseURLAndroid : APIResources.BaseURL;
            usersList = await GetServerCall<List<string>>(uri, APIResources.GetUsersId) ?? new List<string>();
            return usersList;
        }

        internal void Disconnect()
        {
            connection.DisposeAsync();
        }

        public async Task SendMessage(string user, string message)
        {
            await connection.InvokeAsync("SendMessage", user, message);
        }

        public async Task SendMessageToUser(string userId, string name, string message)
        {
            ReceiveMessage(name, message);
            await connection.InvokeAsync("SendMessageToUser", userId, name, message);
        }

        public void ReceiveMessage(string user, string message)
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
        
        public async Task<T> PostServerCall<T>(string uri, string endPoint)
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
    }
}
