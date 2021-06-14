using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.Helpers
{
    public static class SetAppTheme
    {
        public static void SetTheme()
        {
            switch(Settings.Theme)
            {
                case 0:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Unspecified;
                    break;
                case 1:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Light;
                    break;
                case 2:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Dark;
                    break;
            }
        }
    }
}
