using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Plugin.CloudFirestore;
using ProjectCaitlin.Models;
using System.Linq;

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
                var formContent = new StringContent(dataString, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {

                    // without async, will get stuck, needs bug fix
                    HttpResponseMessage response = client.PostAsync(request.RequestUri, formContent).Result;
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
            var data = (IDictionary<string, object>)ConvertDocumentGet(document.Data, "goals%routines");
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

        public static async Task<bool> PostPhoto(string photoId, string description, string note)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/SavePhotoDetails"),
                Method = HttpMethod.Post
            };

            var requestData = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "data",
                        new Dictionary<string, string>
                        {
                            {"userId", App.User.id},
                            {"photoId", photoId},
                            {"desc", description},
                            {"notes", note}
                        }
                    },
                };

            string dataString = JsonConvert.SerializeObject(requestData);
            var formContent = new StringContent(dataString, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                // without async, will get stuck, needs bug fix
                HttpResponseMessage response = client.PostAsync(request.RequestUri, formContent).Result;
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

        public static async Task<photo> GetPhoto(string photoId)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/GetPhotoDetails"),
                Method = HttpMethod.Get
            };

            var requestData = new Dictionary<string, Dictionary<string, string>>
                {
                    {
                        "data",
                        new Dictionary<string, string>
                        {
                            {"userId", App.User.id},
                            {"photoId", photoId}
                        }
                    },
                };

            string dataString = JsonConvert.SerializeObject(requestData);
            var formContent = new StringContent(dataString, Encoding.UTF8, "application/json");
            photo photo = new photo();
            using (var client = new HttpClient())
            {
                // without async, will get stuck, needs bug fix
                HttpResponseMessage response = client.PostAsync(request.RequestUri, formContent).Result;
                var photosResponse = await response.Content.ReadAsStringAsync();
                try
                {
                    JObject photosJson = JObject.Parse(photosResponse);
                    var photo_field = photosJson["result"];

                    photo.id = photo_field["photo_id"]+"";
                    photo.description = photo_field["description"]+"";
                    photo.note = photo_field["notes"]+"";

                    App.User.FirebasePhotos.Add(photo);
                }
                catch
                {
                    Console.WriteLine($"error with photo id: {photoId}");
                }
                return photo;
            }
        }

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

        public object ConvertDocumentGet(IDictionary<string, object> docData, string GratisType)
        {
            var gratisBoolField = new List<string>() { "is_available", "is_complete", "is_in_progress", "is_timed"};

            // create a list of attributes to use
            var gratisConvertField = new List<string>();
            switch (GratisType)
            {
                case "goals&routines":
                    var grBoolFieldAdd = new List<string>() { "is_persistent", "is_sublist_available" };
                    gratisConvertField = (List<string>)gratisBoolField.Concat(grBoolFieldAdd);
                    break;
                case "actions&tasks":
                    var atBoolFieldAdd = new List<string>() { "is_sublist_available" };
                    gratisConvertField = (List<string>)gratisBoolField.Concat(atBoolFieldAdd);
                    break;
                case "instructions&steps":
                    gratisConvertField = gratisBoolField;
                    break;
            }

            var notifyIndivis = new List<string>() {"user_notifications", "ta_notifications"};
            var notifyStateFields = new List<string>() { "before", "during", "after" };
            var notifyBoolFields = new List<string>() { "is_enabled", "is_set" };

            // for about me page
            if (GratisType == "goals&routines")
            {
                if (docData.ContainsKey("about_me"))
                {
                    var aboutMeDict = (IDictionary<string, object>)docData["about_me"];
                    if (aboutMeDict.ContainsKey("have_pic"))
                        aboutMeDict["have_pic"] = BinaryToBool((string)aboutMeDict["have_pic"]);
                }

            }

            // for Gratis Arrays
            if (docData.ContainsKey(GratisType))
            {
                var gratisArrayData = (List<object>)docData["goals&routines"];
                foreach (IDictionary<string, object> gratisDict in gratisArrayData)
                {
                    // Convert GRATIS fields
                    foreach (var field in gratisConvertField)
                    {
                        if (gratisDict.ContainsKey(field))
                            gratisDict[field] = BinaryToBool((string)gratisDict[field]);
                    }

                    // Convert notifications fields
                    foreach (var notifyIndivi in notifyIndivis)
                    {
                        if (gratisDict.ContainsKey(notifyIndivi))
                        {
                            var notifyDict = (IDictionary<string, object>)gratisDict[notifyIndivi];
                            foreach (var state in notifyStateFields)
                            {
                                if (notifyDict.ContainsKey(state))
                                {
                                    var notifyStateDict = (IDictionary<string, object>)notifyDict[state];
                                    foreach (var notifyBoolField in notifyBoolFields)
                                    {
                                        if (notifyStateDict.ContainsKey(notifyBoolField))
                                        {
                                            notifyStateDict[notifyBoolField] = BinaryToBool((string)notifyStateDict[notifyBoolField]);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return docData;
        }

        private bool BinaryToBool(string binary)
        {
            return (binary == "1") ? true : false;
        }
    }
}
