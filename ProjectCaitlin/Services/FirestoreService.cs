﻿using System;
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

        public FirestoreService()
        {
            uid = App.User.id;
            notificationManager = DependencyService.Get<INotificationManager>();
            googleService = new GoogleService();
            firebaseFunctionsService = new FirebaseFunctionsService();
        }

        public async Task LoadUser()
        {
            Console.WriteLine("In Load User");
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

                // About me
                JToken userAboutMe;
                try
                {
                    userAboutMe = userJson["fields"]["about_me"]["mapValue"]["fields"];
                    if (userAboutMe == null)
                        return;
                }
                catch
                {
                    //Console.WriteLine("Error with json goal/routine token:");
                    //Console.WriteLine(userJson);
                    return;
                }

                App.User.Me.have_pic = (bool) userAboutMe["have_pic"]["booleanValue"];
                App.User.Me.message_day = userAboutMe["message_day"]["stringValue"].ToString();
                App.User.Me.message_card = userAboutMe["message_card"]["stringValue"].ToString();
                App.User.Me.pic = userAboutMe["pic"]["stringValue"].ToString();

                //int peopleIdx = 0;
                if (userAboutMe["important_people"] != null)
                {
                    foreach (JToken jsonPeople in userAboutMe["important_people"]["arrayValue"]["values"])
                    {
                        try
                        {
                            String people_id = jsonPeople["referenceValue"].ToString();
                            // Console.WriteLine(jsonPeople["referenceValue"]);
                            var request_people = new HttpRequestMessage
                            {
                                RequestUri = new Uri("https://firestore.googleapis.com/v1/" + people_id),
                                Method = HttpMethod.Get
                            };
                            var client_people = new HttpClient();
                            HttpResponseMessage response_people = await client.SendAsync(request_people);


                            if (response_people.StatusCode == System.Net.HttpStatusCode.OK)
                            {
                                HttpContent content_people = response_people.Content;
                                var peopleResponse = await content_people.ReadAsStringAsync();
                                JObject peopleJson = JObject.Parse(peopleResponse);

                                people people = new people();

                                people.have_pic = (bool)peopleJson["fields"]["have_pic"]["booleanValue"];
                                people.name = peopleJson["fields"]["name"]["stringValue"].ToString();
                                people.phone_number = peopleJson["fields"]["phone_number"]["stringValue"].ToString();
                                people.pic = peopleJson["fields"]["pic"]["stringValue"].ToString();
                                people.unique_id = peopleJson["fields"]["unique_id"]["stringValue"].ToString();


                                //Console.WriteLine("People values");
                                App.User.Me.peoples.Add(people);

                                /*Console.WriteLine("People Values");
                                Console.WriteLine(peopleJson["fields"]["name"]["stringValue"].ToString());
                                Console.WriteLine(peopleJson["createTime"]);*/

                            }
                        }
                        catch
                        {

                        }
                    }
                }
                // Goals and routines
                JToken userJsonGoalsAndRoutines;
                try
                {
                    userJsonGoalsAndRoutines = userJson["fields"]["goals&routines"]["arrayValue"]["values"];
                    if (userJsonGoalsAndRoutines == null)
                        return;
                }
                catch
                {
                    //Console.WriteLine("Error with json goal/routine token:");
                    //Console.WriteLine(userJson);
                    return;
                }

                App.User.firstName = userJson["fields"]["first_name"]["stringValue"].ToString();
                App.User.lastName = userJson["fields"]["last_name"]["stringValue"].ToString();

                TimeSpan currentTime = DateTime.Now.TimeOfDay;
                int dbIdx_ = 0;
                int routineIdx = 0;
                int goalIdx = 0;
                foreach (JToken jsonGorR in userJsonGoalsAndRoutines)
                {
                    try
                    {
                        JToken jsonMapGorR = jsonGorR["mapValue"]["fields"];

                        Console.WriteLine("routine: " + jsonMapGorR["title"]["stringValue"].ToString());

                        var isDeleted = false;
                        if (jsonMapGorR["deleted"] != null)
                            if ((bool)jsonMapGorR["deleted"]["booleanValue"])
                                isDeleted = true;

                        if ((bool)jsonMapGorR["is_available"]["booleanValue"] && !isDeleted)
                        {
                            bool isInProgressCheck = (jsonMapGorR["is_in_progress"] == null) ? false : (bool)jsonMapGorR["is_in_progress"]["booleanValue"];
                            if ((bool)jsonMapGorR["is_persistent"]["booleanValue"])
                            {
                                routine routine = new routine
                                {
                                    title = jsonMapGorR["title"]["stringValue"].ToString(),

                                    id = jsonMapGorR["id"]["stringValue"].ToString(),

                                    photo = jsonMapGorR["photo"]["stringValue"].ToString(),

                                    isInProgress = isInProgressCheck
                                        && IsDateToday(jsonMapGorR["datetime_started"]["stringValue"].ToString()),

                                    isComplete = (bool)jsonMapGorR["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapGorR["datetime_completed"]["stringValue"].ToString())
                                        && !isInProgressCheck,

                                    expectedCompletionTime = TimeSpan.Parse(jsonMapGorR["expected_completion_time"]["stringValue"].ToString()),

                                    dbIdx = dbIdx_,

                                    isSublistAvailable = (bool)jsonMapGorR["is_sublist_available"]["booleanValue"],

                                    dateTimeCompleted = DateTime.Parse(jsonMapGorR["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),

                                    availableStartTime = TimeSpan.Parse(jsonMapGorR["available_start_time"]["stringValue"].ToString()),

                                    availableEndTime = TimeSpan.Parse(jsonMapGorR["available_end_time"]["stringValue"].ToString())
                                };

                                //time precised in minutes, can be positive or negative.
                                int startTime = (int)(currentTime - routine.availableStartTime).TotalMinutes;
                                int endTime = (int)(currentTime - routine.availableEndTime).TotalMinutes;

                                JToken userNotification;
                                try
                                {
                                    //Console.WriteLine("jsonMapGorR" + jsonMapGorR["user_notifications"]["mapValue"]["fields"]);
                                    userNotification = jsonMapGorR["user_notifications"]["mapValue"]["fields"];
                                    if (userNotification == null)
                                        return;
                                    routine.Notification = new Notification();
                                    routine.Notification.user = new NotificationTime();

                                    JToken userBefore = userNotification["before"];
                                    JToken userBeforeMap = userBefore["mapValue"]["fields"];

                                    routine.Notification.user.before.is_set = (bool)userBeforeMap["is_set"]["booleanValue"]
                                        && ((userBeforeMap["date_set"] != null) ? IsDateToday(userBeforeMap["date_set"]["stringValue"].ToString()) : false);

                                    routine.Notification.user.before.is_enabled = (bool)userBeforeMap["is_enabled"]["booleanValue"];

                                    if (routine.Notification.user.before.is_enabled && !routine.Notification.user.before.is_set)
                                    {

                                        routine.Notification.user.before.time = TimeSpan.Parse(userBeforeMap["time"]["stringValue"].ToString());
                                        //TotalMinutes

                                        double total = routine.Notification.user.before.time.TotalSeconds - (routine.availableStartTime - DateTime.Now.TimeOfDay).TotalSeconds;

                                        //routine.Notification.user.before.message = userBefore["message"]["stringValue"].ToString();

                                        if (!routine.isComplete && total > 0 && !routine.Notification.user.before.is_set)
                                        {
                                            notificationManager.ScheduleNotification(routineIdx + "Ready for ", routine.title + "? Open the app to review your tasks." + routine.Notification.user.before.message, total);
                                            firebaseFunctionsService.GRUserNotificationSetToTrue(routine.id, routine.dbIdx.ToString(), "before");

                                        }
                                        Console.WriteLine("total : " + total);
                                        Console.WriteLine("before message: " + routine.Notification.user.before.message);
                                    }

                                    JToken userDuring = userNotification["during"];
                                    JToken userDuringMap = userDuring["mapValue"]["fields"];

                                    routine.Notification.user.during.is_set = (bool)userDuringMap["is_set"]["booleanValue"]
                                        && (userDuringMap["date_set"] != null) ? IsDateToday(userDuringMap["date_set"]["stringValue"].ToString()) : false;

                                    routine.Notification.user.during.is_enabled = (bool)userDuringMap["is_enabled"]["booleanValue"];

                                    if (routine.Notification.user.during.is_enabled && !routine.Notification.user.during.is_set)
                                    {
                                        routine.Notification.user.during.time = TimeSpan.Parse(userDuringMap["time"]["stringValue"].ToString());
                                        //TotalMinutes
                                        double total = routine.Notification.user.during.time.TotalSeconds + (routine.availableStartTime - DateTime.Now.TimeOfDay).TotalSeconds;
                                        routine.Notification.user.during.message = userDuringMap["message"]["stringValue"].ToString();

                                        if (!routine.isComplete && total > 0 && !routine.Notification.user.during.is_set)
                                        {
                                            notificationManager.ScheduleNotification(routineIdx + "Time for ", routine.title + ". Open the app to review your tasks." + routine.Notification.user.during.message, total);
                                            firebaseFunctionsService.GRUserNotificationSetToTrue(routine.id, routine.dbIdx.ToString(), "during");
                                        }
                                        Console.WriteLine("total : " + total);
                                        Console.WriteLine("during message: " + routine.Notification.user.during.message);
                                    }

                                    JToken userAfter = userNotification["after"];
                                    JToken userAfterMap = userAfter["mapValue"]["fields"];
                                    Console.WriteLine("userAfterMap" + userAfterMap);

                                    // is_set to make sure notification is not already stored on phone
                                    routine.Notification.user.after.is_set = (bool)userAfterMap["is_set"]["booleanValue"]
                                        && (userAfterMap["date_set"] != null) ? IsDateToday(userAfterMap["date_set"]["stringValue"].ToString()) : false;

                                    routine.Notification.user.during.is_enabled = (bool)userDuringMap["is_enabled"]["booleanValue"];

                                    if (routine.Notification.user.during.is_enabled && !routine.Notification.user.after.is_set)
                                    {
                                        routine.Notification.user.after.time = TimeSpan.Parse(userAfterMap["time"]["stringValue"].ToString());

                                        //TotalMinutes
                                        double total = routine.Notification.user.after.time.TotalSeconds +  (routine.availableEndTime - DateTime.Now.TimeOfDay).TotalSeconds;
                                        routine.Notification.user.after.message = userAfterMap["message"]["stringValue"].ToString();
                                        if (!routine.isComplete && total > 0 && !routine.Notification.user.after.is_set)
                                        {
                                            notificationManager.ScheduleNotification(routineIdx + "You Missed a Routine! ", routine.title + " is overdue. Open the app to review your tasks." + routine.Notification.user.after.message, total);
                                            firebaseFunctionsService.GRUserNotificationSetToTrue(routine.id, routine.dbIdx.ToString(), "after");
                                        }
                                        Console.WriteLine("total : " + total);
                                        Console.WriteLine("after message: " + routine.Notification.user.after.message);
                                    }

                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("NOTIFICATION ERROR");
                                    Console.WriteLine("{0} Exception caught.", e);
                                }

                                notificationManager.PrintPendingNotifications();


                                /*
                                Console.WriteLine("start time : " + startTime);
                                Console.WriteLine("end time : " + endTime);
                                if (startTime < 0 && startTime > total)
                                    notificationManager.ScheduleNotification("You Missed a Routine! ", routine.title + " is overdue. Open the app to review your tasks.", 1);
                                if (!routine.isComplete && endTime > 0 && endTime < 30 )
                                    notificationManager.ScheduleNotification("You Missed a Routine! ", routine.title + " is overdue. Open the app to review your tasks.", 1);
                                else if (!routine.isComplete &&  currentTime > routine.availableStartTime && currentTime < routine.availableEndTime)
                                    notificationManager.ScheduleNotification("Time for ", routine.title + ". Open the app to review your tasks.", 1);

                                */


                                /*JToken userNotification;
                                try
                                {
                                    userNotification = jsonGorR["fields"]["ta_notifications"]["arrayValue"]["values"];
                                    if (userNotification == null)
                                        return;
                                }
                                catch
                                {
                                    return;
                                }*/


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
                                         //Console.WriteLine("duration: " + duration);
                                         if (duration > 0)
                                         {
                                             Console.WriteLine("notification id: " + notificationManager.ScheduleNotification(title, message, duration));
                                         }
                                     }
                                 }*/

                                /*
                                Console.WriteLine("start time : " + startTime);
                                Console.WriteLine("end time : " + endTime);
                                if (startTime < 0 && startTime > total)
                                    notificationManager.ScheduleNotification("You Missed a Routine! ", routine.title + " is overdue. Open the app to review your tasks.", 1);
                                if (!routine.isComplete && endTime > 0 && endTime < 30 )
                                    notificationManager.ScheduleNotification("You Missed a Routine! ", routine.title + " is overdue. Open the app to review your tasks.", 1);
                                else if (!routine.isComplete &&  currentTime > routine.availableStartTime && currentTime < routine.availableEndTime)
                                    notificationManager.ScheduleNotification("Time for ", routine.title + ". Open the app to review your tasks.", 1);

                                */


                                /*JToken userNotification;
                                try
                                {
                                    userNotification = jsonGorR["fields"]["ta_notifications"]["arrayValue"]["values"];
                                    if (userNotification == null)
                                        return;
                                }
                                catch
                                {
                                    return;
                                }*/


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
                                         //Console.WriteLine("duration: " + duration);
                                         if (duration > 0)
                                         {
                                             Console.WriteLine("notification id: " + notificationManager.ScheduleNotification(title, message, duration));
                                         }
                                     }
                                 }*/

                                App.User.routines.Add(routine);
                                routineIdx++;

                                //Console.WriteLine("on Routine: " + routine.title + " " + routine.id);
                            }
                            else
                            {
                                goal goal = new goal
                                {
                                    title = jsonMapGorR["title"]["stringValue"].ToString(),

                                    id = jsonMapGorR["id"]["stringValue"].ToString(),

                                    photo = jsonMapGorR["photo"]["stringValue"].ToString(),

                                    isInProgress = isInProgressCheck
                                        && IsDateToday(jsonMapGorR["datetime_started"]["stringValue"].ToString()),

                                    isComplete = (bool)jsonMapGorR["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapGorR["datetime_completed"]["stringValue"].ToString())
                                        && !isInProgressCheck,

                                    dbIdx = dbIdx_,

                                    isSublistAvailable = (bool)jsonMapGorR["is_sublist_available"]["booleanValue"],

                                    expectedCompletionTime = TimeSpan.Parse(jsonMapGorR["expected_completion_time"]["stringValue"].ToString()),

                                    dateTimeCompleted = DateTime.Parse(jsonMapGorR["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),

                                    availableStartTime = TimeSpan.Parse(jsonMapGorR["available_start_time"]["stringValue"].ToString()),

                                    availableEndTime = TimeSpan.Parse(jsonMapGorR["available_end_time"]["stringValue"].ToString())
                                };

                                App.User.goals.Add(goal);
                                goalIdx++;

                                ////Console.WriteLine("on Goal: " + goal.id);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                    dbIdx_++;
                }

                App.User.routines.Sort((x, y) => TimeSpan.Compare(x.availableStartTime, y.availableStartTime));
                App.User.goals.Sort((x, y) => TimeSpan.Compare(x.availableStartTime, y.availableStartTime));

                routineIdx = 0;
                foreach (routine routine in App.User.routines)
                {
                    LoadTasks(routine.id, routineIdx, "routine");
                    routineIdx++;
                }

                goalIdx = 0;
                foreach (goal goal in App.User.goals)
                {
                    LoadTasks(goal.id, goalIdx, "goal");
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

                JToken jsonActionsAndTasks = null;
                try
                {
                    jsonActionsAndTasks = taskJson["fields"]["actions&tasks"]["arrayValue"]["values"];
                    if (jsonActionsAndTasks == null)
                    {
                        if (routineType == "routine")
                        {
                            App.User.routines[routineIdx].isSublistAvailable = false;
                        }
                        else
                        {
                            App.User.goals[routineIdx].isSublistAvailable = false;
                        }
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Exception caught.", e);
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
                            bool isInProgressCheck = (jsonMapAorT["is_in_progress"] == null) ? false : (bool)jsonMapAorT["is_in_progress"]["booleanValue"];

                            if (routineType == "routine")
                            {
                                task task = new task
                                {
                                    title = jsonMapAorT["title"]["stringValue"].ToString(),
                                    id = jsonMapAorT["id"]["stringValue"].ToString(),
                                    photo = jsonMapAorT["photo"]["stringValue"].ToString(),
                                    isInProgress = (bool)isInProgressCheck
                                        && IsDateToday(jsonMapAorT["datetime_started"]["stringValue"].ToString()),
                                    isComplete = (bool)jsonMapAorT["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapAorT["datetime_completed"]["stringValue"].ToString())
                                        && !isInProgressCheck,
                                    dbIdx = dbIdx_,
                                    isSublistAvailable = (bool)jsonMapAorT["is_sublist_available"]["booleanValue"],
                                    expectedCompletionTime = TimeSpan.Parse(jsonMapAorT["expected_completion_time"]["stringValue"].ToString()),
                                    dateTimeCompleted = DateTime.Parse(jsonMapAorT["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = TimeSpan.Parse(jsonMapAorT["available_start_time"]["stringValue"].ToString()),
                                    availableEndTime = TimeSpan.Parse(jsonMapAorT["available_end_time"]["stringValue"].ToString())
                                };

                                App.User.routines[routineIdx].tasks.Add(task);

                                //Console.WriteLine("on Task: " + task.id);

                            }
                            else if (routineType == "goal")
                            {
                                action action = new action
                                {
                                    title = jsonMapAorT["title"]["stringValue"].ToString(),
                                    id = jsonMapAorT["id"]["stringValue"].ToString(),
                                    photo = jsonMapAorT["photo"]["stringValue"].ToString(),
                                    isInProgress = (bool)jsonMapAorT["is_in_progress"]["booleanValue"]
                                        && IsDateToday(jsonMapAorT["datetime_started"]["stringValue"].ToString()),
                                    isComplete = (bool)jsonMapAorT["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapAorT["datetime_completed"]["stringValue"].ToString())
                                        && !isInProgressCheck,
                                    dbIdx = dbIdx_,
                                    isSublistAvailable = (bool)jsonMapAorT["is_sublist_available"]["booleanValue"],
                                    expectedCompletionTime = TimeSpan.Parse(jsonMapAorT["expected_completion_time"]["stringValue"].ToString()),
                                    dateTimeCompleted = DateTime.Parse(jsonMapAorT["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = TimeSpan.Parse(jsonMapAorT["available_start_time"]["stringValue"].ToString()),
                                    availableEndTime = TimeSpan.Parse(jsonMapAorT["available_end_time"]["stringValue"].ToString())
                                };

                                App.User.goals[routineIdx].actions.Add(action);

                                //Console.WriteLine("on Action: " + action.id);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0}", e);
                        Console.WriteLine("{0}", jsonAorT);
                    }
                    dbIdx_++;
                }

                if (routineType == "routine")
                {
                    int taskIdx = 0;
                    if (App.User.routines[routineIdx].tasks.Count == 0)
                        App.User.routines[routineIdx].isSublistAvailable = false;
                    foreach (task task in App.User.routines[routineIdx].tasks)
                    {
                        ////Console.WriteLine("on Task step load: " + task.id);
                        _ = LoadSteps(routineID, task.id, routineIdx, taskIdx, routineType);
                        taskIdx++;
                    }
                }
                else
                {
                    int actionIdx = 0;
                    if (App.User.goals[routineIdx].actions.Count == 0)
                        App.User.goals[routineIdx].isSublistAvailable = false;
                    foreach (action action in App.User.goals[routineIdx].actions)
                    {
                        ////Console.WriteLine("on action step load: " + action.id);
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
                    {
                        if (routineType == "routine")
                        {
                            App.User.routines[routineIdx].tasks[taskIdx].isSublistAvailable = false;
                        }
                        else
                        {
                            App.User.goals[routineIdx].actions[taskIdx].isSublistAvailable = false;
                        }
                        return;
                    }
                }
                catch
                {
                    //Console.WriteLine("Error with json action/task token:");
                    //Console.WriteLine(stepJson);
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
                            bool isInProgressCheck = (jsonMapIorS["is_in_progress"] == null) ? false : (bool)jsonMapIorS["is_in_progress"]["booleanValue"];

                            if (routineType == "routine")
                            {
                                DateTime duration = DateTime.Parse(jsonMapIorS["expected_completion_time"]["stringValue"].ToString());

                                step step = new step
                                {
                                    title = jsonMapIorS["title"]["stringValue"].ToString(),
                                    photo = jsonMapIorS["photo"]["stringValue"].ToString(),
                                    isInProgress = isInProgressCheck
                                        && IsDateToday(jsonMapIorS["datetime_started"]["stringValue"].ToString()),
                                    isComplete = (bool)jsonMapIorS["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapIorS["datetime_completed"]["stringValue"].ToString())
                                        && !isInProgressCheck,
                                    dbIdx = dbIdx_,
                                    expectedCompletionTime = TimeSpan.Parse(jsonMapIorS["expected_completion_time"]["stringValue"].ToString()),
                                    dateTimeCompleted = DateTime.Parse(jsonMapIorS["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = TimeSpan.Parse(jsonMapIorS["available_start_time"]["stringValue"].ToString()),
                                    availableEndTime = TimeSpan.Parse(jsonMapIorS["available_end_time"]["stringValue"].ToString())
                                };

                                ////Console.WriteLine("on Step: " + step.isComplete);

                                App.User.routines[routineIdx].tasks[taskIdx].steps.Add(step);
                            }
                            else if (routineType == "goal")
                            {
                                instruction instruction = new instruction
                                {
                                    title = jsonMapIorS["title"]["stringValue"].ToString(),
                                    photo = jsonMapIorS["photo"]["stringValue"].ToString(),
                                    isInProgress = isInProgressCheck
                                        && IsDateToday(jsonMapIorS["datetime_started"]["stringValue"].ToString()),
                                    isComplete = (bool)jsonMapIorS["is_complete"]["booleanValue"]
                                        && IsDateToday(jsonMapIorS["datetime_completed"]["stringValue"].ToString())
                                        && !isInProgressCheck,

                                    dbIdx = dbIdx_,
                                    expectedCompletionTime = TimeSpan.Parse(jsonMapIorS["expected_completion_time"]["stringValue"].ToString()),
                                    dateTimeCompleted = DateTime.Parse(jsonMapIorS["datetime_completed"]["stringValue"].ToString()).ToLocalTime(),
                                    availableStartTime = TimeSpan.Parse(jsonMapIorS["available_start_time"]["stringValue"].ToString()),
                                    availableEndTime = TimeSpan.Parse(jsonMapIorS["available_end_time"]["stringValue"].ToString())
                                };



                                App.User.goals[routineIdx].actions[taskIdx].instructions.Add(instruction);

                                ////Console.WriteLine("on Instruction: " + instruction.isComplete);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                    dbIdx_++;
                }
                if (routineType == "routine")
                {
                    if (App.User.routines[routineIdx].tasks[taskIdx].steps.Count == 0)
                        App.User.routines[routineIdx].tasks[taskIdx].isSublistAvailable = false;
                }
                else
                {
                    if (App.User.goals[routineIdx].actions[taskIdx].instructions.Count == 0)
                        App.User.goals[routineIdx].actions[taskIdx].isSublistAvailable = false;
                }
            }
        }

        private bool IsDateToday(string dateTimeString)
        {
            ////Console.WriteLine("checkDatestring: " + dateTimeString);

            DateTime today = DateTime.Now;
            DateTime checkDate = DateTime.Parse(dateTimeString);

            ////Console.WriteLine("checkDate: " + checkDate.ToString());
            ////Console.WriteLine("checkDate result: " + (today.Date == checkDate.Date).ToString());

            return (today.Date == checkDate.Date) ? true : false;
        }

        private string formatTimeSpanString(string inputString)
        {
            string result = "";

            string[] componants = inputString.Split(':');

            foreach (var componant in componants)
                result += componant.PadLeft(2, '0');

            return result;
        }

        void PrintFirebaseUser()
        {
            Console.WriteLine("user first name: " + App.User.firstName);
            Console.WriteLine("user last name: " + App.User.lastName);

            foreach (routine routine in App.User.routines)
            {
                Console.WriteLine("user routine title: " + routine.title);
                Console.WriteLine("user routine id: " + routine.id);
                foreach (task task in routine.tasks)
                {
                    Console.WriteLine("user task title: " + task.title);
                    Console.WriteLine("user task id: " + task.id);
                    foreach (step step in task.steps)
                    {
                        Console.WriteLine("user step title: " + step.title);
                    }
                }
            }

            foreach (goal goal in App.User.goals)
            {
                Console.WriteLine("user goal title: " + goal.title);
                Console.WriteLine("user goal id: " + goal.id);
                foreach (action action in goal.actions)
                {
                    Console.WriteLine("user action title: " + goal.title);
                    Console.WriteLine("user action id: " + goal.id);
                    foreach (instruction instruction in action.instructions)
                    {
                        Console.WriteLine("user instruction title: " + instruction.title);
                    }
                }
            }
        }
    }
}
