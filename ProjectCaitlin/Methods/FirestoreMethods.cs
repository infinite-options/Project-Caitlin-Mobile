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
        string uid;

        public FirestoreMethods(string _uid)
        {
            uid = _uid;
        }

        public async Task LoadUser()
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

                int routineIdx = 0;
                foreach (JToken jsonRoutine in userJsonRoutines)
                {
                    routine routine = new routine();
                    routine.title = jsonRoutine["mapValue"]["fields"]["title"]["stringValue"].ToString();
                    routine.id = jsonRoutine["mapValue"]["fields"]["id"]["stringValue"].ToString();
                    if (jsonRoutine["mapValue"]["fields"]["photo"] != null)
                    {
                        routine.photo = jsonRoutine["mapValue"]["fields"]["photo"]["stringValue"].ToString();
                    }

                    App.user.routines.Add(routine);
                    await LoadTasks(routine.id, routineIdx, "routine");
                    routineIdx++;
                }

                int goalIdx = 0;
                foreach (JToken jsonGoal in userJsonGoals)
                {
                    routine goal = new routine();
                    goal.title = jsonGoal["mapValue"]["fields"]["title"]["stringValue"].ToString();
                    goal.id = jsonGoal["mapValue"]["fields"]["id"]["stringValue"].ToString();
                    if (jsonGoal["mapValue"]["fields"]["photo"] != null)
                    {
                        goal.photo = jsonGoal["mapValue"]["fields"]["photo"]["stringValue"].ToString();
                    }

                    App.user.goals.Add(goal);
                    await LoadTasks(goal.id, goalIdx, "goal");
                    goalIdx++;
                }
            }
        }

        public async Task LoadTasks(string routineID, int routineIdx, string routineType)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid + "/routines/" + routineID);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var routineResponse = await content.ReadAsStringAsync();
                JObject routineJson = JObject.Parse(routineResponse);

                JToken jsonTasks = routineJson["fields"]["tasks"]["arrayValue"]["values"];

                foreach (JToken jsonTask in jsonTasks)
                {
                    task task = new task();
                    task.title = jsonTask["mapValue"]["fields"]["title"]["stringValue"].ToString();
                    task.id = jsonTask["mapValue"]["fields"]["id"]["stringValue"].ToString();
                    if (jsonTask["mapValue"]["fields"]["photo"] != null)
                    {
                        task.photo = jsonTask["mapValue"]["fields"]["photo"]["stringValue"].ToString();
                    }

                    if (routineType == "routine")
                        App.user.routines[routineIdx].tasks.Add(task);
                    else if (routineType == "goal")
                        App.user.goals[routineIdx].tasks.Add(task);
                }
            }
        }
    }
}
