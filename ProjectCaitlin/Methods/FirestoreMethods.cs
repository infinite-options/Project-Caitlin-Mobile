using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjectCaitlin.Models;

namespace ProjectCaitlin.Methods
{
    public class FirestoreMethods
    {
        public async Task<user> LoadUser(string uid)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var userResponse = await content.ReadAsStringAsync();
                JObject userJson = JObject.Parse(userResponse);

                JToken userJsonFields = userJson["fields"];
                JToken userJsonRoutines = userJsonFields["routines"];
                if (userJsonRoutines != null)
                {
                    userJsonRoutines = userJsonFields["routines"]["arrayValue"]["values"];
                }
                JToken userJsonGoals = userJsonFields["goals"];
                if (userJsonGoals != null)
                {
                    userJsonGoals = userJsonFields["goals"]["arrayValue"]["values"];
                }

                App.user.firstName = userJsonFields["first_name"]["stringValue"].ToString();
                App.user.lastName = userJsonFields["last_name"]["stringValue"].ToString();

                foreach (JToken jsonRoutine in userJsonRoutines)
                {
                    routine routine = new routine();
                    routine.title = jsonRoutine["mapValue"]["fields"]["title"]["stringValue"].ToString();
                    routine.id = jsonRoutine["mapValue"]["fields"]["id"]["stringValue"].ToString();

                    App.user.routines.Add(routine);
                }

                foreach (JToken jsonGoal in userJsonGoals)
                {
                    routine goal = new routine();
                    goal.title = jsonGoal["mapValue"]["fields"]["title"]["stringValue"].ToString();
                    goal.id = jsonGoal["mapValue"]["fields"]["id"]["stringValue"].ToString();

                    App.user.goals.Add(goal);
                }
            }
            return App.user;
        }
    }
}
