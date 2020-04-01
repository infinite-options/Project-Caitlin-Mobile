using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ProjectCaitlin.Models;
using Xamarin.Forms;

namespace ProjectCaitlin.Services
{
    public class FirestoreService
    {
        public string uid;

        GoogleService googleService;

        FirebaseFunctionsService firebaseFunctionsService;

        INotificationManager notificationManager;

        public FirestoreService(string _uid)
        {
            App.User.id = uid;
            uid = _uid;
            notificationManager = DependencyService.Get<INotificationManager>();
            googleService = new GoogleService();
            firebaseFunctionsService = new FirebaseFunctionsService();
        }

        public async Task LoadUser()
        {
            // reset current user and goals values (in case of reload)
            App.User.routines = new List<routine>();
            App.User.goals = new List<goal>();

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
                    if (userJsonGoalsAndRoutines == null)
                        return;
                }
                catch
                {
                    Console.WriteLine("Error with json goal/routine token:");
                    Console.WriteLine(userJson);
                    return;
                }


                App.User.firstName = userJson["fields"]["first_name"]["stringValue"].ToString();
                App.User.lastName = userJson["fields"]["last_name"]["stringValue"].ToString();

                try
                {
                    App.User.access_token = userJson["fields"]["google_auth_token"]["stringValue"].ToString();
                    App.User.refresh_token = userJson["fields"]["google_refresh_token"]["stringValue"].ToString();
                }
                catch
                {
                    await googleService.RefreshToken();
                }

                DateTime currentTime = DateTime.Now;

                notificationManager.PrintPendingNotifications();

                int dbIdx_ = 0;
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

                                    dbIdx = dbIdx_,

