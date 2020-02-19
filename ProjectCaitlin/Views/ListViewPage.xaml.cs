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

        FirestoreMethods FSMethods;

        user user;
        //public DailyViewModel dailyViewModel;

        public ListViewPage()
        {
            InitializeComponent();

            SetupUIAsync();

            //dateTimeNow = DateTime.Now;
            dateTimeNow = new DateTime(2020, 2, 19, 10, 0, 0);

            labelFont = Device.RuntimePlatform == Device.iOS ? "Lobster-Regular" :
                Device.RuntimePlatform == Device.Android ? "Lobster-Regular.ttf#Lobster-Regular" : "Assets/Fonts/Lobster-Regular.ttf#Lobster";

            user = App.user;
            FSMethods = new FirestoreMethods("7R6hAVmDrNutRkG3sVRy");

            StartTimer();
        }

        async Task SetupUIAsync()
        {
            await LoadDataAsync();
            DayOfWeekLabel.Text = dateTimeNow.DayOfWeek.ToString();

            //dateTimeNow = DateTime.Now;
            HidePreviousTimeOfDayElements(dateTimeNow.TimeOfDay);

            try
            {
                PopulatePage();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine("get here?");

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) => {
                ReloadImage.GestureRecognizers.RemoveAt(0);
                ReloadImage.Source = "redoGray.png";
                await RefreshPage();
                ReloadImage.Source = "redo.png";
            };
            ReloadImage.GestureRecognizers.Add(tapGestureRecognizer);

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

        private async Task LoadDataAsync()
        {
            await EventsLoad();
            sortData();
        }

        private void sortData()
        {
            user.routines.Sort((x, y) => TimeSpan.Compare(x.availableStartTime.TimeOfDay, y.availableStartTime.TimeOfDay));

            for (int i = 0; i < user.routines.Count; i++)
                Console.WriteLine("routine " + (i + 1) + ": " + user.routines[i].title);
            //eventsList.Sort((x, y) => TimeSpan.Compare(x.Start.DateTime.DateTime.TimeOfDay, y.Start.DateTime.DateTime.TimeOfDay));
        }

        public async Task RefreshPage()
        {
            DeletePageElements();
            await FSMethods.LoadUser();
            await SetupUIAsync();

        }

        void DeletePageElements()
        {
            // Empty events
            eventsList = new List<EventsItems>();
            List<View> MorningREElements = new List<View>();
            List<View> MorningGoalsListElements = new List<View>();
            List<View> AfternoonREElements = new List<View>();
            List<View> AfternoonGoalsListElements = new List<View>();
            List<View> EveningREElements = new List<View>();
            List<View> EveningGoalsListElements = new List<View>();
            List<View> NightREElements = new List<View>();
            List<View> NightGoalsListElements = new List<View>();

            foreach (View element in MorningREStackLayout.Children)
                MorningREElements.Add(element);
            foreach (View element in MorningGoalsStackLayout.Children)
                MorningGoalsListElements.Add(element);
            foreach (View element in AfternoonREStackLayout.Children)
                AfternoonREElements.Add(element);
            foreach (View element in AfternoonGoalsStackLayout.Children)
                AfternoonGoalsListElements.Add(element);
            foreach (View element in EveningREStackLayout.Children)
                EveningREElements.Add(element);
            foreach (View element in EveningGoalsStackLayout.Children)
                EveningGoalsListElements.Add(element);
            foreach (View element in NightREStackLayout.Children)
                NightREElements.Add(element);
            foreach (View element in NightGoalsStackLayout.Children)
                NightGoalsListElements.Add(element);

            foreach (View element in MorningREElements)
                if (!(element is Label))
                    MorningREStackLayout.Children.Remove(element);
            foreach (View element in MorningGoalsListElements)
                if (!(element is Label))
                    MorningGoalsStackLayout.Children.Remove(element);
            foreach (View element in AfternoonREElements)
                if (!(element is Label))
                    AfternoonREStackLayout.Children.Remove(element);
            foreach (View element in AfternoonGoalsListElements)
                if (!(element is Label))
                    AfternoonGoalsStackLayout.Children.Remove(element);
            foreach (View element in EveningREElements)
                if (!(element is Label))
                    EveningREStackLayout.Children.Remove(element);
            foreach (View element in EveningGoalsListElements)
                if (!(element is Label))
                    EveningGoalsStackLayout.Children.Remove(element);
            foreach (View element in NightREElements)
                if (!(element is Label))
                    NightREStackLayout.Children.Remove(element);
            foreach (View element in NightGoalsListElements)
                if (!(element is Label))
                    NightGoalsStackLayout.Children.Remove(element);
        }

        private void PopulatePage()
        {
            //foreach (goal goal in user.goals)
            //    foreach (StackLayout stackLayout in GetInTimeOfDayList(goal.availableStartTime.TimeOfDay, goal.availableEndTime.TimeOfDay))
            //        PopulateGoal(goal, stackLayout);

            PopulateEventsAndRoutines(0, 0);
        }

        private void PopulateEventsAndRoutines(int eventIdx, int routineIdx)
        {

            Console.WriteLine("eventIdx: " + eventIdx);
            Console.WriteLine("routineIdx: " + routineIdx);

            TimeSpan currentTime = dateTimeNow.TimeOfDay;
            if (eventsList.Count == eventIdx && user.routines.Count == routineIdx)
                return;

            if (eventsList.Count == eventIdx)
            {
                PopulateRoutine(user.routines[routineIdx], GetFirstInTimeOfDay(currentTime, user.routines[routineIdx].availableStartTime.TimeOfDay, user.routines[routineIdx].availableEndTime.TimeOfDay));
                PopulateEventsAndRoutines(eventIdx, ++routineIdx);
            }
            if (user.routines.Count == routineIdx)
            {
                PopulateEvent(eventsList[eventIdx], GetFirstInTimeOfDay(currentTime, eventsList[eventIdx].Start.DateTime.DateTime.TimeOfDay, eventsList[eventIdx].End.DateTime.DateTime.TimeOfDay));
                PopulateEventsAndRoutines(++eventIdx, routineIdx);
            }

            if (user.routines[routineIdx].availableStartTime.TimeOfDay <= eventsList[eventIdx].Start.DateTime.TimeOfDay)
            {
                PopulateEvent(eventsList[eventIdx], GetFirstInTimeOfDay(currentTime, eventsList[eventIdx].Start.DateTime.DateTime.TimeOfDay, eventsList[eventIdx].End.DateTime.DateTime.TimeOfDay));
                PopulateEventsAndRoutines(++eventIdx, routineIdx);
            }
            else
            {
                PopulateRoutine(user.routines[routineIdx], GetFirstInTimeOfDay(currentTime, user.routines[routineIdx].availableStartTime.TimeOfDay, user.routines[routineIdx].availableEndTime.TimeOfDay));
                PopulateEventsAndRoutines(eventIdx, ++routineIdx);
            }
        }

        private void PopulateRoutine(routine routine, StackLayout stackLayout)
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
                Text = "Takes me " + 5.ToString() + " minutes",
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

            stackLayoutInner.Children.Add(routineTitleLabel);
            stackLayoutInner.Children.Add(expectedTimeLabel);

            stackLayoutOuter.Children.Add(stackLayoutInner);
            stackLayoutOuter.Children.Add(image);

            frame.Content = stackLayoutOuter;

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
                Source = Xamarin.Forms.ImageSource.FromResource("https://image.freepik.com/free-vector/calendar-with-clock-as-waiting-scheduled-event-icon-symbol-isolated-flat-cartoon_101884-758.jpg"),
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

        private void PopulateGoal(goal goal, StackLayout stackLayout)
        {
            StackLayout goalStackLayout = new StackLayout()
            {
                Margin = new Thickness(0, 0, 10, 0)
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

            Label goalLabel = new Label
            {
                Text = goal.title,
                TextColor = Color.DimGray,
                FontFamily = labelFont,
                HorizontalOptions = LayoutOptions.Start,

            };

            goalStackLayout.Children.Add(image);
            goalStackLayout.Children.Add(goalLabel);

            stackLayout.Children.Add(stackLayout);
        }

        internal void StartTimer()
        {
            int seconds = 10 * 1000;

            var timer =
                new Timer(TimerReset, null, 0, seconds);
        }

        private void TimerReset(object o)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    MorningEventsLoad();
            //});
        }

        public async Task EventsLoad()
        {
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

        private StackLayout GetFirstInTimeOfDay(TimeSpan currentTime, TimeSpan startTime, TimeSpan endTime)
        {
            Console.WriteLine("startTime: " + startTime.ToString());
            Console.WriteLine("endTime: " + endTime.ToString());

            //currentTime = testTime;

            if (currentTime < morningEnd
                && ((startTime < morningEnd && morningStart < endTime)
                || startTime == morningStart || morningEnd == endTime))

            {
                Console.WriteLine("Morning");

                return MorningREStackLayout;
            }
            if (currentTime < afternoonEnd
                && ((startTime < afternoonEnd && afternoonStart < endTime)
                || startTime == afternoonStart || afternoonEnd == endTime))
            {
                Console.WriteLine("Afternoon");

                return AfternoonREStackLayout;
            }
            if (currentTime < eveningEnd
                && ((startTime < eveningEnd && eveningStart < endTime)
                || startTime == eveningStart || eveningEnd == endTime))
            {
                Console.WriteLine("Evening");

                return EveningREStackLayout;
            }
            if ((startTime < nightEnd && nightStart < endTime)
                || startTime == nightStart || nightEnd == endTime)
            {
                Console.WriteLine("Night");

                return NightREStackLayout;
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

        public async void PrepareRefreshEvents()
        {
            await Task.Delay(1000);
            dateTimeNow = DateTime.Now;
            await RefreshEvents();
        }

        public async Task RefreshEvents()
        {

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
    }
}
