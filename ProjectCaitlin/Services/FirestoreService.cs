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
using ProjectCaitlin.Services;
using Newtonsoft.Json;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

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

        public async Task LoadFirebasePhoto()
        {
            App.User.photoURIs = new List<List<String>>();
            //load people from firebase
            var photosCollection = await CrossCloudFirestore.Current.Instance.GetCollection("users")
                            .GetDocument(uid)
                            .GetCollection("people")
                            .GetDocumentsAsync();

            List<photo> photos = new List<photo>();
            foreach (var photoDocument in photosCollection.Documents)
            {
                try
                {
                    var data = photoDocument.Data;
                    photo photo = new photo();
                    photo.id = data["photo_id"].ToString();
                    photo.description = data["description"].ToString();
                    photo.note = data["notes"].ToString();

                    App.User.FirebasePhotos.Add(photo);
                }
                catch
                {

                }
            }
        }


        public async Task SetupFirestoreSnapshot()
        {

                CrossCloudFirestore.Current.Instance
                    .GetCollection("users")
                    .GetDocument(uid)
                    .AddSnapshotListener(async (snapshot, error) =>
                    {
                        if (!App.isFirstSetup)
                        {
                            await LoadDatabase();
                        }
                        App.isFirstSetup = false;
                    });
        }

        public async Task LoadDatabase()
        {
            LoadFirebasePhoto();
            LoadPeople();
            await LoadUser();
        }

        public async Task LoadUser()
        {
            // reset current user and goals values (in case of reload)
            App.User.people = new List<person>();
            App.User.routines = new List<routine>();
            App.User.goals = new List<goal>();
            App.User.allDates = new HashSet<string>();

            Console.WriteLine("uid is: " + uid);

            var userDocument = await CrossCloudFirestore.Current.Instance
                .GetCollection("users")
                .GetDocument(uid)
                .GetDocumentAsync();


            Console.WriteLine("Printing Data");
            Console.WriteLine(userDocument);
            

            var docData = userDocument.Data;

            //Console.WriteLine(userDocument.Data["is_displayed_today"]);


            if (docData.ContainsKey("first_name") && docData.ContainsKey("last_name"))
            {
                App.User.firstName = docData["first_name"].ToString();
                Console.WriteLine(docData["first_name"].ToString());
                App.User.lastName = docData["last_name"].ToString();
            }

            if (docData.ContainsKey("email_id"))
            {
                App.User.email = docData["email_id"].ToString();
            }

            if (docData.ContainsKey("goals&routines"))
            {
                var grArrayData = (List<object>)docData["goals&routines"];
                LoadGoalsAndRoutines(grArrayData);
            }

            if (docData.ContainsKey("about_me"))
            {
                var aboutMeData = (Dictionary<string, object>)docData["about_me"];
                LoadAboutMe(aboutMeData);
            }
        }



        private void LoadAboutMe(Dictionary<string, object> aboutMeData)
        {
            App.User.aboutMe.message_day = aboutMeData["message_day"].ToString();
            App.User.aboutMe.message_card = aboutMeData["message_card"].ToString();
            App.User.aboutMe.pic = aboutMeData["pic"].ToString();
        }

        public async Task LoadPeople()
        {
            //load people from firebase
            var peopleCollection = await CrossCloudFirestore.Current.Instance.GetCollection("users")
                                    .GetDocument(uid)
                                    .GetCollection("people")
                                    .GetDocumentsAsync();

            if (peopleCollection != null)
            {
                foreach (var document in peopleCollection.Documents)
                {
                    var data = document.Data;
                    if (data["important"].ToString() == "1")
                    {
                        var person = new person()
                        {
                            name = data["name"].ToString(),
                            relationship = data.ContainsKey("relationship") ? data["relationship"].ToString() : "",
                            phoneNumber = data["phone_number"].ToString(),
                            pic = data.ContainsKey("pic") ? data["pic"].ToString() : "",
                            //speakerId = data["speaker_id"].ToString(),
                        };
                        App.User.people.Add(person);
                    }
                }
            }
        }

        public void LoadGoalsAndRoutines(List<Object> grArrayData)
        {

            int dbIdx_ = 0, routineIdx = 0;
            foreach (IDictionary<string, object> data in grArrayData)
            {
                try
                {
                    bool isDisplayedToday = true;
                    if (data.ContainsKey("is_displayed_today"))
                        Console.WriteLine(data["is_displayed_today"]);
                        //isDisplayedToday = (bool) data["is_displayed_today"];
                        isDisplayedToday = convertBinToBool(data["is_displayed_today"].ToString()); 
                    if (isDisplayedToday)
                    {
                        if (convertBinToBool(data["is_available"].ToString()))
                        {
                            bool isInProgressCheck = data.ContainsKey("is_in_progress") ? convertBinToBool(data["is_in_progress"].ToString()) == true : false;

                            grObject grObject = new grObject
                            {
                                title = data["title"].ToString(),

                                id = data["id"].ToString(),

                                photo = data["photo"].ToString(),

                                isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                                isComplete = convertBinToBool(data["is_complete"].ToString())
                                                    && IsDateToday(data["datetime_completed"].ToString())
                                                    && !isInProgressCheck,

                                expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                                dbIdx = dbIdx_,

                                isSublistAvailable = convertBinToBool(data["is_sublist_available"].ToString()),

                                dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                                //availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                                availableStartTime = DateTime.Parse(data["end_day_and_time"].ToString()).TimeOfDay,

                                //availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                                availableEndTime = DateTime.Parse(data["end_day_and_time"].ToString()).TimeOfDay

                            };

                            var serializedParent = JsonConvert.SerializeObject(grObject);


                            if (convertBinToBool(data["is_persistent"].ToString()))
                            {
                                routine routine = JsonConvert.DeserializeObject<routine>(serializedParent);

                                App.User.routines.Add(routine);

                                setNotifications(routine, routineIdx, (IDictionary<string, object>)data["user_notifications"]);

                                routineIdx++;
                            }
                            else
                            {
                                goal goal = JsonConvert.DeserializeObject<goal>(serializedParent);

                                App.User.goals.Add(goal);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error with Routine or Goal: {data.Keys}");
                    Console.WriteLine($"ERROR: {e}");
                }

                dbIdx_++;
            }

            App.User.routines.Sort((x, y) => TimeSpan.Compare(x.availableStartTime, y.availableStartTime));
            App.User.goals.Sort((x, y) => TimeSpan.Compare(x.availableStartTime, y.availableStartTime));

            int grIdx = 0;
            foreach (routine routine in App.User.routines)
            {
                CreateActionsAndTasksSnapshot(grIdx, routine, "routine");
                grIdx++;
            }

            grIdx = 0;
            foreach (goal goal in App.User.goals)
            {
                CreateActionsAndTasksSnapshot(grIdx, goal, "goal");
                grIdx++;
            }
        }

        private async Task CreateActionsAndTasksSnapshot(int grIdx, grObject grObject, string grType)
        {
            Console.WriteLine($"Loading Goal/Routine: {grObject.title}");
            var document = await CrossCloudFirestore.Current.Instance.GetCollection("users")
                           .GetDocument(uid)
                           .GetCollection("goals&routines")
                           .GetDocument(grObject.id)
                           .GetDocumentAsync();

            if (document.Data != null)
            {
                var docData = document.Data;
                if (docData.ContainsKey("actions&tasks"))
                {
                    var atArrayData = (List<object>)docData["actions&tasks"];

                    LoadActionsAndTasks(atArrayData, grIdx, grObject, grType);
                }
                else
                {
                    if (grType == "routine")
                        App.User.routines[grIdx].isSublistAvailable = false;
                    else
                        App.User.goals[grIdx].isSublistAvailable = false;
                }
            }
        }

        private void LoadActionsAndTasks(List<object> atArrayData, int grIdx, grObject grObject, string grType)
        {
            int dbIdx_ = 0;
            foreach (Dictionary<string, object> data in atArrayData)
            {
                try
                {
                    if (data["is_available"].ToString() == "1")
                    {
                        bool isInProgressCheck = data.ContainsKey("is_in_progress") ? data["is_in_progress"].ToString() == "1" : false;

                        atObject atObject = new atObject
                        {
                            title = data["title"].ToString(),

                            id = data["id"].ToString(),

                            grId = grObject.id,

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

                            //availableStartTime = TimeSpan.Parse(DateTime.Parse(data["start_day_and_time"].ToString()).ToString()),

                            availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                            //availableEndTime = TimeSpan.Parse(DateTime.Parse(data["end_day_and_time"].ToString()).ToString())
                        };

                        var serializedParent = JsonConvert.SerializeObject(atObject);

                        if (grType == "routine")
                        {
                            task task = JsonConvert.DeserializeObject<task>(serializedParent);

                            App.User.routines[grIdx].tasks.Add(task);
                        }
                        else
                        {
                            action action = JsonConvert.DeserializeObject<action>(serializedParent);

                            App.User.goals[grIdx].actions.Add(action);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error with Action or Task: {data.Keys}");
                    Console.WriteLine($"ERROR: {e}");
                }
                dbIdx_++;
            }

            if (grType == "routine")
            {
                int taskIdx = 0;
                if (App.User.routines[grIdx].tasks.Count == 0)
                    App.User.routines[grIdx].isSublistAvailable = false;
                foreach (task task in App.User.routines[grIdx].tasks)
                {
                    var routineId = App.User.routines[grIdx].id;
                    CreateStepsAndInstrSnapshot(grObject, task, grIdx, taskIdx, grType);
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
                    var goalId = App.User.goals[grIdx].id;
                    CreateStepsAndInstrSnapshot(grObject, action, grIdx, actionIdx, grType);
                    actionIdx++;
                }
            }
        }

        private async Task CreateStepsAndInstrSnapshot(grObject grObject, atObject atObject, int grIdx, int atIdx, string grType)
        {
            Console.WriteLine($"Loading action/task: {atObject.title}");
            var document = await CrossCloudFirestore.Current.Instance.GetCollection("users")
                           .GetDocument(uid)
                           .GetCollection("goals&routines")
                           .GetDocument(grObject.id)
                           .GetCollection("actions&tasks")
                           .GetDocument(atObject.id)
                           .GetDocumentAsync();

            if (document.Data != null)
            {
                var docData = document.Data;
                if (docData.ContainsKey("instructions&steps"))
                {
                    var isArrayData = (List<object>)docData["instructions&steps"];

                    LoadInstructionsAndSteps(isArrayData, grObject, atObject, grIdx, atIdx, grType);
                }
                else
                {
                    if (grType == "routine")
                        App.User.routines[grIdx].tasks[atIdx].isSublistAvailable = false;
                    else
                        App.User.goals[grIdx].actions[atIdx].isSublistAvailable = false;

                }
            }
        }

        private void LoadInstructionsAndSteps(List<object> isArrayData, grObject grObject, atObject atObject, int grIdx, int atIdx, string grType)
        {
            int dbIdx_ = 0;
            foreach (Dictionary<string, object> data in isArrayData)
            {
                try
                {
                    if (data["is_available"].ToString() == "1")
                    {
                        bool isInProgressCheck = data.ContainsKey("is_in_progress") ? data["is_in_progress"].ToString() == "1" : false;

                        isObject isObject = new isObject
                        {
                            grId = grObject.id,

                            atId = atObject.id,

                            title = data["title"].ToString(),

                            photo = data["photo"].ToString(),

                            isInProgress = isInProgressCheck && IsDateToday(data["datetime_started"].ToString()),

                            isComplete = data["is_complete"].ToString() == "1"
                                                        && IsDateToday(data["datetime_completed"].ToString())
                                                        && !isInProgressCheck,

                            expectedCompletionTime = TimeSpan.Parse(data["expected_completion_time"].ToString()),

                            dbIdx = dbIdx_,

                            dateTimeCompleted = DateTime.Parse(data["datetime_completed"].ToString()).ToLocalTime(),

                            //availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                            availableStartTime = TimeSpan.Parse(data["available_start_time"].ToString()),

                            //availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                            availableEndTime = TimeSpan.Parse(data["available_end_time"].ToString())
                        };

                        var serializedParent = JsonConvert.SerializeObject(isObject);

                        if (grType == "routine")
                        {
                            step step = JsonConvert.DeserializeObject<step>(serializedParent);

                            App.User.routines[grIdx].tasks[atIdx].steps.Add(step);
                        }
                        else
                        {
                            instruction instruction = JsonConvert.DeserializeObject<instruction>(serializedParent);

                            App.User.goals[grIdx].actions[atIdx].instructions.Add(instruction);
                        }
                    }
                }
                catch
                {

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

            DateTime today = DateTime.Now.AddHours(-5);
            DateTime checkDate = DateTime.Parse(dateTimeString);

            ////Console.WriteLine("checkDate: " + checkDate.ToString());
            ////Console.WriteLine("checkDate result: " + (today.Date == checkDate.Date).ToString());

            return (today.Date == checkDate.Date) ? true : false;
        }


        public async Task setNotifications(routine routine, int grIdx, IDictionary<string, object> notificationDict)
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            //time precised in minutes, can be positive or negative.
            int startTime = (int)(currentTime - routine.availableStartTime).TotalMinutes;
            int endTime = (int)(currentTime - routine.availableEndTime).TotalMinutes;

            List<string> titles
                = new List<string>()
                {
                    "Ready for " + routine.title + "?",
                    "Time for " + routine.title,
                    "You missed " + routine.title
                };

            List<string> notiTimeKeysList
                = new List<string>()
                {
                    "before",
                    "during",
                    "after"
                };

            List<NotificationAttributes> notiAttriObjList
                = new List<NotificationAttributes>()
                {
                    routine.Notification.user.before,
                    routine.Notification.user.during,
                    routine.Notification.user.after
                };

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (notificationDict == null)
                        return;

                    IDictionary<string, object> userTimeDict = (IDictionary<string, object>)notificationDict[notiTimeKeysList[i]];

                    notiAttriObjList[i].is_set = userTimeDict["is_set"].ToString() == "1"
                                                    && ((userTimeDict["date_set"] != null) ? IsDateToday(userTimeDict["date_set"].ToString()) : false);

                    notiAttriObjList[i].is_enabled = userTimeDict["is_enabled"].ToString() == "1";

                    if (notiAttriObjList[i].is_enabled && !notiAttriObjList[i].is_set)
                    {
                        notiAttriObjList[i].time = TimeSpan.Parse(userTimeDict["time"].ToString());

                        //TotalMinutes
                        double total = 0;
                        switch (i)
                        {
                            case 0:
                                total = (routine.availableStartTime - DateTime.Now.TimeOfDay).TotalSeconds - notiAttriObjList[i].time.TotalSeconds;
                                break;
                            case 1:
                                total = (routine.availableStartTime - DateTime.Now.TimeOfDay).TotalSeconds + notiAttriObjList[i].time.TotalSeconds;
                                break;
                            case 2:
                                total = (routine.availableEndTime - DateTime.Now.TimeOfDay).TotalSeconds + notiAttriObjList[i].time.TotalSeconds;
                                break;
                        }

                        notiAttriObjList[i].message = userTimeDict["message"].ToString();

                        if (!routine.isComplete && total > 0 && !routine.Notification.user.before.is_set)
                        {
                            string title = titles[i];
                            //subtitle is not used, this is only for setting user info for now
                            string subtitle = grIdx + routine.id;
                            string message = "Open the app to review your tasks. " + notiAttriObjList[i].message;
                            notificationManager.ScheduleNotification(title, subtitle, message, total);
                            firebaseFunctionsService.GRUserNotificationSetToTrue(routine, grIdx.ToString(), notiTimeKeysList[i]);

                        }
                        Console.WriteLine("total : " + total);
                        Console.WriteLine("before message: " + notiAttriObjList[i].message);
                    }
                }
                catch
                {

                }
            }
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

        public bool convertBinToBool(string obj)
        {
            bool ret = false;
            if (obj == "1" || obj == "True")
                ret = true;

            else if (obj == "0" || obj == "False")
                ret = false;

            return ret;



        }
    }
}
