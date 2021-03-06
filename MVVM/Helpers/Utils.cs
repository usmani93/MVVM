using MVVM.Interfaces;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MVVM.Helpers
{
    public static class Utils
    {
        #region Properties

        private static string firebaseToken { get; set; }

        #endregion

        #region Methods

        public static void LogFirebaseEvent(string eventName, string actionName)
        {
            DependencyService.Get<IFirebaseEvents>().LogEvent(eventName, actionName);
        }
        
        public static void LogCrashlytics(Exception exception)
        {
            DependencyService.Get<IFirebaseEvents>().CrashlyticsLogEvent(exception);
        }

        internal static void FirebaseTokenRefreshed(object s, FirebasePushNotificationTokenEventArgs p)
        {
            firebaseToken = p.Token;
        }

        internal static void FirebaseOnNotificationReceived(object s, FirebasePushNotificationDataEventArgs p)
        {
            var payload = p.Data;
        }

        internal static void FirebaseOnNotificationOpened(object s, FirebasePushNotificationResponseEventArgs p)
        {
            var payload = p.Data;
        }

        #endregion
    }
}
