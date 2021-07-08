using MVVM.Helpers;
using MVVM.Models;
using MVVM.Services;
using MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        #region Properties

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public Command LoginCommand { get; }

        public HttpClientService httpClientService;

        #endregion

        #region Methods

        public LoginViewModel()
        {
            httpClientService = new HttpClientService();
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            try
            {
                IsBusy = true;

                if (obj.ToString() == "User")
                {
                    if (!string.IsNullOrEmpty(Name))
                    {
                        await GetAndVerifyToken(Name);
                        await SetMainPage();
                    }
                    else
                    {
                        MessagingCenter.Send<string>("Enter Username", "ShowDialog");
                    }
                }
                else
                {
                    Name = "Guest";
                    await GetAndVerifyToken(Name);
                    await SetMainPage();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task GetAndVerifyToken(string name)
        {
            var savedToken = await SecureStorage.GetAsync("token");
            if (string.IsNullOrEmpty(savedToken))
            {
                var token = await httpClientService.UpdateAndSendTokenToHubAsync(false, name);
                await SecureStorage.SetAsync("token", token);
            }
            else
            {
                var token = await httpClientService.UpdateAndSendTokenToHubAsync(true, name, savedToken);
                if(string.IsNullOrEmpty(token))
                {
                    token = await httpClientService.UpdateAndSendTokenToHubAsync(false, name);
                }
                await SecureStorage.SetAsync("token", token);
            }
        }

        internal async Task NavigateToChat()
        {
            var task1 = SecureStorage.SetAsync("UserLoggedIn", "1");
            var task2 = SetMainPage();
            await Task.WhenAll(task1, task2);
        }

        async Task SetMainPage()
        {
            Request request = new Request
            {
                UserName = Name
            };
            Utils.Request = request;
            Application.Current.MainPage = new AppShell();
            await Shell.Current.GoToAsync($"//{nameof(UsersList)}");
        }

        #endregion
    }
}
