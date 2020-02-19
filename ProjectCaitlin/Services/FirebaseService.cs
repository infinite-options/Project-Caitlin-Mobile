using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ProjectCaitlin;
using ProjectCaitlin.Authentication;
using Newtonsoft.Json.Linq;

namespace ProjectCaitlin.Services
{
    public class FirebaseService
    {
        protected async Task LoadFirestore()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/7R6hAVmDrNutRkG3sVRy");
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var mealsString = await content.ReadAsStringAsync();
                JObject meals = JObject.Parse(mealsString);
                //Console.WriteLine("Firebase:" + meals["fields"]["last_name"]["stringValue"].ToString());
            }


        }

        public async Task GetActivities()
        {

            //var request = new HttpRequestMessage();
            //request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/7R6hAVmDrNutRkG3sVRy/routines");
            //request.Method = HttpMethod.Get;
            //var client = new HttpClient();
            //HttpResponseMessage response = await client.SendAsync(request);
            //HttpContent content = response.Content;
            //var json = await content.ReadAsStringAsync();

            //var result = JsonConvert.DeserializeObject<Methods.GetFbActivitiesMethod>(json);

            //var itemList = new List<string>();

            //try
            //{
            //     foreach (var activities in result.Documents[0].Fields.Tasks.ArrayValue.Values)
            //     {
            //         itemList.Add(activities.MapValue.Fields.Title.StringValue);
            //         //itemList.Add(activities.MapValue.Fields.Status.StringValue);
            //     }
            //}

            //catch (NullReferenceException e)
            // {
            //   return (null);
            // }

            // string eventNameString = String.Join(", ", itemList);
            //System.Diagnostics.Debug.WriteLine(eventNameString);
            // return eventNameString;
            
        }
    }
}
