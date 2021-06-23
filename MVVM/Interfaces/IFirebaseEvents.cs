using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.Interfaces
{
    public interface IFirebaseEvents
    {
        void LogEvent(string eventName, string actionName);

        void CrashlyticsLogEvent(Exception exception);
    }
}
