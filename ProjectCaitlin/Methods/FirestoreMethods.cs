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
        string uid;

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

                JToken userJsonFields = userJson["fields"];
                JToken userJsonGoalsAndRoutines = userJsonFields["goals&routines"];
                if (userJsonGoalsAndRoutines != null)
                    userJsonGoalsAndRoutines = userJsonFields["goals&routines"]["arrayValue"]["values"];
                else
                    return;

                App.user.firstName = userJsonFields["first_name"]["stringValue"].ToString();
                App.user.lastName = userJsonFields["last_name"]["stringValue"].ToString();

                int routineIdx = 0;
                int goalIdx = 0;
                foreach (JToken jsonGoalOrRoutine in userJsonGoalsAndRoutines)
                {
                    if ((bool)jsonGoalOrRoutine["mapValue"]["fields"]["is_available"]["booleanValue"])
                    {
                        if ((bool)jsonGoalOrRoutine["mapValue"]["fields"]["is_persistent"]["booleanValue"])
                        {
                            routine routine = new routine();
                            routine.title = jsonGoalOrRoutine["mapValue"]["fields"]["title"]["stringValue"].ToString();
                            routine.id = jsonGoalOrRoutine["mapValue"]["fields"]["id"]["stringValue"].ToString();
                            routine.photo = jsonGoalOrRoutine["mapValue"]["fields"]["photo"]["stringValue"].ToString();

                            App.user.routines.Add(routine);
                            await LoadTasks(routine.id, routineIdx, "routine");
                            routineIdx++;
                        }
                        else
                        {
                            goal goal = new goal();
                            goal.title = jsonGoalOrRoutine["mapValue"]["fields"]["title"]["stringValue"].ToString();
                            goal.id = jsonGoalOrRoutine["mapValue"]["fields"]["id"]["stringValue"].ToString();
                            goal.photo = jsonGoalOrRoutine["mapValue"]["fields"]["photo"]["stringValue"].ToString();

                            App.user.goals.Add(goal);
                            await LoadTasks(goal.id, goalIdx, "goal");
                            goalIdx++;
                        }
                    }
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

                JToken jsonActionsAndTasks = taskJson["fields"]["goals&routines"];
                if (jsonActionsAndTasks != null)
                    jsonActionsAndTasks = taskJson["fields"]["goals&routines"]["arrayValue"]["values"];
                else
                    return;

                int taskIdx = 0;
                int actionIdx = 0;
                foreach (JToken jsonAorT in jsonActionsAndTasks)
                {
                    if ((bool)jsonAorT["mapValue"]["fields"]["is_available"]["booleanValue"])
                    {
                        if (routineType == "routine")
                        {
                            task task = new task();
                            task.title = jsonAorT["mapValue"]["fields"]["title"]["stringValue"].ToString();
                            task.id = jsonAorT["mapValue"]["fields"]["id"]["stringValue"].ToString();
                            if (jsonAorT["mapValue"]["fields"]["photo"] != null)
                            {
                                task.photo = jsonAorT["mapValue"]["fields"]["photo"]["stringValue"].ToString();
                            }
                            App.user.routines[routineIdx].tasks.Add(task);
                            await LoadSteps(routineID, task.id, routineIdx, taskIdx, routineType);
                            taskIdx++;

                        }
                        else if (routineType == "goal")
                        {
                            action action = new action();
                            action.title = jsonAorT["mapValue"]["fields"]["title"]["stringValue"].ToString();
                            action.id = jsonAorT["mapValue"]["fields"]["id"]["stringValue"].ToString();
                            if (jsonAorT["mapValue"]["fields"]["photo"] != null)
                            {
                                action.photo = jsonAorT["mapValue"]["fields"]["photo"]["stringValue"].ToString();
                            }

                            App.user.goals[routineIdx].actions.Add(action);
                            await LoadSteps(routineID, action.id, routineIdx, actionIdx, routineType);
                            actionIdx++;
                        }
                    }
                }
            }
        }

        public async Task LoadSteps(string routineID, string taskID, int routineIdx, int taskIdx, string routineType)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid + "/goals&routines/" + routineID + "/instructions&steps/" + taskID);
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var routineResponse = await content.ReadAsStringAsync();
                JObject stepJson = JObject.Parse(routineResponse);

                JToken jsonInstructionsAndSteps = stepJson["fields"]["instructions&steps"];
                if (jsonInstructionsAndSteps != null)
                    jsonInstructionsAndSteps = stepJson["fields"]["instructions&steps"]["arrayValue"]["values"];
                else
                    return;

                foreach (JToken jsonIorS in jsonInstructionsAndSteps)
                {
                    if ((bool)jsonIorS["mapValue"]["fields"]["is_available"]["booleanValue"])
                    {
                        if (routineType == "routine")
                        {
                            step step = new step();
                            step.title = jsonIorS["mapValue"]["fields"]["title"]["stringValue"].ToString();
                            if (jsonIorS["mapValue"]["fields"]["photo"] != null)
                                step.photo = jsonIorS["mapValue"]["fields"]["photo"]["stringValue"].ToString();
                            App.user.routines[routineIdx].tasks[taskIdx].steps.Add(step);

                        }
                        else if (routineType == "goal")
                        {
                            instruction instruction = new instruction();
                            instruction.title = jsonIorS["mapValue"]["fields"]["title"]["stringValue"].ToString();
                            if (jsonIorS["mapValue"]["fields"]["photo"] != null)
                                instruction.photo = jsonIorS["mapValue"]["fields"]["photo"]["stringValue"].ToString();
                            
                            App.user.goals[routineIdx].actions[taskIdx].instructions.Add(instruction);

                        }
                    }
                }
            }
        }
    }
}
