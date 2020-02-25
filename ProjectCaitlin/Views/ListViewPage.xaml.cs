using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectCaitlin.Services;
//using ProjectCaitlin.ViewModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using ProjectCaitlin.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ProjectCaitlin.Views;
using ProjectCaitlin.Methods;
using System.Threading;

namespace ProjectCaitlin
{
    public partial class ListViewPage : ContentPage
    {
        private static List<string> eventNameList;

        List<StackLayout> EventAndRoutineStackLayouts = new List<StackLayout>();
        List<StackLayout> GoalsStackLayouts = new List<StackLayout>();
        public int oldDate;

        public int publicYear;
        public int publicMonth;
        public int publicDay;
        public int uTCHour;
        public int currentLocalUTCMinute;

        TimeSpan morningStart = new TimeSpan(6, 0, 0);
        TimeSpan morningEnd = new TimeSpan(11, 0, 0);
        TimeSpan afternoonStart = new TimeSpan(11, 0, 0);
        TimeSpan afternoonEnd = new TimeSpan(18, 0, 0);
        TimeSpan eveningStart = new TimeSpan(18, 0, 0);
        TimeSpan eveningEnd = new TimeSpan(23, 59, 59);
        TimeSpan nightStart = new TimeSpan(0, 0, 0);
        TimeSpan nightEnd = new TimeSpan(6, 0, 0);

        List<EventsItems> eventsList = new List<EventsItems>();

        DateTime dateTimeNow;

        string labelFont;

        FirestoreService firestoreService;

        user user;
        //public DailyViewModel dailyViewModel;

        public ListViewPage()
        {
            InitializeComponent();

            AddTapGestures();

            user = App.user;

            putLayoutsIntoLists();

            dateTimeNow = DateTime.Now;
            //dateTimeNow = new DateTime(2020, 2, 19, 10, 0, 0);

            labelFont = Device.RuntimePlatform == Device.iOS ? "Lobster-Regular" :
                Device.RuntimePlatform == Device.Android ? "Lobster-Regular.ttf#Lobster-Regular" : "Assets/Fonts/Lobster-Regular.ttf#Lobster";

            firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

            StartTimer();
            SetupUIAsync();
        }

