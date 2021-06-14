using MVVM.Helpers;
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
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            switch(Settings.Theme)
            {
                case 0:
                    RadioButtonSystem.IsChecked = true;
                    break;
                case 1:
                    RadioButtonLight.IsChecked = true;
                    break;
                case 2:
                    RadioButtonDark.IsChecked = true;
                    break;
            }
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!e.Value)
                return;

            var val = (sender as RadioButton)?.Value as string;
            switch(val)
            {
                case "System":
                    Settings.Theme = 0;
                    break;

                case "Light":
                    Settings.Theme = 1;
                    break;

                case "Dark":
                    Settings.Theme = 2;
                    break;
            }

            SetAppTheme.SetTheme();
        }
    }
}