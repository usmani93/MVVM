using MVVM.Helpers;
using MVVM.Interfaces;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MVVM.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Utils.LogFirebaseEvent(nameof(OnAppearing), "Page Start");
        }
    }
}