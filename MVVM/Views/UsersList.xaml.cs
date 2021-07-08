using MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MVVM.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersList : ContentPage
    {
        public UsersList()
        {
            InitializeComponent();
            this.BindingContext = new UsersListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            onlineUsersList.SelectedItem = null;
        }
    }
}