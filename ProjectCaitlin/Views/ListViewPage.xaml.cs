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

            LoadDataAsync();

            labelFont = Device.RuntimePlatform == Device.iOS ? "Lobster-Regular" :
                Device.RuntimePlatform == Device.Android ? "Lobster-Regular.ttf#Lobster-Regular" : "Assets/Fonts/Lobster-Regular.ttf#Lobster";

            user = App.user;
            dateTimeNow = DateTime.Now;
            FSMethods = new FirestoreMethods("7R6hAVmDrNutRkG3sVRy");

            StartTimer();

            //BindingContext = DailyViewModel.Instance;
            SetupUI();
            //dailyViewModel = (DailyViewModel)BindingContext;

        }

        void SetupUI()
        {
            DayOfWeekLabel.Text = dateTimeNow.DayOfWeek.ToString();

            MorningGoalsPopulate();

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
            await MorningEventsLoad();
            sortData();
        }

        private void sortData()
        {
            user.routines.Sort((x, y) => DateTime.Compare(x.availableStartTime, y.availableStartTime));
            //eventsList.Sort((x, y) => DateTime.Compare(x.Start, y.Start));
        }

        public async Task RefreshPage()
        {
            DeleteRoutinePageElements();
            await FSMethods.LoadUser();
            SetupUI();

        }

        void DeleteRoutinePageElements()
        {
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

        private void MorningRoutinesPopulate()
        {
            foreach (routine routine in user.routines)
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

                MorningREStackLayout.Children.Add(frame);
            }
        }

        private void MorningGoalsPopulate()
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

        public async Task MorningEventsLoad()
        {
            //Call Google API
            var googleService = new GoogleService();

            publicYear = dateTimeNow.Year;
            publicMonth = (dateTimeNow.Month);
            publicDay = dateTimeNow.Day;

            string timeZoneOffset = DateTimeOffset.Now.ToString();
            string[] timeZoneOffsetParsed = timeZoneOffset.Split('-');
            int timeZoneNum = Int32.Parse(timeZoneOffsetParsed[1].Substring(0, 2));

            DateTime currentTimeinUTC = DateTime.Now.ToUniversalTime();
            uTCHour = (currentTimeinUTC.Hour - timeZoneNum);
            currentLocalUTCMinute = currentTimeinUTC.Minute;


            var jsonResult = await googleService.GetSpecificEventsList(publicYear, publicMonth, publicDay, 0, 0, timeZoneNum);


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
                }
            }
            catch (ArgumentNullException e)
            {
                //LoginPage.accessToken = LoginPage.refreshToken;
                //await Navigation.PopAsync();
                //Console.WriteLine(LoginPage.accessToken);
            }
        }

        void PopulateEvents()
        {

            foreach (var events in eventsList)
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
                    Text = events.Start.DateTime.ToString("h:mm tt"),
                    FontSize = 10,
                    TextColor = Color.White,
                    VerticalOptions = LayoutOptions.Start,
                    FontFamily = labelFont
                };

                Label EventTitleLabel = new Label
                {
                    Text = events.EventName,
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    FontFamily = labelFont

                };

                Label expectedTimeLabel = new Label
                {
                    Text = events.EventName,
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

                MorningREStackLayout.Children.Add(frame);
            }
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
