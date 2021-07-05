using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.Helpers
{
    public static class APIResources
    {
        public static string BaseURL = "https://localhost:5001";
        public static string BaseURLAndroid = "https://10.0.2.2:5001";
        public static string URLForHUBAndroid = "https://10.0.2.2:5001/chatserver"; 
        public static string URLForHUB = "http://localhost:5001/chatserver";

        public static string GetUsersId = "/api/chatserver/getusers";
        public static string Authenticate = "api/chatserver/authenticate";
    }
}
