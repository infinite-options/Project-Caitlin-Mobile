using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using ProjectCaitlin.Models;

namespace ProjectCaitlin.Services
{
    public class FirebaseFunctionsService
    {
        public async Task<string> FindUserDoc(string email)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/FindUserDoc"),
                    Method = HttpMethod.Post
                };

                //Format Headers of Request


                var requestData = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "data",
                        new Dictionary<string, string>
                        {
                            {"emailId", email},
                        }
                    },
                };

                string dataString = JsonConvert.SerializeObject(requestData);
                var fromContent = new StringContent(dataString, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {

                    // without async, will get stuck, needs bug fix
                    HttpResponseMessage response = client.PostAsync(request.RequestUri, fromContent).Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        var responseString = await content.ReadAsStringAsync();
                        JObject responseJson = JObject.Parse(responseString);
                        return responseJson["result"]["id"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch
            {
                Console.WriteLine("error while calling find user id");
                return "";
            }
        }

        public async Task<bool> GRUserNotificationSetToTrue(string routineId, string routineIdx, string status)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/GRUserNotificationSetToTrue"),
                    Method = HttpMethod.Post
                };

                //Format Headers of Request
                request.Headers.Add("userId", App.User.id);
                request.Headers.Add("routineId", routineId);
                request.Headers.Add("routineNumber", routineIdx);
                request.Headers.Add("status", status);

                var client = new HttpClient();

                // without async, will get stuck, needs bug fix
                HttpResponseMessage response = client.SendAsync(request).Result;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> startGR(grObject grObject)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(App.User.id)
                                        .GetDocumentAsync();
            var data = document.Data;
            var grArrayData = (List<object>)data["goals&routines"];
            var grData = (IDictionary<string, object>)grArrayData[grObject.dbIdx];

            grData["is_in_progress"] = true;
            grData["is_complete"] = false;
            grData["datetime_started"] = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz");

            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("TODO")
                         .GetDocument("Ou0Yd0UXp6yHBt4Z1nVn")
                         .UpdateDataAsync(data);
            return true;
        }

        public async Task<bool> CompleteRoutine(grObject grObject)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(App.User.id)
                                        .GetDocumentAsync();
            var data = document.Data;
            var grArrayData = (List<object>)data["goals&routines"];
            var grData = (IDictionary<string, object>)grArrayData[grObject.dbIdx];

            grData["is_in_progress"] = false;
            grData["is_complete"] = true;
            grData["datetime_completed"] = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz");

            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("TODO")
                         .GetDocument("Ou0Yd0UXp6yHBt4Z1nVn")
                         .UpdateDataAsync(data);
            return true;
        }

        public async Task<bool> StartAT(atObject atObject)
        {
            var document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(App.User.id)
                                        .GetCollection("goals&routines")
                                        .GetDocument(atObject.grId)
                                        .GetDocumentAsync();
            var data = document.Data;
            var grArrayData = (List<object>)data["goals&routines"];
            var grData = (IDictionary<string, object>)grArrayData[atObject.dbIdx];

            grData["is_in_progress"] = false;
            grData["is_complete"] = true;
            grData["datetime_completed"] = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss \"GMT\"zzz");

            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("TODO")
                         .GetDocument("Ou0Yd0UXp6yHBt4Z1nVn")
                         .UpdateDataAsync(data);
            return true;
        }

        public async Task<bool> StartIS(string goalId, string actionId, string instructionNumber)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/StartInstructionOrStep");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userId", App.User.id);
            request.Headers.Add("routineId", goalId);
            request.Headers.Add("taskId", actionId);
            request.Headers.Add("stepNumber", instructionNumber);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            HttpContent content = response.Content;
            var routineResponse = await content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateStep(string routineId, string taskId, string stepNumber)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteInstructionOrStep"),
                Method = HttpMethod.Post
            };

            //Format Headers of Request with included Token
            request.Headers.Add("userId", App.User.id);
            request.Headers.Add("routineId", routineId);
            request.Headers.Add("taskId", taskId);
            request.Headers.Add("stepNumber", stepNumber);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*public async Task<bool> UpdatePhoto(string photoId,string description,string note)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteInstructionOrStep"),
                Method = HttpMethod.Post
            };

            //Format Headers of Request with included Token
            request.Headers.Add("userId", App.User.id);
            request.Headers.Add("routineId", routineId);
            request.Headers.Add("taskId", taskId);
            request.Headers.Add("stepNumber", stepNumber);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

        public async Task<bool> UpdateTask(string routineId, string taskId, string taskIndex)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteActionOrTask");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userId", App.User.id);
            request.Headers.Add("routineId", routineId);
            request.Headers.Add("taskId", taskId);
            request.Headers.Add("taskNumber", taskIndex);

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            HttpContent content = response.Content;
            var routineResponse = await content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateInstruction(string goalId, string actionId, string instructionNumber)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/CompleteInstructionOrStep");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userId", App.User.id);
            request.Headers.Add("routineId", goalId);
            request.Headers.Add("taskId", actionId);
            request.Headers.Add("stepNumber", instructionNumber);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            HttpContent content = response.Content;
            var routineResponse = await content.ReadAsStringAsync();

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
