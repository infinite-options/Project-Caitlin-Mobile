using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Plugin.CloudFirestore;
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

            var userDoc = await CrossCloudFirestore.Current
                                        .Instance
                                        .GetCollection("users")
                                        .GetDocument(uid)
                                        .GetDocumentAsync();

            CrossCloudFirestore.Current.Instance.GetCollection("users")
                           .GetDocument(uid)
                           .AddSnapshotListener((snapshot, error) =>
                           {
                               App.User = snapshot.ToObject<user>();
                               App.User.routines = new List<routine>();

                               var docData = snapshot.Data;

                               App.User.firstName = docData["first_name"].ToString();
                               App.User.lastName = docData["last_name"].ToString();

                               var grArrayData = (List<Object>) docData["goals&routines"];
                               var aboutMeData = (Dictionary<string, object>) docData["about_me"];

                               LoadAboutMe(aboutMeData);

                               LoadGoalsAndRoutines(grArrayData);
                           });

            CrossCloudFirestore.Current.Instance.GetCollection("users")
                           .GetDocument(uid)
                           .GetCollection("people")
                           .AddSnapshotListener((snapshot, error) =>
                           {
                               if (snapshot != null)
                               {
                                   foreach (var document in snapshot.Documents)
                                   {
                                       LoadPeople(document.Data);
                                   }
                               }
                           });

        }

        

        private void LoadAboutMe(Dictionary<string, object> aboutMeData)
        {
            App.User.aboutMe.message_day = aboutMeData["message_day"].ToString();
            App.User.aboutMe.message_card = aboutMeData["message_card"].ToString();
            App.User.aboutMe.pic = aboutMeData["pic"].ToString();
        }

        private void LoadPeople(IDictionary<string, object> data)
        {
            var person = new person()
            {
                name = data["name"].ToString(),
                relationship = data["relationship"].ToString(),
                phoneNumber = data["phone_number"].ToString(),
                pic = data["pic"].ToString(),
                speakerId = data["speaker_id"].ToString(),
            };

            App.User.people.Add(person);
        }

        public void LoadGoalsAndRoutines(List<Object> grArrayData)
        {
            int dbIdx_ = 0;
            foreach (Dictionary<string, object> data in grArrayData)
            {
                try
                {
                    if (data["is_available"] == "1")
                    {
                        bool isInProgressCheck = data.ContainsKey("is_in_progress") ? data["is_in_progress"].ToString() == "1" : false;

                        if (data["is_persistent"] == "1")
                        {
                            routine routine = new routine
                            {
                                title = data["title"].ToString(),

                                id = data["id"].ToString(),

                                photo = data["photo"].ToString(),

                                isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                                isComplete = data["is_complete"].ToString() == "1"
                                                && IsDateToday(data["datetime_completed"].ToString())
                                                && !isInProgressCheck,

                                expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                                dbIdx = dbIdx_,

                                isSublistAvailable = data["is_sublist_available"].ToString() == "1",

                                dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                                availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                                availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                            };

                            App.User.routines.Add(routine);
                        }
                        else
                        {
                            goal goal = new goal
                            {
                                title = data["title"].ToString(),

                                id = data["id"].ToString(),

                                photo = data["photo"].ToString(),

                                isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                                isComplete = data["is_complete"].ToString() == "1"
                                                && IsDateToday(data["datetime_completed"].ToString())
                                                && !isInProgressCheck,

                                expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                                dbIdx = dbIdx_,

                                isSublistAvailable = data["is_sublist_available"].ToString() == "1",

                                dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                                availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                                availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                            };

                            App.User.goals.Add(goal);
                        }
                    }

                    dbIdx_++;

                    App.User.routines.Sort((x, y) => TimeSpan.Compare(x.availableStartTime, y.availableStartTime));
                    App.User.goals.Sort((x, y) => TimeSpan.Compare(x.availableStartTime, y.availableStartTime));

                    int grIdx = 0;
                    foreach (routine routine in App.User.routines)
                    {
                        CreateActionsAndTasksSnapshot(routine.id, grIdx, "routine");
                        grIdx++;
                    }

                    int goalIdx = 0;
                    foreach (goal goal in App.User.goals)
                    {
                        CreateActionsAndTasksSnapshot(goal.id, goalIdx, "goal");
                        goalIdx++;
                    }
                }
                catch
                {
                    Console.WriteLine($"Error with Routine or Goal: {data}");
                }
            }
        }

        private void CreateActionsAndTasksSnapshot(string grId, int grIdx, string grType)
        {
            CrossCloudFirestore.Current.Instance.GetCollection("users")
                           .GetDocument(uid)
                           .GetCollection("goals&routines")
                           .GetDocument(grId)
                           .AddSnapshotListener((snapshot, error) =>
                           {
                               var docData = snapshot.Data;

                               var atArrayData = (List<Object>)docData["actions&tasks"];

                               LoadActionsAndTasks(atArrayData, grIdx, grType);
                           });
        }

        private void LoadActionsAndTasks(List<Object> atArrayData, int grIdx, string grType)
        {
            int dbIdx_ = 0;
            foreach (Dictionary<string, object> data in atArrayData)
            {
                if (data["is_available"].ToString() == "1")
                {
                    bool isInProgressCheck = data.ContainsKey("is_in_progress") ? data["is_in_progress"].ToString() == "1" : false;

                    if (grType == "routine")
                    {
                        task task = new task
                        {
                            title = data["title"].ToString(),

                            id = data["id"].ToString(),

                            photo = data["photo"].ToString(),

                            isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                            isComplete = data["is_complete"].ToString() == "1"
                                                    && IsDateToday(data["datetime_completed"].ToString())
                                                    && !isInProgressCheck,

                            expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                            dbIdx = dbIdx_,

                            isSublistAvailable = data["is_sublist_available"].ToString() == "1",

                            dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                            availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                            availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                        };
                    }
                    else
                    {
                        action action = new action
                        {
                            title = data["title"].ToString(),

                            id = data["id"].ToString(),

                            photo = data["photo"].ToString(),

                            isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                            isComplete = data["is_complete"].ToString() == "1"
                                                    && IsDateToday(data["datetime_completed"].ToString())
                                                    && !isInProgressCheck,

                            expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                            dbIdx = dbIdx_,

                            isSublistAvailable = data["is_sublist_available"].ToString() == "1",

                            dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                            availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                            availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                        };
                    }
                }

                dbIdx_++;

                if (grType == "routine")
                {
                    int taskIdx = 0;
                    if (App.User.routines[grIdx].tasks.Count == 0)
                        App.User.routines[grIdx].isSublistAvailable = false;
                    foreach (task task in App.User.routines[grIdx].tasks)
                    {
                        var routineId = App.User.routines[grIdx].id;
                        CreateStepsAndInstrSnapshot(routineId, task.id, grIdx, taskIdx, grType);
                        taskIdx++;
                    }
                }
                else
                {
                    int actionIdx = 0;
                    if (App.User.goals[grIdx].actions.Count == 0)
                        App.User.goals[grIdx].isSublistAvailable = false;
                    foreach (action action in App.User.goals[grIdx].actions)
                    {
                        var goalId = App.User.routines[grIdx].id;
                        CreateStepsAndInstrSnapshot(goalId, action.id, grIdx, actionIdx, grType);
                        actionIdx++;
                    }
                }
            }
        }

        private void CreateStepsAndInstrSnapshot(string grId, string atId, int grIdx, int atIdx, string grType)
        {
            CrossCloudFirestore.Current.Instance.GetCollection("users")
                           .GetDocument(uid)
                           .GetCollection("goals&routines")
                           .GetDocument(grId)
                           .GetCollection("actions&tasks")
                           .GetDocument(atId)
                           .AddSnapshotListener((snapshot, error) =>
                           {
                               var docData = snapshot.Data;

                               var isArrayData = (List<Object>)docData["instructions&steps"];

                               LoadInstructionsAndSteps(isArrayData, grIdx, atIdx, grType);
                           });
        }

        private void LoadInstructionsAndSteps(List<object> isArrayData, int grIdx, int atIdx, string grType)
        {
            int dbIdx_ = 0;
            foreach (Dictionary<string, object> data in isArrayData)
            {
                if (data["is_available"].ToString() == "1")
                {
                    bool isInProgressCheck = data.ContainsKey("is_in_progress") ? data["is_in_progress"].ToString() == "1" : false;

                    if (grType == "routine")
                    {
                        task task = new task
                        {
                            title = data["title"].ToString(),

                            id = data["id"].ToString(),

                            photo = data["photo"].ToString(),

                            isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                            isComplete = data["is_complete"].ToString() == "1"
                                                    && IsDateToday(data["datetime_completed"].ToString())
                                                    && !isInProgressCheck,

                            expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                            dbIdx = dbIdx_,

                            isSublistAvailable = data["is_sublist_available"].ToString() == "1",

                            dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                            availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                            availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                        };
                    }
                    else
                    {
                        action action = new action
                        {
                            title = data["title"].ToString(),

                            id = data["id"].ToString(),

                            photo = data["photo"].ToString(),

                            isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                            isComplete = data["is_complete"].ToString() == "1"
                                                    && IsDateToday(data["datetime_completed"].ToString())
                                                    && !isInProgressCheck,

                            expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                            dbIdx = dbIdx_,

                            isSublistAvailable = data["is_sublist_available"].ToString() == "1",

                            dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                            availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                            availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                        };
                    }
                }

                dbIdx_++;
            }

            if (grType == "routine")
            {
                if (App.User.routines[grIdx].tasks[atIdx].steps.Count == 0)
                    App.User.routines[grIdx].tasks[atIdx].isSublistAvailable = false;
            }
            else
            {
                if (App.User.goals[grIdx].actions[atIdx].instructions.Count == 0)
                    App.User.goals[grIdx].actions[atIdx].isSublistAvailable = false;
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


        //                        //time precised in minutes, can be positive or negative.
        //                        int startTime = (int)(currentTime - routine.availableStartTime).TotalMinutes;
        //                        int endTime = (int)(currentTime - routine.availableEndTime).TotalMinutes;

        //                        JToken userNotification;
        //                        try
        //                        {
        //                            //Console.WriteLine("jsonMapGorR" + jsonMapGorR["user_notifications"]["mapValue"]["fields"]);
        //                            userNotification = jsonMapGorR["user_notifications"]["mapValue"]["fields"];
        //                            if (userNotification == null)
        //                                return;
        //                            routine.Notification = new Notification();
        //                            routine.Notification.user = new NotificationTime();

        //                            JToken userBefore = userNotification["before"];
        //                            JToken userBeforeMap = userBefore["mapValue"]["fields"];

        //                            routine.Notification.user.before.is_set = (bool)userBeforeMap["is_set"]["booleanValue"]
        //                                && ((userBeforeMap["date_set"] != null) ? IsDateToday(userBeforeMap["date_set"]["stringValue"].ToString()) : false);

        //                            routine.Notification.user.before.is_enabled = (bool)userBeforeMap["is_enabled"]["booleanValue"];

        //                            if (routine.Notification.user.before.is_enabled && !routine.Notification.user.before.is_set)
        //                            {

        //                                routine.Notification.user.before.time = TimeSpan.Parse(userBeforeMap["time"]["stringValue"].ToString());
        //                                //TotalMinutes

        //                                double total = (routine.availableStartTime - DateTime.Now.TimeOfDay).TotalSeconds - routine.Notification.user.before.time.TotalSeconds;

        //                                routine.Notification.user.before.message = userBeforeMap["message"]["stringValue"].ToString();

        //                                if (!routine.isComplete && total > 0 && !routine.Notification.user.before.is_set)
        //                                {
        //                                    string title = "Ready for " + routine.title + "?";
        //                                    string subtitle = grIdx + routine.id;
        //                                    string message = "Open the app to review your tasks. " + routine.Notification.user.before.message;
        //                                    notificationManager.ScheduleNotification(title, subtitle, message, total);
        //                                    firebaseFunctionsService.GRUserNotificationSetToTrue(routine.id, routine.dbIdx.ToString(), "before");

        //                                }
        //                                Console.WriteLine("total : " + total);
        //                                Console.WriteLine("before message: " + routine.Notification.user.before.message);
        //                            }

        //                            JToken userDuring = userNotification["during"];
        //                            JToken userDuringMap = userDuring["mapValue"]["fields"];

        //                            routine.Notification.user.during.is_set = (bool)userDuringMap["is_set"]["booleanValue"]
        //                                && (userDuringMap["date_set"] != null) ? IsDateToday(userDuringMap["date_set"]["stringValue"].ToString()) : false;

        //                            routine.Notification.user.during.is_enabled = (bool)userDuringMap["is_enabled"]["booleanValue"];

        //                            if (routine.Notification.user.during.is_enabled && !routine.Notification.user.during.is_set)
        //                            {
        //                                routine.Notification.user.during.time = TimeSpan.Parse(userDuringMap["time"]["stringValue"].ToString());
        //                                //TotalMinutes
        //                                double total = routine.Notification.user.during.time.TotalSeconds + (routine.availableStartTime - DateTime.Now.TimeOfDay).TotalSeconds;
        //                                routine.Notification.user.during.message = userDuringMap["message"]["stringValue"].ToString();

        //                                if (!routine.isComplete && total > 0 && !routine.Notification.user.during.is_set)
        //                                {
        //                                    string title = "Time for " + routine.title;
        //                                    string subtitle = grIdx + routine.id;
        //                                    string message = "Open the app to review your tasks. " + routine.Notification.user.during.message;
        //                                    notificationManager.ScheduleNotification(title, subtitle, message, total);
        //                                    firebaseFunctionsService.GRUserNotificationSetToTrue(routine.id, routine.dbIdx.ToString(), "during");
        //                                }
        //                                Console.WriteLine("total : " + total);
        //                                Console.WriteLine("during message: " + routine.Notification.user.during.message);
        //                            }

        //                            JToken userAfter = userNotification["after"];
        //                            JToken userAfterMap = userAfter["mapValue"]["fields"];
        //                            Console.WriteLine("userAfterMap" + userAfterMap);

        //                            // is_set to make sure notification is not already stored on phone
        //                            routine.Notification.user.after.is_set = (bool)userAfterMap["is_set"]["booleanValue"]
        //                                && (userAfterMap["date_set"] != null) ? IsDateToday(userAfterMap["date_set"]["stringValue"].ToString()) : false;

        //                            routine.Notification.user.after.is_enabled = (bool)userAfterMap["is_enabled"]["booleanValue"];

        //                            if (routine.Notification.user.after.is_enabled && !routine.Notification.user.after.is_set)
        //                            {
        //                                routine.Notification.user.after.time = TimeSpan.Parse(userAfterMap["time"]["stringValue"].ToString());

        //                                //TotalMinutes
        //                                double total = routine.Notification.user.after.time.TotalSeconds +  (routine.availableEndTime - DateTime.Now.TimeOfDay).TotalSeconds;
        //                                routine.Notification.user.after.message = userAfterMap["message"]["stringValue"].ToString();
        //                                if (!routine.isComplete && total > 0 && !routine.Notification.user.after.is_set)
        //                                {
        //                                    string title = "You missed " + routine.title;
        //                                    string subtitle = grIdx + routine.id;
        //                                    string message = "Open the app to review your tasks. " + routine.Notification.user.after.message;
        //                                    notificationManager.ScheduleNotification(title, subtitle, message, total);
        //                                    firebaseFunctionsService.GRUserNotificationSetToTrue(routine.id, routine.dbIdx.ToString(), "after");
        //                                }
        //                                Console.WriteLine("total : " + total);
        //                                Console.WriteLine("after message: " + routine.Notification.user.after.message);
        //                            }

        //                        }
        //                        catch (Exception e)
        //                        {
        //                            Console.WriteLine("NOTIFICATION ERROR");
        //                            Console.WriteLine("{0} Exception caught.", e);
        //                        }

        //                        notificationManager.PrintPendingNotifications();

        //                        App.User.routines.Add(routine);
        //                        grIdx++;

        //                        //Console.WriteLine("on Routine: " + routine.title + " " + routine.id);

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