        async Task SetupUIAsync()
        {
            DayOfWeekLabel.Text = dateTimeNow.DayOfWeek.ToString();

            //dateTimeNow = DateTime.Now;
            //HidePreviousTimeOfDayElements(dateTimeNow.TimeOfDay);

            await EventsLoad();

            try
            {
                DeletePageElements();
                PopulatePage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            mainScrollView.HeightRequest = Application.Current.MainPage.Height - NavBar.Height;
        }

        public async Task RefreshPage()
        {
            await firestoreService.LoadUser();
            await SetupUIAsync();
            PrintFirebaseUser();

        }

        void DeletePageElements()
        {
            List<List<View>> ElementsRandE = new List<List<View>>();
            List<List<View>> ElementsGoals = new List<List<View>>();


            for (int i = 0; i < EventAndRoutineStackLayouts.Count; i++)
            {
                int skipNum = 0;
                ElementsRandE.Add(new List<View>());
                foreach (View element in EventAndRoutineStackLayouts[i].Children)
                    if (skipNum++ != 0)
                        ElementsRandE[i].Add(element);
            }

            for (int i = 0; i < GoalsStackLayouts.Count; i++)
            {
                ElementsGoals.Add(new List<View>());
                foreach (View element in GoalsStackLayouts[i].Children)
                    ElementsGoals[i].Add(element);
            }

            for (int i = 0; i < EventAndRoutineStackLayouts.Count; i++)
            {
                foreach (View element in ElementsRandE[i])
                    EventAndRoutineStackLayouts[i].Children.Remove(element);
            }

            for (int i = 0; i < EventAndRoutineStackLayouts.Count; i++)
            {
                foreach (View element in ElementsGoals[i])
                    GoalsStackLayouts[i].Children.Remove(element);
            }
        }

        private void PopulatePage()
        {
            //foreach (goal goal in user.goals)
            //    foreach (StackLayout stackLayout in GetInTimeOfDayList(goal.availableStartTime.TimeOfDay, goal.availableEndTime.TimeOfDay))
            //        PopulateGoal(goal, stackLayout);

            int goalIdx = 0;
            foreach (goal goal in user.goals)
                PopulateGoal(goal, goalIdx++, GetFirstInTimeOfDay("goal", dateTimeNow.TimeOfDay, goal.availableStartTime.TimeOfDay, goal.availableEndTime.TimeOfDay));

            PopulateEventsAndRoutines(0, 0);
        }

        private void PopulateEventsAndRoutines(int eventIdx, int routineIdx)
        {

            Console.WriteLine("eventIdx: " + eventIdx);
            Console.WriteLine("routineIdx: " + routineIdx);

            Console.WriteLine("eventcount: " + eventsList.Count);
            Console.WriteLine("routinecount: " + user.routines.Count);

            TimeSpan currentTime = dateTimeNow.TimeOfDay;
            if (eventsList.Count == eventIdx && user.routines.Count == routineIdx)
                return;

            if (eventsList.Count == eventIdx)
            {
                PopulateRoutine(user.routines[routineIdx], routineIdx, GetFirstInTimeOfDay("routine", currentTime, user.routines[routineIdx].availableStartTime.TimeOfDay, user.routines[routineIdx].availableEndTime.TimeOfDay));
                PopulateEventsAndRoutines(eventIdx, ++routineIdx);
                return;
            }
            if (user.routines.Count == routineIdx)
            {
                PopulateEvent(eventsList[eventIdx], GetFirstInTimeOfDay("routine", currentTime, eventsList[eventIdx].Start.DateTime.DateTime.TimeOfDay, eventsList[eventIdx].End.DateTime.DateTime.TimeOfDay));
                PopulateEventsAndRoutines(++eventIdx, routineIdx);
                return;
            }

            if (user.routines[routineIdx].availableStartTime.TimeOfDay <= eventsList[eventIdx].Start.DateTime.TimeOfDay)
            {
                PopulateEvent(eventsList[eventIdx], GetFirstInTimeOfDay("routine", currentTime, eventsList[eventIdx].Start.DateTime.DateTime.TimeOfDay, eventsList[eventIdx].End.DateTime.DateTime.TimeOfDay));
                PopulateEventsAndRoutines(++eventIdx, routineIdx);
                return;
            }
            else
            {
                PopulateRoutine(user.routines[routineIdx], routineIdx, GetFirstInTimeOfDay("routine", currentTime, user.routines[routineIdx].availableStartTime.TimeOfDay, user.routines[routineIdx].availableEndTime.TimeOfDay));
                PopulateEventsAndRoutines(eventIdx, ++routineIdx);
                return;
            }
        }

        private void PopulateRoutine(routine routine, int routineIdx, StackLayout stackLayout)
        {
            Frame frame = new Frame
            {
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(10, 10, 10, 10),
                Margin = new Thickness(0, 2, 0, 2)
            };

            StackLayout stackLayoutOuter = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            StackLayout stackLayoutInner = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };

            Label routineTitleLabel = new Label
            {
                Text = routine.title,
                FontSize = 20,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = labelFont

            };

            Label expectedTimeLabel = new Label
            {
                Text = "Takes me " + "x".ToString() + " minutes",
                FontSize = 10,
                TextColor = Color.DimGray,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontFamily = labelFont

            };

            CachedImage image = new CachedImage()
            {
                Source = routine.photo,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                Transformations = new List<ITransformation>()
                {
                    new CircleTransformation(),
                },
            };

            var indicator = new ActivityIndicator { Color = Color.Gray, };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = image;

            stackLayoutInner.Children.Add(routineTitleLabel);
            stackLayoutInner.Children.Add(expectedTimeLabel);

            stackLayoutOuter.Children.Add(stackLayoutInner);
            stackLayoutOuter.Children.Add(image);

            frame.Content = stackLayoutOuter;

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                await Navigation.PushAsync(new TaskPage(routineIdx, true));
            };
            frame.GestureRecognizers.Add(tapGestureRecognizer);

            if (routine.isComplete)
            {
                StackLayout completeStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal
                };

                CachedImage checkmarkImage = new CachedImage()
                {
                    Source = Xamarin.Forms.ImageSource.FromFile("greencheckmarkicon.png"),
                    WidthRequest = 30,
                    HeightRequest = 30,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                completeStackLayout.Children.Add(checkmarkImage);

                frame.HorizontalOptions = LayoutOptions.FillAndExpand;
                completeStackLayout.Children.Add(frame);

                stackLayout = GetCompleteTimeOfDay(routine.dateTimeCompleted.TimeOfDay);
                stackLayout.Children.Add(completeStackLayout);

            }
            else
                stackLayout.Children.Add(frame);
        }

