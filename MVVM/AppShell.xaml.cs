using MVVM.Helpers;
using MVVM.ViewModels;
using MVVM.Views;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVVM
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(LocationsListPage), typeof(LocationsListPage));
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await SecureStorage.SetAsync("UserLoggedIn", "0");
            MessagingCenter.Send<object>(Utils.Connected.Disconnected, "UserConnected");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
