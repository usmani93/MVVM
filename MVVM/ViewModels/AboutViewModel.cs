using MVVM.Helpers;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        #region Properties

        public string internetStatus;
        public string InternetStatus
        {
            get => internetStatus;
            set => SetProperty(ref internetStatus, value);
        }

        public ICommand OpenWebCommand { get; }
        public ICommand CheckInternet { get; set; }
        public ICommand CheckCrashlytics { get; set; }

        #endregion

        #region Methods

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            CheckInternet = new Command(CheckInternetStatus);
            CheckCrashlytics = new Command(CheckCrashlyticsException);
        }

        public void CheckInternetStatus()
        {
            switch (Connectivity.NetworkAccess)
            {
                case NetworkAccess.Internet:
                    InternetStatus = "Connected";
                    break;
                default:
                    InternetStatus = "Not Connected";
                    break;
            }
        }

        public void CheckCrashlyticsException()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                Utils.LogCrashlytics(ex);
            }
        }

        #endregion
    }
}