        private void PopulateEvent(EventsItems event_, StackLayout stackLayout)
        {
            Frame frame = new Frame
            {
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(10, 10, 10, 10),
                Margin = new Thickness(0, 2, 0, 2),
                BackgroundColor = Color.Goldenrod
            };

            StackLayout stackLayoutOuter = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            StackLayout stackLayoutInner = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };

            Label EventTimeLabel = new Label
            {
                Text = event_.Start.DateTime.ToString("h:mm tt") + " - " + event_.End.DateTime.ToString("h:mm tt"),
                FontSize = 10,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                FontFamily = labelFont
            };

            Label EventTitleLabel = new Label
            {
                Text = event_.EventName,
                FontSize = 20,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = labelFont

            };

            Label expectedTimeLabel = new Label
            {
                Text = event_.EventName,
                FontSize = 10,
                TextColor = Color.DimGray,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontFamily = labelFont

            };

            CachedImage image = new CachedImage()
            {
                Source = Xamarin.Forms.ImageSource.FromFile("eventIcon.jpg"),
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                Transformations = new List<ITransformation>()
                {
                    new CircleTransformation(),
                },
            };

            stackLayoutInner.Children.Add(EventTimeLabel);
            stackLayoutInner.Children.Add(EventTitleLabel);
            stackLayoutInner.Children.Add(expectedTimeLabel);

            stackLayoutOuter.Children.Add(stackLayoutInner);
            stackLayoutOuter.Children.Add(image);

            frame.Content = stackLayoutOuter;

