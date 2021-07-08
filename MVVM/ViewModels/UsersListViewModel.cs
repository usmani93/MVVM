using MVVM.Helpers;
using MVVM.Models;
using MVVM.Services;
using MVVM.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    public class UsersListViewModel : BaseViewModel
    {
        #region Properties

        private Request request;
        public Request RequestOfUser
        {
            get => request;
            set => SetProperty(ref request, value);
        }

        private List<User> usersList;
        public List<User> UsersList
        {
            get => usersList;
            set => SetProperty(ref usersList, value);
        }

        private User selectedUser;
        public User SelectedUser
        {
            get => selectedUser;
            set => SetProperty(ref selectedUser, value);
        }

        public Command OnUserSelected { get; }
        
        HttpClientService httpService;

        #endregion

        #region Methods

        public UsersListViewModel()
        {
            OnUserSelected = new Command(OnUserSelectedClick);
            Init();
        }

        async void Init()
        {
            RequestOfUser = Utils.Request;
            UsersList = new List<User>();
            httpService = new HttpClientService();
            HttpClientService.InitChatService();
            UsersList = await httpService.GetUsersListAsync();
            MessagingCenter.Subscribe<object>(this, "UserConnected", (sender) =>
            {
                UpdateUserStatus((Utils.Connected)sender);
            });
        }

        private void UpdateUserStatus(Utils.Connected status)
        {
            switch (status)
            {
                case Utils.Connected.Connected:
                    UsersList = Utils.UsersList;
                    break;

                case Utils.Connected.Disconnected:
                    HttpClientService.Disconnect();
                    break;

                default:
                    break;
            }
        }

        private async void OnUserSelectedClick(object obj)
        {
            if (SelectedUser != null)
            {
                Utils.Request.ConnectionId = RequestOfUser.ConnectionId = SelectedUser.connectionId;
                await Shell.Current.GoToAsync($"/{nameof(ChatPage)}");
            }
            else
            {
                return;
            }
        }

        #endregion
    }
}
