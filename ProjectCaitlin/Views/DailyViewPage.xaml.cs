using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectCaitlin.Models;
using ProjectCaitlin.Services;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class DailyViewPage : ContentPage
    {
        user user;

        public int publicYear;
        public int publicMonth;
        public int publicDay;
        public int uTCHour;
        public int currentLocalUTCMinute;

        DateTime dateTimeNow;

        public DailyViewPage()
        {
            InitializeComponent();
            BindingContext = this;
            user = App.user;
            setupUI();
            StartTimer();
            PrepSetUpcomingEvents();
        }


        internal void StartTimer()
        {
            int seconds = 60 * 1000;

            var timer =
                new Timer(TimerReset, null, 0, seconds);
        }

        private void TimerReset(object o)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                PrepSetUpcomingEvents();
            });
        }

        public async void PrepSetUpcomingEvents()
        {
            dateTimeNow = DateTime.Now;
            await SetUpcomingEvents();
        }

        public async Task<string> SetUpcomingEvents()
        {
            //Call Google API
            var googleService = new GoogleService();

            publicYear = dateTimeNow.Year; 
            publicMonth = (dateTimeNow.Month);
            publicDay = dateTimeNow.Day;

            string timeZoneOffset = DateTimeOffset.Now.ToString();
            string[] timeZoneOffsetParsed = timeZoneOffset.Split('-');
            int timeZoneNum = Int32.Parse(timeZoneOffsetParsed[1].Substring(0, 2));

            var currentTimeinUTC = DateTime.Now.ToUniversalTime();
            uTCHour = (currentTimeinUTC.Hour - timeZoneNum);
            currentLocalUTCMinute = currentTimeinUTC.Minute;


            var jsonResult = await googleService.GetSpecificEventsList(publicYear, publicMonth, publicDay, uTCHour, currentLocalUTCMinute, timeZoneNum);


            //Return error if result is empty
            if (jsonResult == null)
            {
                await DisplayAlert("Oops!", "There was an error listing your events", "OK");
            }

            //Parse the json using EventsList Method

            try
            {

                var parsedResult = JsonConvert.DeserializeObject<Methods.GetEventsListMethod>(jsonResult);

                //Create Item List 
                var eventList = new List<string>();
                var dateList = new List<string>();
                var startTimeList = new List<string>();
                var endTimeList = new List<string>();


                //Separate out just the EventName 
                foreach (var events in parsedResult.Items)
                {
                    eventList.Add(events.EventName);
                    dateList.Add(events.Start.DateTime.ToString());
                    startTimeList.Add(events.Start.DateTime.ToString());
                    endTimeList.Add(events.End.DateTime.ToString());
                }

                try
                {
                    eventBtn2.Text = eventList[1];
                }
                catch (ArgumentOutOfRangeException e)
                {
                    eventBtn2.Text = "";
                }

                try
                {
                    eventBtn1.Text = eventList[0];

                    await noEventsButton.FadeTo(0, 2000);
                    noEventsButton.IsVisible = false;

                    eventBtn1.IsVisible = true;
                    eventBtn2.IsVisible = true;
                    await Task.WhenAll(eventBtn1.FadeTo(1, 2000), eventBtn2.FadeTo(1, 2000));

                }
                catch (ArgumentOutOfRangeException e)
                {
                    await Task.WhenAll(eventBtn1.FadeTo(0, 2000), eventBtn2.FadeTo(0, 2000));
                    eventBtn1.IsVisible = false;
                    eventBtn2.IsVisible = false;

                    noEventsButton.IsVisible = true;
                    await noEventsButton.FadeTo(1, 2000);

                }

                return null;

            }
            catch (ArgumentNullException e)
            {
                //LoginPage.accessToken = LoginPage.refreshToken;
                //await Navigation.PopAsync();
                //Console.WriteLine(LoginPage.accessToken);
                return null;
            }
        }

        public async void MonthlyBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonthlyViewPage());
        }

        public async void ListBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListViewPage());
        }

        public void ReLoginClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        async void PrintID(object sender, System.EventArgs e)
        {
            var task = sender as task;

            Console.WriteLine("button clicked, ID: " + task.id);
        }

        async void PhotosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhotoDisplayPage());
        }

        public void setupUI()
        {
            Console.WriteLine("user.routines.Count: " + user.routines.Count);
            if (user.routines.Count > 0)
            {
                routineTitle.Text = user.routines[0].title;
            }

            foreach (routine routine in user.routines)
            {
                foreach (task task in routine.tasks)
                {

                    Button button = new Button
                    {
                        Text = task.title,
                        TextColor = Color.Black,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.Center,
                        ImageSource = task.photo,
                        WidthRequest = 80,
                        HeightRequest = 90,
                        CornerRadius = 5,
                        Margin = new Thickness(5, 20, 5, 10),
                        BackgroundColor = Color.WhiteSmoke,
                    };

                    taskList.Children.Add(button);

                    Console.WriteLine("task name: " + task.title);
                    Console.WriteLine("task id: " + task.id);
                    Console.WriteLine("task id: " + task.photo);

                }
            }

        }
    }
}