            stackLayout.Children.Add(frame);
        }

        private void PopulateGoal(goal goal, int goalIdx, StackLayout stackLayout)
        {
            StackLayout goalStackLayout = new StackLayout()
            {
                Margin = new Thickness(0, 0, 10, 0)
            };

            Grid grid = new Grid()
            {

            };

            CachedImage image = new CachedImage()
            {
                Source = goal.photo,
                WidthRequest = 120,
                HeightRequest = 90,
                HorizontalOptions = LayoutOptions.Start,
                Transformations = new List<ITransformation>()
                {
                    new RoundedTransformation(30, 120, 90),
                },
            };
            if (goal.isComplete)
            {
                image.Opacity = .6;
            }

            CachedImage checkmarkImage = new CachedImage()
            {
                Source = Xamarin.Forms.ImageSource.FromFile("greencheckmarkicon.png"),
                WidthRequest = 70,
                HeightRequest = 70,
                IsVisible = goal.isComplete,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label goalLabel = new Label
            {
                Text = goal.title,
                TextColor = Color.DimGray,
                FontFamily = labelFont,
                HorizontalOptions = LayoutOptions.Start,

            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                await Navigation.PushAsync(new TaskPage(goalIdx, false));
            };
            goalStackLayout.GestureRecognizers.Add(tapGestureRecognizer);

            var indicator = new ActivityIndicator { Color = Color.Gray, };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = image;

            grid.Children.Add(image, 0, 0);
            grid.Children.Add(checkmarkImage, 0, 0);

            goalStackLayout.Children.Add(grid);
            goalStackLayout.Children.Add(goalLabel);

            stackLayout.Children.Add(goalStackLayout);
        }

        private void putLayoutsIntoLists()
        {
            EventAndRoutineStackLayouts.Add(MorningREStackLayout);
            EventAndRoutineStackLayouts.Add(AfternoonREStackLayout);
            EventAndRoutineStackLayouts.Add(EveningREStackLayout);
            EventAndRoutineStackLayouts.Add(NightREStackLayout);

            GoalsStackLayouts.Add(MorningGoalsStackLayout);
            GoalsStackLayouts.Add(AfternoonGoalsStackLayout);
            GoalsStackLayouts.Add(EveningGoalsStackLayout);
            GoalsStackLayouts.Add(NightGoalsStackLayout);
        }

        public async Task EventsLoad()
        {
            // empty current events
            eventsList = new List<EventsItems>();

            //Call Google API
            var googleService = new GoogleService();

            publicYear = dateTimeNow.Year;
            publicMonth = dateTimeNow.Month;
            publicDay = dateTimeNow.Day;

            string timeZoneOffset = DateTimeOffset.Now.ToString();
            string[] timeZoneOffsetParsed = timeZoneOffset.Split('-');
            int timeZoneNum = Int32.Parse(timeZoneOffsetParsed[1].Substring(0, 2));

            DateTime currentTimeinUTC = DateTime.Now.ToUniversalTime();
            uTCHour = (currentTimeinUTC.Hour - timeZoneNum);
            currentLocalUTCMinute = currentTimeinUTC.Minute;


            var jsonResult = await googleService.GetAllTodaysEventsList(publicYear, publicMonth, publicDay, timeZoneNum);
            //var jsonResult = await googleService.GetEventsList();

            Console.WriteLine("jsonResult event: " + jsonResult);

            //Return error if result is empty
            if (jsonResult == null)
            {
                await DisplayAlert("Oops!", "There was an error listing your events", "OK");
            }

            //Parse the json using EventsList Method

            try
            {

                var parsedResult = JsonConvert.DeserializeObject<Methods.GetEventsListMethod>(jsonResult);

                //Separate out just the EventName
                foreach (var events in parsedResult.Items)
                {
                    eventsList.Add(events);
                    Console.WriteLine(events.EventName.ToString());
                }
            }
            catch (ArgumentNullException e)
            {
                //LoginPage.accessToken = LoginPage.refreshToken;
                //await Navigation.PopAsync();
                //Console.WriteLine(LoginPage.accessToken);
            }
        }

        private StackLayout GetFirstInTimeOfDay(string GorR,TimeSpan currentTime, TimeSpan startTime, TimeSpan endTime)
        {
            Console.WriteLine("startTime: " + startTime.ToString());
            Console.WriteLine("endTime: " + endTime.ToString());

            //currentTime = testTime;

            if (currentTime < morningEnd
                && ((startTime < morningEnd && morningStart < endTime)
                || startTime == morningStart || morningEnd == endTime))

            {
                Console.WriteLine("Morning");

                if (GorR == "routine")
                    return MorningREStackLayout;
                else
                    return MorningGoalsStackLayout;
            }
            if (currentTime < afternoonEnd
                && ((startTime < afternoonEnd && afternoonStart < endTime)
                || startTime == afternoonStart || afternoonEnd == endTime))
            {
                Console.WriteLine("Afternoon");

                if (GorR == "routine")
                    return AfternoonREStackLayout;
                else
                    return AfternoonGoalsStackLayout;
            }
            if (currentTime < eveningEnd
                && ((startTime < eveningEnd && eveningStart < endTime)
                || startTime == eveningStart || eveningEnd == endTime))
            {
                Console.WriteLine("Evening");

                if (GorR == "routine")
                    return EveningREStackLayout;
                else
                    return EveningGoalsStackLayout;
            }
            if ((startTime < nightEnd && nightStart < endTime)
                || startTime == nightStart || nightEnd == endTime)
            {
                Console.WriteLine("Night");

                if (GorR == "routine")
                    return NightREStackLayout;
                else
                    return NightGoalsStackLayout;
            }

            return new StackLayout();
        }

        private List<StackLayout> GetInTimeOfDayList(TimeSpan startTime, TimeSpan endTime)
        {
            List<StackLayout> result = new List<StackLayout>();

            Console.WriteLine("startTime: " + startTime.ToString());
            Console.WriteLine("endTime: " + endTime.ToString());
            if ((startTime < morningEnd && morningStart < endTime)
                || startTime == morningStart || morningEnd == endTime)
            {
                Console.WriteLine("Morning");

                result.Add(MorningGoalsStackLayout);
            }
            if ((startTime < afternoonEnd && afternoonStart < endTime)
                || startTime == afternoonStart || afternoonEnd == endTime)
            {
                Console.WriteLine("Afternoon");

                result.Add(AfternoonGoalsStackLayout);
            }
            if ((startTime < eveningEnd && eveningStart < endTime)
                || startTime == eveningStart || eveningEnd == endTime)
            {
                Console.WriteLine("Evening");

                result.Add(EveningGoalsStackLayout);
            }
            if ((startTime < nightEnd && nightStart < endTime)
                || startTime == nightStart || nightEnd == endTime)
            {
                Console.WriteLine("Night");

                result.Add(NightGoalsStackLayout);
            }

            return result;
        }

        private StackLayout GetCompleteTimeOfDay(TimeSpan completeTime)
        {
            Console.WriteLine("completeTime: " + completeTime.ToString());
            if (morningStart < completeTime && completeTime < morningEnd)
            {
                Console.WriteLine("Morning");

                return MorningREStackLayout;
            }
            if (afternoonStart < completeTime && completeTime < afternoonEnd)
            {
                Console.WriteLine("Afternoon");

                return AfternoonREStackLayout;
            }
            if (eveningStart < completeTime && completeTime < eveningEnd)
            {
                Console.WriteLine("Evening");

                return EveningREStackLayout;
            }
            if (nightStart < completeTime && completeTime < nightEnd)
            {
                Console.WriteLine("Night");

                return NightREStackLayout;
            }

            return new StackLayout();
        }

        internal void StartTimer()
        {
            //int seconds = 10 * 1000;

            //var timer =
            //    new Timer(TimerReset, null, 0, seconds);
        }

        private void TimerReset(object o)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    RefreshPage();
            //});
        }

        public async void PrepareRefreshEvents()
        {
            await Task.Delay(1000);
            dateTimeNow = DateTime.Now;
            await RefreshPage();
        }

        private void AddTapGestures()
        {
            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += async (s, e) => {
                ReloadImage.GestureRecognizers.RemoveAt(0);
                ReloadImage.Source = "redoGray.png";
                await RefreshPage();
                ReloadImage.Source = "redo.png";
                ReloadImage.GestureRecognizers.Add(tapGestureRecognizer1);
            };
            ReloadImage.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GreetingPage());
            };
            AboutMeButton.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += async (s, e) => {
                await Navigation.PushAsync(new PhotoDisplayPage());
            };
            MyPhotosButton.GestureRecognizers.Add(tapGestureRecognizer3);

            var tapGestureRecognizer4 = new TapGestureRecognizer();
            tapGestureRecognizer4.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
            };
            MyDayButton.GestureRecognizers.Add(tapGestureRecognizer4);
        }

        private void HidePreviousTimeOfDayElements(TimeSpan currentTime)
        {
            if (currentTime > morningEnd)
            {
                MorningREStackLayout.IsVisible = false;
                MorningGoalsSection.IsVisible = false;
            }
            if (currentTime > afternoonEnd)
            {
                AfternoonREStackLayout.IsVisible = false;
                AfternoonGoalsSection.IsVisible = false;
            }
            if (currentTime < nightEnd)
            {
                MorningREStackLayout.IsVisible = false;
                MorningGoalsSection.IsVisible = false;
                EveningREStackLayout.IsVisible = false;
                EveningGoalsSection.IsVisible = false;
                AfternoonREStackLayout.IsVisible = false;
                AfternoonGoalsSection.IsVisible = false;
            }
        }

        public async void btn1(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GreetingPage());
        }

        public async void btn3(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PhotoDisplayPage());
        }

        public async void btn4(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());
        }

        void PrintFirebaseUser()
        {
            OnPropertyChanged(nameof(App.user));
            Console.WriteLine("user first name: " + App.user.firstName);
            Console.WriteLine("user last name: " + App.user.lastName);

            foreach (routine routine in App.user.routines)
            {
                OnPropertyChanged(nameof(routine));
                Console.WriteLine("user routine title: " + routine.title);
                Console.WriteLine("user routine id: " + routine.id);
                foreach (task task in routine.tasks)
                {
                    OnPropertyChanged(nameof(task));
                    Console.WriteLine("user task title: " + task.title);
                    Console.WriteLine("user task id: " + task.id);
                    foreach (step step in task.steps)
                    {
                        OnPropertyChanged(nameof(step));
                        Console.WriteLine("user step title: " + step.title);
                    }
                }
            }

            foreach (goal goal in App.user.goals)
            {
                OnPropertyChanged(nameof(goal));
                Console.WriteLine("user goal title: " + goal.title);
                Console.WriteLine("user goal id: " + goal.id);
                foreach (action action in goal.actions)
                {
                    OnPropertyChanged(nameof(goal));
                    Console.WriteLine("user action title: " + action.title);
                    Console.WriteLine("user action id: " + action.id);
                    foreach (instruction instruction in action.instructions)
                    {
                        OnPropertyChanged(nameof(instruction));
                        Console.WriteLine("user instruction title: " + instruction.title);
                    }
                }
            }
        }
    }
}
