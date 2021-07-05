using MVVM.Helpers;
using MVVM.Models;
using MVVM.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    [QueryProperty(nameof(Name), nameof(Name))]
    public class ChatViewModel : BaseViewModel
    {
        #region Properties

        private string connectionId;
        public string ConnectionId
        {
            get => connectionId;
            set => SetProperty(ref connectionId, value);
        }

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        private bool isEnabledToSend;
        public bool IsEnabledToSend
        {
            get => isEnabledToSend;
            set => SetProperty(ref isEnabledToSend, value);
        }

        private ObservableCollection<Request> messages;
        public ObservableCollection<Request> Messages
        {
            get => messages;
            set => SetProperty(ref messages, value);
        }

        private List<string> usersList;
        public List<string> UsersList
        {
            get => usersList;
            set => SetProperty(ref usersList, value);
        }

        private string selectedUser;
        public string SelectedUser
        {
            get => selectedUser;
            set => SetProperty(ref selectedUser, value);
        }

        public Command SendCommand { get; }
        public Command OnUserSelected { get; }

        HttpClientService httpService;

        #endregion

        #region Methods

        public ChatViewModel()
        {
            SendCommand = new Command(OnSendMessageClick);
            OnUserSelected = new Command(OnUserSelectedClick);
            Init();
        }

        public async void Init()
        {
            isEnabledToSend = false;
            UsersList = new List<string>();
            Messages = new ObservableCollection<Request>();
            httpService = new HttpClientService();
            httpService.InitChatService();
            UsersList = await httpService.GetUsersListAsync();
            MessagingCenter.Subscribe<string>(this, "ReceiveMessage", (sender) =>
            {
                OnMessageReceived(sender);
            });
            MessagingCenter.Subscribe<object>(this, "UserConnected", (sender) =>
            {
                UpdateUserStatus((Utils.Connected)sender);
            });
        }

        private void UpdateUserStatus(Utils.Connected status)
        {
            switch(status)
            {
                case Utils.Connected.Connected:
                    UsersList = Utils.UsersList;
                    break;

                case Utils.Connected.Disconnected:
                    httpService.Disconnect();
                    break;

                default:
                    break;
            }
        }

        private async void OnSendMessageClick(object obj)
        {
            await httpService.SendMessageToUser(ConnectionId, Name, Message);
            Message = "";
        }

        private void OnUserSelectedClick(object obj)
        {
            ConnectionId = SelectedUser;
            IsEnabledToSend = true;
        }

        public void OnMessageReceived(string request)
        {
            var items = request.Split("|");
            Messages.Insert(0, new Request() { UserName = items[0], Message = items[1] });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            httpService.Disconnect();
        }

        #endregion
    }
}
