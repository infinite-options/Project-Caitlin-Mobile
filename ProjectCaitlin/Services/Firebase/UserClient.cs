using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognition.Config;
using VoiceRecognition.Model;

namespace VoiceRecognition.Services.Firebase
{
    public class UserClient
    {
        private readonly HttpClient client;

        public UserClient()
        {
            string project_base_url = FirebaseFirestore.BASE_URL + FirebaseFirestore.PROJECT_URL;
            client = new HttpClient
            {
                BaseAddress = new Uri(project_base_url)
            };
        }

        public async Task<user> GetUserFromId(string id)
        {
            try
            {
                string uri = client.BaseAddress + FirebaseFirestore.USER_URL + "/" + id;
                Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
                responseTask.Wait();
                if(responseTask.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
                    JObject userJson = JObject.Parse(contentTask.Result);
                    var fname = userJson["fields"]["first_name"]["stringValue"].ToString();
                    if (fname == null) { return null; }
                    user user = new user()
                    {
                        firstName = fname
                    };
                    return user;
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw e;
            }
        }
    }
}
