using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public class PeopleClient
    {
        private readonly string UserId;
        private readonly HttpClient client;

        private PeopleClient()
        {
            string project_base_url = FirebaseFirestore.BASE_URL + FirebaseFirestore.PROJECT_URL;
            client = new HttpClient
            {
                BaseAddress = new Uri(project_base_url)
            };
        }

        public PeopleClient(string UserId) : this()
        {
            this.UserId = UserId;
        }

        public PeopleClient(User user) : this(user.id) { }

        /*--------------------------------------
         * @return: fresh list of People
         * If unsuccessfull return an emtpy list
         --------------------------------------*/
        public async Task<List<People>> GetAllPeopleAsync()
        {
            try
            {
                List<People> AllPeople = new List<People>();
                string uri = client.BaseAddress + FirebaseFirestore.USER_URL + "/" + UserId + FirebaseFirestore.PEOPLE_URL;
                Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
                responseTask.Wait();
                if (responseTask.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await responseTask.Result.Content.ReadAsStringAsync();
                    JObject peopleJson = JObject.Parse(content);
                    PeopleParser peopleParser = new PeopleParser();

                    foreach(JToken pJt in peopleJson["documents"])
                    {
                        People peep = peopleParser.JsonToObject(pJt);
                        if (peep != null)
                        {
                            AllPeople.Add(peep);
                        }
                    }
                }
                return AllPeople;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw e;
            }
        }
        /*-----------------------------------
         * @parameter: string: People Id
         * @return: Task of Poeple Object
         -----------------------------------*/
        public async Task<People> GetPeopleFromIdAsync(string id)
        {
            try
            {
                string uri = client.BaseAddress + FirebaseFirestore.USER_URL + "/" + UserId + FirebaseFirestore.PEOPLE_URL+"/"+id;
                Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
                responseTask.Wait();
                if (responseTask.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string content = await responseTask.Result.Content.ReadAsStringAsync();
                    JObject peopleJson = JObject.Parse(content);
                    PeopleParser peopleParser = new PeopleParser();
                    People peep = peopleParser.JsonToObject(peopleJson);
                    return peep;
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw e;
            }
        }

        // Return People object with SpeakerId if found else null
        public async Task<People> GetPeopleFromSpeakerIdAsync(string SpeakerId)
        {
            try
            {
                List<People> AllPeople = await this.GetAllPeopleAsync();
                if(AllPeople==null || AllPeople.Count == 0)
                {
                    return null;
                }
                foreach(People p in AllPeople)
                {
                    if(p.SpeakerId == SpeakerId)
                    {
                        return p;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw e;
            }
        }

        public async Task<People> PostPeopleAsync(People people){
            try
            {
                string uri = client.BaseAddress + FirebaseFirestore.USER_URL + "/" + UserId + FirebaseFirestore.PEOPLE_URL;
                PeopleParser peopleParser = new PeopleParser();
                var jsonObject = peopleParser.ObjectToJson(people);
                string jsonString = jsonObject.ToString();
                var content = new StringContent(jsonObject.ToString(), Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> responseTask = client.PostAsync(uri,content);
                responseTask.Wait();
                if (responseTask.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = await responseTask.Result.Content.ReadAsStringAsync();
                    JObject peopleJson = JObject.Parse(responseContent);
                    People peep = peopleParser.JsonToObject(peopleJson);
                    return peep;
                }
                if (AppConfig.IsDebug())
                {
                    Trace.WriteLine(responseTask.Result.StatusCode + " : " + responseTask.Result.RequestMessage);
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                throw e;
            }
        }

        public async Task<People> PatchPeopleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
