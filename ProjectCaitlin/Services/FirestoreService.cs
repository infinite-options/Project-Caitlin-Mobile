using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjectCaitlin.Models;

namespace ProjectCaitlin.Methods
{
    public class FirestoreService
    {
        public string uid;

        public FirestoreService(string _uid)
        {
            uid = _uid;
        }

        public async Task LoadUser()
        {
            // reset current user and goals values (in case of reload)
            App.user.routines = new List<routine>();
            App.user.goals = new List<goal>();

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid),
                Method = HttpMethod.Get
            };
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
                    Console.WriteLine("Error with json goal/routine token:");
                    Console.WriteLine(userJson);
                    return;
                }


                App.user.firstName = userJson["fields"]["first_name"]["stringValue"].ToString();
                App.user.lastName = userJson["fields"]["last_name"]["stringValue"].ToString();

                App.user.access_token = userJson["fields"]["google_auth_token"]["stringValue"].ToString();
                App.user.refresh_token = userJson["fields"]["google_refresh_token"]["stringValue"].ToString();


                foreach (JToken jsonGorR in userJsonGoalsAndRoutines)
                {
                    try
                    {
                        JToken jsonMapGorR = jsonGorR["mapValue"]["fields"];

                        var isDeleted = false;
                        if (jsonMapGorR["deleted"] != null)
                            if ((bool)jsonMapGorR["deleted"]["booleanValue"])
                                isDeleted = true;

                        if ((bool)jsonMapGorR["is_available"]["booleanValue"] && !isDeleted)
                        {
                            if ((bool)jsonMapGorR["is_persistent"]["booleanValue"])
                            {
                                routine routine = new routine
                                {
                                    title = jsonMapGorR["title"]["stringValue"].ToString(),
                                    id = jsonMapGorR["id"]["stringValue"].ToString(),
                                    photo = jsonMapGorR["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapGorR["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapGorR["datetime_completed"]["stringValue"].ToString()),
                                    dateTimeCompleted = DateTime.Parse(jsonMapGorR["datetime_completed"]["stringValue"].ToString()),
                                    availableStartTime = DateTime.ParseExact(jsonMapGorR["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapGorR["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                App.user.routines.Add(routine);

                                //Console.WriteLine("on Routine: " + routine.id);
                            }
                            else
                            {
                                goal goal = new goal
                                {
                                    title = jsonMapGorR["title"]["stringValue"].ToString(),
                                    id = jsonMapGorR["id"]["stringValue"].ToString(),
                                    photo = jsonMapGorR["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapGorR["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapGorR["datetime_completed"]["stringValue"].ToString()),
                                    availableStartTime = DateTime.ParseExact(jsonMapGorR["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapGorR["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                App.user.goals.Add(goal);

                                //Console.WriteLine("on Goal: " + goal.id);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error with json goal/routine token:");
                        Console.WriteLine(jsonGorR);
                    }
                }

                int routineIdx = 0;
                foreach (routine routine in App.user.routines)
                {
                    await LoadTasks(routine.id, routineIdx, "routine");
                    routineIdx++;
                }

                int goalIdx = 0;
                foreach (goal goal in App.user.goals)
                {
                    await LoadTasks(goal.id, goalIdx, "goal");
                    goalIdx++;
                }
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
            request.Headers.Add("userId", "7R6hAVmDrNutRkG3sVRy");
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

        public async Task LoadTasks(string routineID, int routineIdx, string routineType)
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid + "/goals&routines/" + routineID),
                Method = HttpMethod.Get
            };
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
                    Console.WriteLine("Error with json action/task token:");
                    Console.WriteLine(taskJson);
                    return;
                }

                foreach (JToken jsonAorT in jsonActionsAndTasks)
                {
                    try
                    {
                        JToken jsonMapAorT = jsonAorT["mapValue"]["fields"];

                        var isDeleted = false;
                        if (jsonMapAorT["deleted"] != null)
                            if ((bool)jsonMapAorT["deleted"]["booleanValue"])
                                isDeleted = true;

                        if ((bool)jsonAorT["mapValue"]["fields"]["is_available"]["booleanValue"] && !isDeleted)
                        {
                            if (routineType == "routine")
                            {
                                task task = new task
                                {
                                    title = jsonMapAorT["title"]["stringValue"].ToString(),
                                    id = jsonMapAorT["id"]["stringValue"].ToString(),
                                    photo = jsonMapAorT["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapAorT["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapAorT["datetime_completed"]["stringValue"].ToString()),
                                    availableStartTime = DateTime.ParseExact(jsonMapAorT["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapAorT["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                App.user.routines[routineIdx].tasks.Add(task);

                                Console.WriteLine("on Task: " + task.id);

                            }
                            else if (routineType == "goal")
                            {
                                action action = new action
                                {
                                    title = jsonMapAorT["title"]["stringValue"].ToString(),
                                    id = jsonMapAorT["id"]["stringValue"].ToString(),
                                    photo = jsonMapAorT["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapAorT["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapAorT["datetime_completed"]["stringValue"].ToString()),
                                    availableStartTime = DateTime.ParseExact(jsonMapAorT["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapAorT["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                App.user.goals[routineIdx].actions.Add(action);

                                Console.WriteLine("on Action: " + action.id);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error with json action/task token:");
                        Console.WriteLine(jsonAorT);
                    }
                }

                if (routineType == "routine")
                {
                    int taskIdx = 0;
                    foreach (task task in App.user.routines[routineIdx].tasks)
                    {
                        //Console.WriteLine("on Task step load: " + task.id);
                        await LoadSteps(routineID, task.id, routineIdx, taskIdx, routineType);
                        taskIdx++;
                    }
                }
                else
                {
                    int actionIdx = 0;
                    foreach (action action in App.user.goals[routineIdx].actions)
                    {
                        //Console.WriteLine("on action step load: " + action.id);
                        await LoadSteps(routineID, action.id, routineIdx, actionIdx, routineType);
                        actionIdx++;
                    }
                }
                //App.user.routines.Sort((x, y) => TimeSpan.Compare(x.availableStartTime.TimeOfDay, y.availableStartTime.TimeOfDay));
                //App.user.goals.Sort((x, y) => TimeSpan.Compare(x.availableStartTime.TimeOfDay, y.availableStartTime.TimeOfDay));

            }
        }

        public async Task LoadSteps(string routineID, string taskID, int routineIdx, int taskIdx, string routineType)
        {
            var request = new HttpRequestMessage
            {
                RequestUri = new Uri("https://firestore.googleapis.com/v1/projects/project-caitlin-c71a9/databases/(default)/documents/users/" + uid + "/goals&routines/" + routineID + "/actions&tasks/" + taskID),
                Method = HttpMethod.Get
            };
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
                    Console.WriteLine("Error with json action/task token:");
                    Console.WriteLine(stepJson);
                    return;
                }

                foreach (JToken jsonIorS in jsonInstructionsAndSteps)
                {
                    try
                    {
                        JToken jsonMapIorS = jsonIorS["mapValue"]["fields"];

                        var isDeleted = false;
                        if (jsonMapIorS["deleted"] != null)
                            if ((bool)jsonMapIorS["deleted"]["booleanValue"])
                                isDeleted = true;

                        if ((bool)jsonMapIorS["is_available"]["booleanValue"] && !isDeleted)
                        {
                            if (routineType == "routine")
                            {
                                step step = new step
                                {
                                    title = jsonMapIorS["title"]["stringValue"].ToString(),
                                    photo = jsonMapIorS["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapIorS["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapIorS["datetime_completed"]["stringValue"].ToString()),
                                    availableStartTime = DateTime.ParseExact(jsonMapIorS["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapIorS["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                //Console.WriteLine("on Step: " + step.isComplete);

                                App.user.routines[routineIdx].tasks[taskIdx].steps.Add(step);
                            }
                            else if (routineType == "goal")
                            {
                                instruction instruction = new instruction
                                {
                                    title = jsonMapIorS["title"]["stringValue"].ToString(),
                                    photo = jsonMapIorS["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapIorS["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapIorS["datetime_completed"]["stringValue"].ToString()),
                                    availableStartTime = DateTime.ParseExact(jsonMapIorS["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapIorS["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };



                                App.user.goals[routineIdx].actions[taskIdx].instructions.Add(instruction);

                                //Console.WriteLine("on Instruction: " + instruction.isComplete);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error with json action/task token:");
                        Console.WriteLine(jsonIorS);
                    }
                }
            }
        }

        private bool IsDateToday(string dateTimeString)
        {
            //Console.WriteLine("checkDatestring: " + dateTimeString);

            DateTime today = DateTime.Now;
            DateTime checkDate = DateTime.Parse(dateTimeString);

            //Console.WriteLine("checkDate: " + checkDate.ToString());
            //Console.WriteLine("checkDate result: " + (today.Date == checkDate.Date).ToString());

            return (today.Date == checkDate.Date) ? true : false;
        }
    }
}
