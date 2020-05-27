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
        public async Task<string> FindUserDocAsync(string email)
        {
            email = email.ToLower();
            String[] emailSplit = email.Split('@');
            emailSplit[0].Replace(".", string.Empty);
            email = emailSplit[0] + "@" + emailSplit[1];

            var userDoc = await CrossCloudFirestore.Current.Instance.GetCollection("users")
                                    .WhereEqualsTo("email_id", email)
                                    .GetDocumentsAsync();
            var userId = "";
            foreach (var user in userDoc.Documents)
            {
                return user.Reference.Id;
            }

            return "";
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

        public async Task<bool> updateGratisStatus(GratisObject gratisObject, string gratisType, bool completionState)
        {
            IDocumentSnapshot document = null;
            switch (gratisType)
            {
                case "goals&routines":
                    document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(App.User.id)
                                        .GetDocumentAsync();
                    break;
                case "actions&tasks":
                    document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(App.User.id)
                                        .GetCollection("goals&routines")
                                        .GetDocument(((atObject)gratisObject).grId)
                                        .GetDocumentAsync();
                    break;
                case "instructions&steps":
                    document = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(App.User.id)
                                        .GetCollection("goals&routines")
                                        .GetDocument(((isObject)gratisObject).grId)
                                        .GetCollection("actions&tasks")
                                        .GetDocument(((isObject)gratisObject).atId)
                                        .GetDocumentAsync();
                    break;
            }

            var data = (IDictionary<string, object>)ConvertDocumentGet(document.Data, gratisType);
            var grArrayData = (List<object>)data[gratisType];
            var grData = (IDictionary<string, object>)grArrayData[gratisObject.dbIdx];

            string completionDateKey = completionState ? "datetime_completed" : "datetime_started";

            grData["is_in_progress"] = !completionState;
            grData["is_complete"] = completionState;
            grData[completionDateKey] = DateTime.Now.ToString("ddd, dd MMM yyy HH:mm:ss 'GMT'");

            switch (gratisType)
            {
                case "goals&routines":
                    await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("users")
                        .GetDocument(App.User.id)
                        .UpdateDataAsync(data);
                    break;
                case "actions&tasks":
                    await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("users")
                        .GetDocument(App.User.id)
                        .GetCollection("goals&routines")
                        .GetDocument(((atObject)gratisObject).grId)
                        .UpdateDataAsync(data);
                    break;
                case "instructions&steps":
                    await CrossCloudFirestore.Current
                        .Instance
                        .GetCollection("users")
                        .GetDocument(App.User.id)
                        .GetCollection("goals&routines")
                        .GetDocument(((isObject)gratisObject).grId)
                        .GetCollection("actions&tasks")
                        .GetDocument(((isObject)gratisObject).atId)
                        .UpdateDataAsync(data);
                    break;
            }
            return true;
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

        public object ConvertDocumentGet(IDictionary<string, object> docData, string GratisType)
        {
            var gratisBoolField = new List<string>() { "is_available", "is_complete", "is_in_progress", "is_timed"};

            // create a list of attributes to use
            var gratisConvertField = new List<string>();
            switch (GratisType)
            {
                case "goals&routines":
                    var grBoolFieldAdd = new List<string>() { "is_persistent", "is_sublist_available", "repeat" };
                    gratisConvertField = gratisBoolField.Concat(grBoolFieldAdd).ToList();
                    break;
                case "actions&tasks":
                    var atBoolFieldAdd = new List<string>() { "is_sublist_available" };
                    gratisConvertField = gratisBoolField.Concat(atBoolFieldAdd).ToList();
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
                        aboutMeDict["have_pic"] = BinaryToBool(aboutMeDict["have_pic"].ToString());
                }

            }

            // for Gratis Arrays
            if (docData.ContainsKey(GratisType))
            {
                var gratisArrayData = (List<object>)docData[GratisType];
                foreach (IDictionary<string, object> gratisDict in gratisArrayData)
                {
                    // Convert GRATIS fields
                    foreach (var field in gratisConvertField)
                    {
                        if (gratisDict.ContainsKey(field))
                            gratisDict[field] = BinaryToBool(gratisDict[field].ToString());
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
                                            notifyStateDict[notifyBoolField] = BinaryToBool(notifyStateDict[notifyBoolField].ToString());
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
