using MVVM.Interfaces;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MVVM.Helpers
{
    public static class Utils
    {
        public static void LogFirebaseEvent(string eventName, string actionName)
        {
            DependencyService.Get<IFirebaseEvents>().LogEvent(eventName, actionName);
        }

        internal static void FirebaseTokenRefreshed(object s, FirebasePushNotificationTokenEventArgs p)
        {
            throw new NotImplementedException();
        }

        internal static void FirebaseOnNotificationReceived(object s, FirebasePushNotificationDataEventArgs p)
        {
            throw new NotImplementedException();
        }

        internal static void FirebaseOnNotificationOpened(object s, FirebasePushNotificationResponseEventArgs p)
        {
            throw new NotImplementedException();
        }
    }
}
