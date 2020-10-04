using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCaitlin.Services
{
    public class RestService
    {
        private readonly HttpClient client;

        public RestService()
        {
            client = new HttpClient();
        }

        //void SetAddress(string Url)
        //{
        //    client.BaseAddress = new Uri();
        //}

        public async Task<JObject> GetRequest(string uri)
        {
            try
            {
                var responseMessage = await client.GetAsync(uri);
                string responseStr = await responseMessage.Content.ReadAsStringAsync();
                return JObject.Parse(responseStr);
                //return JsonObject.Parse(responseMessage.ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw;
            }
        }

        //public async Task<> PostRequest(string uri, JsonValue json)
        //{
        //    try
        //    {
        //        var responseMessage = await client.PostAsync(uri, new HttpContent())
        //    }
        //}
    }
}
