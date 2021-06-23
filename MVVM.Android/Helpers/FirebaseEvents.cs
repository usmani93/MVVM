using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Analytics;
using MVVM.Droid.Helpers;
using MVVM.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(FirebaseEvents))]
namespace MVVM.Droid.Helpers
{
    public class FirebaseEvents : IFirebaseEvents
    {
        public void CrashlyticsLogEvent(Exception exception)
        {
            var crashlytics = Firebase.Crashlytics.FirebaseCrashlytics.Instance;
            crashlytics.Log(exception.Message);
        }

        public void LogEvent(string eventName, string actionName)
        {
            var analytics = FirebaseAnalytics.GetInstance(MainActivity.Context);
            var bundle = new Bundle();
            bundle.PutString(eventName, actionName);

            analytics.LogEvent("Event", bundle);
        }
    }
}