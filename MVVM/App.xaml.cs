using MVVM.Helpers;
using MVVM.Services;
using MVVM.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MVVM
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            OnResume();
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SetAppTheme.SetTheme();
            });
        }

        protected override void OnSleep()
        {
            SetAppTheme.SetTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
        }

        protected override void OnResume()
        {
            SetAppTheme.SetTheme();
            RequestedThemeChanged += App_RequestedThemeChanged;
        }
    }
}
