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

        List<EventsItems> eventsList = new List<EventsItems>();

        DateTime dateTimeNow;

        string labelFont;

        FirestoreMethods FSMethods;

        user user;
        //public DailyViewModel dailyViewModel;

        public ListViewPage()
        {
            InitializeComponent();
            dateTimeNow = DateTime.Now;

            SetupUIAsync();

            labelFont = Device.RuntimePlatform == Device.iOS ? "Lobster-Regular" :
                Device.RuntimePlatform == Device.Android ? "Lobster-Regular.ttf#Lobster-Regular" : "Assets/Fonts/Lobster-Regular.ttf#Lobster";

            user = App.user;
            FSMethods = new FirestoreMethods("7R6hAVmDrNutRkG3sVRy");

            StartTimer();

            //BindingContext = DailyViewModel.Instance;
            
            //dailyViewModel = (DailyViewModel)BindingContext;

        }

        async Task SetupUIAsync()
        {
            await LoadDataAsync();
            DayOfWeekLabel.Text = dateTimeNow.DayOfWeek.ToString();

            foreach (StackLayout stackLayout in assignTimeofDay(user.routines[routineIdx].availableStartTime.TimeOfDay, user.routines[routineIdx].availableEndTime.TimeOfDay))
                PopulateMorningGoals();
            PopulatePage(0,0);

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

            foreach (View element in MorningREStackLayout.Children)
            {
                MorningREElements.Add(element);
            }
            foreach (View element in MorningGoalsStackLayout.Children)
            {
                MorningGoalsListElements.Add(element);
            }
            
            foreach (View element in MorningREElements)
            {
                MorningREStackLayout.Children.Remove(element);
            }
            foreach (View element in MorningGoalsListElements)
            {
                MorningGoalsStackLayout.Children.Remove(element);
            }
            
        }

        private void PopulatePage(int eventIdx, int routineIdx)
        {

            Console.WriteLine("eventIdx: " + eventIdx);
            Console.WriteLine("routineIdx: " + routineIdx);


            if (eventsList.Count == eventIdx && user.routines.Count == routineIdx)
                return;

            if (eventsList.Count == eventIdx)
            {
                foreach (StackLayout stackLayout in assignTimeofDay(user.routines[routineIdx].availableStartTime.TimeOfDay, user.routines[routineIdx].availableEndTime.TimeOfDay))
                    PopulateRoutine(user.routines[routineIdx], stackLayout);

                PopulatePage(eventIdx, ++routineIdx);

            }
            if (user.routines.Count == routineIdx)
            {
                foreach (StackLayout stackLayout in assignTimeofDay(eventsList[eventIdx].Start.DateTime.DateTime.TimeOfDay, eventsList[eventIdx].End.DateTime.DateTime.TimeOfDay))
                    PopulateEvent(eventsList[eventIdx], stackLayout);

                PopulatePage(++eventIdx, routineIdx);
            }

            if (user.routines[routineIdx].availableStartTime.TimeOfDay < eventsList[eventIdx].Start.DateTime.TimeOfDay)
            {
                foreach (StackLayout stackLayout in assignTimeofDay(user.routines[routineIdx].availableStartTime.TimeOfDay, user.routines[routineIdx].availableEndTime.TimeOfDay))
                    PopulateRoutine(user.routines[routineIdx], stackLayout);

                PopulatePage(eventIdx, ++routineIdx);
            }
            else if (user.routines[routineIdx].availableStartTime.TimeOfDay == eventsList[eventIdx].Start.DateTime.TimeOfDay)
            {
                foreach (StackLayout stackLayout in assignTimeofDay(eventsList[eventIdx].Start.DateTime.DateTime.TimeOfDay, eventsList[eventIdx].End.DateTime.DateTime.TimeOfDay))
                    PopulateEvent(eventsList[eventIdx], stackLayout);

                PopulatePage(++eventIdx, routineIdx);
            }
            else
            {
                foreach (StackLayout stackLayout in assignTimeofDay(eventsList[eventIdx].Start.DateTime.DateTime.TimeOfDay, eventsList[eventIdx].End.DateTime.DateTime.TimeOfDay))
                    PopulateEvent(eventsList[eventIdx], stackLayout);

                PopulatePage(++eventIdx, routineIdx);
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

        private void PopulateMorningGoals()
        {
            foreach (goal goal in user.goals)
            {
                StackLayout stackLayout = new StackLayout()
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

                stackLayout.Children.Add(image);
                stackLayout.Children.Add(goalLabel);

                MorningGoalsStackLayout.Children.Add(stackLayout);
            }
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

        private List<StackLayout> assignTimeofDay(TimeSpan startTime, TimeSpan endTime)
        {
            List<StackLayout> result = new List<StackLayout>();
            TimeSpan morningStart = new TimeSpan(6, 0, 0);
            TimeSpan morningEnd = new TimeSpan(11, 0, 0);
            TimeSpan afternoonStart = new TimeSpan(11, 0, 0);
            TimeSpan afternoonEnd = new TimeSpan(18, 0, 0);
            TimeSpan eveningStart = new TimeSpan(18, 0, 0);
            TimeSpan eveningEnd = new TimeSpan(23, 59, 59);
            TimeSpan nightStart = new TimeSpan(0, 0, 0);
            TimeSpan nightEnd = new TimeSpan(6, 0, 0);

            Console.WriteLine("startTime: " + startTime.ToString());
            Console.WriteLine("endTime: " + endTime.ToString());
            if ((startTime < morningEnd && morningStart < endTime)
                ||startTime == morningStart || morningEnd == endTime)
            {
                Console.WriteLine("Morning");

                result.Add(MorningREStackLayout);
            }
            if ((startTime < afternoonEnd && afternoonStart < endTime)
                || startTime == afternoonStart || afternoonEnd == endTime)
            {
                Console.WriteLine("Afternoon");

                result.Add(AfternoonREStackLayout);
            }
            if ((startTime < eveningEnd && eveningStart < endTime)
                || startTime == eveningStart || eveningEnd == endTime)
            {
                Console.WriteLine("Evening");

                result.Add(EveningREStackLayout);
            }
            if ((startTime < nightEnd && nightStart < endTime)
                || startTime == nightStart || nightEnd == endTime)
            {
                Console.WriteLine("Night");

                result.Add(NightREStackLayout);
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
    }
}