                                    dateTimeCompleted = DateTime.Parse(jsonMapGorR["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),

                                    availableStartTime = DateTime.ParseExact(jsonMapGorR["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),

                                    availableEndTime = DateTime.ParseExact(jsonMapGorR["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                //time precised in minutes, can be positive or negative.
                                int startTime = (int)(currentTime - routine.availableStartTime).TotalMinutes;
                                int endTime = (int)(currentTime - routine.availableEndTime).TotalMinutes;

                                Console.WriteLine("start time : " + startTime);
                                Console.WriteLine("end time : " + endTime);

                                if( startTime < 0 && startTime > -5)
                                    notificationManager.ScheduleNotification("Ready for ", routine.title + "? Open the app to review your tasks.", 1);
                                else if (!routine.isComplete && endTime > 0 && endTime < 30 )
                                    notificationManager.ScheduleNotification("You Missed a Routine! ", routine.title + " is overdue. Open the app to review your tasks.", 1);
                                else if (!routine.isComplete &&  currentTime > routine.availableStartTime && currentTime < routine.availableEndTime)
                                    notificationManager.ScheduleNotification("Time for ", routine.title + ". Open the app to review your tasks.", 1);

                               /* if (!(bool)jsonMapGorR["user_notification_set"]["booleanValue"]
                                    && (bool)jsonMapGorR["reminds_user"]["booleanValue"]
                                    )
                                {
                                    if (firebaseFunctionsService.GRUserNotificationSetToTrue(routine.id, routine.dbIdx.ToString()).Result)
                                    {
                                        string title = "You Missed a Routine!";
                                        //double duration = (routine.availableEndTime.TimeOfDay - DateTime.Now.TimeOfDay).TotalSeconds;
                                        double duration = 10;
                                        string message = routine.title + " is overdue. Open the app to review your tasks.";
                                        Console.WriteLine("duration: " + duration);
                                        if (duration > 0)
                                        {
                                            Console.WriteLine("notification id: " + notificationManager.ScheduleNotification(title, message, duration));
                                        }
                                    }
                                }*/

                                App.User.routines.Add(routine);

                                //Console.WriteLine("on Routine: " + routine.id);
                            }
                            else
                            {
                                goal goal = new goal
                                {
                                    title = jsonMapGorR["title"]["stringValue"].ToString(),
                                    id = jsonMapGorR["id"]["stringValue"].ToString(),
                                    photo = jsonMapGorR["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapGorR["is_complete"]["booleanValue"],
                                    dbIdx = dbIdx_,
                                    dateTimeCompleted = DateTime.Parse(jsonMapGorR["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = DateTime.ParseExact(jsonMapGorR["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapGorR["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                App.User.goals.Add(goal);

                                //Console.WriteLine("on Goal: " + goal.id);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error with json goal/routine token:");
                        Console.WriteLine(jsonGorR);
                    }
                    dbIdx_++;
                }

                App.User.routines.Sort((x, y) => TimeSpan.Compare(x.availableStartTime.TimeOfDay, y.availableStartTime.TimeOfDay));
                App.User.goals.Sort((x, y) => TimeSpan.Compare(x.availableStartTime.TimeOfDay, y.availableStartTime.TimeOfDay));

                int routineIdx = 0;
                foreach (routine routine in App.User.routines)
                {
                    _ = LoadTasks(routine.id, routineIdx, "routine");
                    routineIdx++;
                }

                int goalIdx = 0;
                foreach (goal goal in App.User.goals)
                {
                    _ = LoadTasks(goal.id, goalIdx, "goal");
                    goalIdx++;
                }
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
                    if (jsonActionsAndTasks == null)
                        return;
                }
                catch
                {
                    Console.WriteLine("Error with json action/task token:");
                    Console.WriteLine(taskJson);
                    return;
                }

                int dbIdx_ = 0;
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
                                    dbIdx = dbIdx_,
                                    dateTimeCompleted = DateTime.Parse(jsonMapAorT["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = DateTime.ParseExact(jsonMapAorT["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapAorT["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                App.User.routines[routineIdx].tasks.Add(task);

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
                                    dbIdx = dbIdx_,
                                    dateTimeCompleted = DateTime.Parse(jsonMapAorT["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = DateTime.ParseExact(jsonMapAorT["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapAorT["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                App.User.goals[routineIdx].actions.Add(action);

                                Console.WriteLine("on Action: " + action.id);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error with json action/task token:");
                        Console.WriteLine(jsonAorT);
                    }
                    dbIdx_++;
                }

                if (routineType == "routine")
                {
                    int taskIdx = 0;
                    foreach (task task in App.User.routines[routineIdx].tasks)
                    {
                        //Console.WriteLine("on Task step load: " + task.id);
                        _ = LoadSteps(routineID, task.id, routineIdx, taskIdx, routineType);
                        taskIdx++;
                    }
                }
                else
                {
                    int actionIdx = 0;
                    foreach (action action in App.User.goals[routineIdx].actions)
                    {
                        //Console.WriteLine("on action step load: " + action.id);
                        _ = LoadSteps(routineID, action.id, routineIdx, actionIdx, routineType);
                        actionIdx++;
                    }
                }

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
                    if (jsonInstructionsAndSteps == null)
                        return;
                }
                catch
                {
                    Console.WriteLine("Error with json action/task token:");
                    Console.WriteLine(stepJson);
                    return;
                }

                int dbIdx_ = 0;
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
                                    dbIdx = dbIdx_,
                                    dateTimeCompleted = DateTime.Parse(jsonMapIorS["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = DateTime.ParseExact(jsonMapIorS["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapIorS["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };

                                //Console.WriteLine("on Step: " + step.isComplete);

                                App.User.routines[routineIdx].tasks[taskIdx].steps.Add(step);
                            }
                            else if (routineType == "goal")
                            {
                                instruction instruction = new instruction
                                {
                                    title = jsonMapIorS["title"]["stringValue"].ToString(),
                                    photo = jsonMapIorS["photo"]["stringValue"].ToString(),
                                    isComplete = (bool)jsonMapIorS["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapIorS["datetime_completed"]["stringValue"].ToString()),
                                    dbIdx = dbIdx_,
                                    dateTimeCompleted = DateTime.Parse(jsonMapIorS["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = DateTime.ParseExact(jsonMapIorS["available_start_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture),
                                    availableEndTime = DateTime.ParseExact(jsonMapIorS["available_end_time"]["stringValue"].ToString(),
                                        "HH:mm:ss", CultureInfo.InvariantCulture)
                                };



                                App.User.goals[routineIdx].actions[taskIdx].instructions.Add(instruction);

                                //Console.WriteLine("on Instruction: " + instruction.isComplete);
                            }
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Error with json action/task token:");
                        Console.WriteLine(jsonIorS);
                    }
                    dbIdx_++;
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
