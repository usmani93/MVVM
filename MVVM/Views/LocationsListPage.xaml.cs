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
    public partial class LocationsListPage : ContentPage
    {
        public LocationsListPage()
        {
            InitializeComponent();
            BindingContext = new LocationsListViewModel();
        }
    }
}