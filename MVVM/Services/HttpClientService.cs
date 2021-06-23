using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MVVM.Services
{
    class HttpClientService
    {
        HttpClient client;

        public HttpClientService()
        {

        }

        void InitService()
        {
            client = new HttpClient();
        }
    }
}
