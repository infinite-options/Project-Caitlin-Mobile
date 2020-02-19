using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjectCaitlin.Models;

namespace ProjectCaitlin.Methods
{
    public class FirestoreMethods
    {
        public string uid;

        public FirestoreMethods(string _uid)
        {
            uid = _uid;
        }

        public async Task LoadUser()
        {
            // reset current user and goals values (in case of reload)
            App.user.routines = new List<routine>();
            App.user.goals = new List<goal>();

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

                JToken userJsonGoalsAndRoutines;
                try
                {
                    userJsonGoalsAndRoutines = userJson["fields"]["goals&routines"]["arrayValue"]["values"];
                }
                catch
                {
                    return;
                }


                App.user.firstName = userJson["fields"]["first_name"]["stringValue"].ToString();
                App.user.lastName = userJson["fields"]["last_name"]["stringValue"].ToString();

                App.user.access_token = userJson["fields"]["google_auth_token"]["stringValue"].ToString();
                App.user.refresh_token = userJson["fields"]["google_refresh_token"]["stringValue"].ToString();


                foreach (JToken jsonGoalOrRoutine in userJsonGoalsAndRoutines)
                {
                    JToken jsonMapGorR = jsonGoalOrRoutine["mapValue"]["fields"];

                    if ((bool)jsonMapGorR["is_available"]["booleanValue"])
                    {
                        if ((bool)jsonMapGorR["is_persistent"]["booleanValue"])
                        {
                            routine routine = new routine();
                            routine.title = jsonMapGorR["title"]["stringValue"].ToString();
                            routine.id = jsonMapGorR["id"]["stringValue"].ToString();
                            routine.photo = jsonMapGorR["photo"]["stringValue"].ToString();

                            App.user.routines.Add(routine);

                            //Console.WriteLine("on Routine: " + routine.id);
                        }
                        else
                        {
                            goal goal = new goal();
                            goal.title = jsonMapGorR["title"]["stringValue"].ToString();
                            goal.id = jsonMapGorR["id"]["stringValue"].ToString();
                            goal.photo = jsonMapGorR["photo"]["stringValue"].ToString();

                            App.user.goals.Add(goal);

                            //Console.WriteLine("Loading Complete");
                        }
                    }
                }

                int routineIdx = 0;
                foreach (routine routine in App.user.routines)
                {
                    LoadTasks(routine.id, routineIdx, "routine");
                    routineIdx++;
                }

                int goalIdx = 0;
                foreach (goal goal in App.user.goals)
                {
                    LoadTasks(goal.id, goalIdx, "goal");
                    goalIdx++;
                }
            }
        }

        public async Task LoadTasks(string routineID, int routineIdx, string routineType)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid + "/goals&routines/" + routineID);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var routineResponse = await content.ReadAsStringAsync();
                JObject taskJson = JObject.Parse(routineResponse);

                JToken jsonActionsAndTasks;
                try
                {
                    jsonActionsAndTasks = taskJson["fields"]["actions&tasks"]["arrayValue"]["values"];
                }
                catch
                {
                    return;
                }

                foreach (JToken jsonAorT in jsonActionsAndTasks)
                {
                    JToken jsonMapAorT = jsonAorT["mapValue"]["fields"];

                    if ((bool)jsonAorT["mapValue"]["fields"]["is_available"]["booleanValue"])
                    {
                        if (routineType == "routine")
                        {
                            task task = new task();
                            task.title = jsonMapAorT["title"]["stringValue"].ToString();
                            task.id = jsonMapAorT["id"]["stringValue"].ToString();
                            task.photo = jsonMapAorT["photo"]["stringValue"].ToString();

                            App.user.routines[routineIdx].tasks.Add(task);

                            //Console.WriteLine("on Task: " + task.id);

                        }
                        else if (routineType == "goal")
                        {
                            action action = new action();
                            action.title = jsonMapAorT["title"]["stringValue"].ToString();
                            action.id = jsonMapAorT["id"]["stringValue"].ToString();
                            action.photo = jsonMapAorT["photo"]["stringValue"].ToString();

                            App.user.goals[routineIdx].actions.Add(action);

                            //Console.WriteLine("on Action: " + action.id);
                        }
                    }
                }

                int taskIdx = 0;
                foreach (task task in App.user.routines[routineIdx].tasks)
                {
                    LoadSteps(routineID, task.id, routineIdx, taskIdx, routineType);
                    taskIdx++;
                }

                int actionIdx = 0;
                foreach (action action in App.user.goals[routineIdx].actions)
                {
                    LoadSteps(routineID, action.id, routineIdx, actionIdx, routineType);
                    actionIdx++;
                }
            }
        }

        public async Task LoadSteps(string routineID, string taskID, int routineIdx, int taskIdx, string routineType)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid + "/goals&routines/" + routineID + "/actions&tasks/" + taskID);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var routineResponse = await content.ReadAsStringAsync();
                JObject stepJson = JObject.Parse(routineResponse);


                JToken jsonInstructionsAndSteps;
                try
                {
                    jsonInstructionsAndSteps = stepJson["fields"]["instructions&steps"]["arrayValue"]["values"];
                }
                catch
                {
                    return;
                }

                foreach (JToken jsonIorS in jsonInstructionsAndSteps)
                {
                    JToken jsonMapIorS = jsonIorS["mapValue"]["fields"];

                    if ((bool)jsonIorS["mapValue"]["fields"]["is_available"]["booleanValue"])
                    {
                        if (routineType == "routine")
                        {
                            step step = new step();
                            step.title = jsonMapIorS["title"]["stringValue"].ToString();
                            step.photo = jsonIorS["mapValue"]["fields"]["photo"]["stringValue"].ToString();

                            //Console.WriteLine("on Step: " + step.title);

                            App.user.routines[routineIdx].tasks[taskIdx].steps.Add(step);
                        }
                        else if (routineType == "goal")
                        {
                            instruction instruction = new instruction();
                            instruction.title = jsonMapIorS["title"]["stringValue"].ToString();
                            instruction.photo = jsonMapIorS["photo"]["stringValue"].ToString();

                            App.user.goals[routineIdx].actions[taskIdx].instructions.Add(instruction);

                            //Console.WriteLine("on Instruction: " + instruction.title);
                        }
                    }
                }
            }
        }
    }
}