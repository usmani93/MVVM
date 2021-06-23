using MVVM.Helpers;
using Plugin.FirebasePushNotification;
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
            
            MainPage = new AppShell();

            // Token event
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                Utils.FirebaseTokenRefreshed(s, p);
                
                //for later use
                //System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };

            // Push message received event
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                Utils.FirebaseOnNotificationReceived(s, p);
                //for later use
                //System.Diagnostics.Debug.WriteLine("Received");
            };

            //Push message received event
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                Utils.FirebaseOnNotificationOpened(s, p);
                //for later use
                //System.Diagnostics.Debug.WriteLine("Opened");
                //foreach (var data in p.Data)
                //{
                //    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                //}
            };

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
