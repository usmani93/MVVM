using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM.Services
{
    public class ClientService
    {
        HttpClient client;

        void InitClient()
        {
            client = new HttpClient();
            
        }

        public async Task<IEnumerable<T>> GetData(string uri)
        {
            HttpResponseMessage response = await client.GetAsync(uri);
            if(response.IsSuccessStatusCode)
            {

            }
        }
    }
}
