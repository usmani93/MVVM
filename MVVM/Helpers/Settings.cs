using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MVVM.Helpers
{
    public static class Settings
    {
        #region Properties 

        const int theme = 0;

        #endregion

        #region Methods

        public static int Theme
        {
            get => Preferences.Get(nameof(Theme), theme);
            set => Preferences.Set(nameof(Theme), value);
        }

        #endregion
    }
}