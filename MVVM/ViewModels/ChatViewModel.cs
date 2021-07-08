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
    public class ChatViewModel : BaseViewModel
    {
        #region Properties

        private Request request;
        public Request UserRequest
        {
            get => request;
            set => SetProperty(ref request, value);
        }

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

        private ObservableCollection<Request> messages;
        public ObservableCollection<Request> Messages
        {
            get => messages;
            set => SetProperty(ref messages, value);
        }        

        public Command SendCommand { get; }
        public Command GoBackCommand { get; }

        #endregion

        #region Methods

        public ChatViewModel()
        {
            UserRequest = Utils.Request;
            Name = UserRequest.UserName;
            ConnectionId = UserRequest.ConnectionId;
            SendCommand = new Command(OnSendMessageClick);
            GoBackCommand = new Command(OnGoBackCommandClick);
            Messages = new ObservableCollection<Request>();
            MessagingCenter.Subscribe<string>(this, "ReceiveMessage", (sender) =>
            {
                OnMessageReceived(sender);
            });
        }

        private void OnGoBackCommandClick(object obj)
        {
            Shell.Current.GoToAsync("..");
        }

        private async void OnSendMessageClick(object obj)
        {
            await HttpClientService.SendMessageToUser(ConnectionId, Name, Message);
            Message = "";
        }

        public void OnMessageReceived(string request)
        {
            var items = request.Split("|");
            Messages.Insert(0, new Request() { UserName = items[0], Message = items[1] });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            HttpClientService.Disconnect();
        }

        #endregion
    }
}