using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
using Xamarin.Forms;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace ProjectCaitlin
{

    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MonthlyViewPage : ContentPage
    {

        GooglePhotoService GooglePhotoService = new GooglePhotoService();

        //List<List<string>> photoURIs = new List<List<string>>();
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 1;
        Label[] labels = new Label[42];
        //Image[] images = new Image[42];

        public MonthlyViewPage()
        {
            InitializeComponent();
            //Add empty calendar
            int row = 1;
            int col = 0;
            for (int i = 0; i < 42; i++)
            {
                labels[i] = new Label();
                labels[i].Text = "";
                labels[i].FontSize = 20;
                labels[i].HorizontalOptions = LayoutOptions.Center;
                labels[i].VerticalOptions = LayoutOptions.Center;

                Grid.SetRow(labels[i], row);
                Grid.SetColumn(labels[i], col);

                col++;
                if (col == 7)
                {
                    row++;
                    col = 0;
                }
                StackLayoutMap.Children.Add(labels[i]);
            }

            var button1 = this.FindByName<ImageButton>("month2");
            button1.Clicked += ButtonOne;

            var button2 = this.FindByName<ImageButton>("month1");
            button2.Clicked += ButtonTwo;



            SetupUI();// add photos, update calendar by date, add navigation bar

        }

        private async void SetupUI()
        {
            App.User.photoURIs = await GooglePhotoService.GetPhotos();
            Grid controlGrid = new Grid();
            int rowLength = 3;
            double gridItemSize = (Application.Current.MainPage.Width / rowLength) - (1.2 * rowLength);
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = gridItemSize });
            for (int i = 0; i < rowLength; i++)
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = gridItemSize });
            var photoCount = 0;
            try
            {
                foreach (List<string> list in App.User.photoURIs)
                {
                    string photoURI = list[0];
                    string date = list[1];
                    string description = list[2];
                    string creationTime = list[3];

                    CachedImage webImage = new CachedImage
                    {
                        Source = Xamarin.Forms.ImageSource.FromUri(new Uri(photoURI)),
                        Transformations = new List<ITransformation>() {
                        new CropTransformation(),
                    },
                    };

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += async (s, e) => {
                        await Navigation.PushAsync(new PhotoDisplayPage(webImage, date, description, creationTime));
                    };
                    webImage.GestureRecognizers.Add(tapGestureRecognizer);
                    var indicator = new ActivityIndicator
                    {
                        Color = Color.Gray,
                        WidthRequest = gridItemSize,
                        HeightRequest = gridItemSize,
                    };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = webImage;

                    controlGrid.Children.Add(indicator, photoCount % rowLength, photoCount / rowLength);
                    controlGrid.Children.Add(webImage, photoCount % rowLength, photoCount / rowLength);
                    photoCount++;
                }
            }
            catch (NullReferenceException e)
            {
                var googleService = new GoogleService();
                if (await googleService.RefreshToken())
                {
                    Console.WriteLine("RefreshToken Done!");
                    App.User.photoURIs = await GooglePhotoService.GetPhotos();
                    //return await GetPhotos();
                }

                //var googleService = new GoogleService();
                // await googleService.RefreshToken();
                // Console.WriteLine("Here");
                //SetupUI();
            }

            //update calendar
            DateTime localDate = DateTime.Now;
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            var currentYear = myCal.GetYear(localDate);
            var currentMonth = myCal.GetMonth(localDate);
            var currentDay = myCal.GetDayOfWeek(localDate);

            Year = currentYear;
            Month = currentMonth;
            yearLabel.Text = Year + "";
            setMonthLabel(Month);
            SetCalendar(currentYear, currentMonth);

            //add navigation bar
            photoScrollView.HeightRequest = Application.Current.MainPage.Height - CalendarContent.Height - NavBar.Height;

            if (App.User.photoURIs != null)
            {
                photoScrollView.Content = controlGrid;
            }
            else
            {
                Label noPhotosLabel = new Label()
                {
                    Text = "No photos to Show",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    TextColor = Color.DimGray

                };
                photoScrollView.Content = noPhotosLabel;
            }
            AddTapGestures();

        }

        void setMonthLabel(int month)
        {
            if (month == 1)
                monthLabel.Text = "January";
            else if (month == 2)
                monthLabel.Text = "Feburary";
            else if (month == 3)
                monthLabel.Text = "March";
            else if (month == 4)
                monthLabel.Text = "April";
            else if (month == 5)
                monthLabel.Text = "May";
            else if (month == 6)
                monthLabel.Text = "June";
            else if (month == 7)
                monthLabel.Text = "July";
            else if (month == 8)
                monthLabel.Text = "August";
            else if (month == 9)
                monthLabel.Text = "September";
            else if (month == 10)
                monthLabel.Text = "October";
            else if (month == 11)
                monthLabel.Text = "November";
            else if (month == 12)
                monthLabel.Text = "December";

        }
        void ButtonOne(object sender, EventArgs args)
        {
            Month += 1;
            if (Month == 13)
            {
                Month = 1;
                Year += 1;
            }
            yearLabel.Text = Year + "";
            setMonthLabel(Month);
            SetCalendar(Year, Month);
        }

        void ButtonTwo(object sender, EventArgs args)
        {
            Month -= 1;
            if (Month == 0)
            {
                Month = 12;
                Year -= 1;
            }
            yearLabel.Text = Year + "";
            setMonthLabel(Month);
            SetCalendar(Year, Month);
        }


        public void SetCalendar(int year, int month)
        {
            //DateTime localDate = DateTime.Now;
            DateTime firstDay = new DateTime(year, month, 1);
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;

            var currentDay = myCal.GetDayOfWeek(firstDay);
            int startDay = 0;
            if (currentDay.ToString().Equals("Monday"))
                startDay = 1;
            else if (currentDay.ToString().Equals("Tuesday"))
                startDay = 2;
            else if (currentDay.ToString().Equals("Wednesday"))
                startDay = 3;
            else if (currentDay.ToString().Equals("Thursday"))
                startDay = 4;
            else if (currentDay.ToString().Equals("Friday"))
                startDay = 5;
            else if (currentDay.ToString().Equals("Saturday"))
                startDay = 6;

            int j = 1;
            int maxDay = 30;
            int lastMonth = 0;
            if (Month == 1)
            {
                maxDay = 31;
                lastMonth = 31;
            }
            else if (Month == 2)
            {
                if ((Year % 4 == 0 && Year % 100 != 0) || Year % 400 == 0)
                    maxDay = 29;
                else
                    maxDay = 28;
                lastMonth = 31;
            }
            else if (Month == 3)
            {
                maxDay = 31;
                if ((Year % 4 == 0 && Year % 100 != 0) || Year % 400 == 0)
                    lastMonth = 29;
                else
                    lastMonth = 28;
            }
            else if (Month == 4)
            {
                maxDay = 30;
                lastMonth = 31;
            }

            else if (Month == 5)
            {
                maxDay = 31;
                lastMonth = 30;
            }
            else if (Month == 6)
            {
                maxDay = 30;
                lastMonth = 31;
            }
            else if (Month == 7)
            {
                maxDay = 31;
                lastMonth = 30;
            }
            else if (Month == 8)
            {
                maxDay = 31;
                lastMonth = 31;
            }
            else if (Month == 9)
            {
                maxDay = 30;
                lastMonth = 31;
            }
            else if (Month == 10)
            {
                maxDay = 31;
                lastMonth = 30;
            }
            else if (Month == 11)
            {
                maxDay = 30;
                lastMonth = 30;
            }
            else if (Month == 12)
            {
                maxDay = 31;
                lastMonth = 30;
            }

            //update calendar before the start day.
            for (int i = startDay - 1; i >= 0; i--)
            {
                labels[i].Text = lastMonth + "";
                labels[i].FontSize = 15;

                labels[i].TextColor = Color.FromHex("#c2c6cc");
                labels[i].FontAttributes = FontAttributes.None;//refresh the font first.
                labels[i].GestureRecognizers.Clear();

                lastMonth--;
            }

            //update calendar for the current month.
            for (int i = startDay; i < startDay + maxDay; i++)
            {
                labels[i].Text = j + "";
                labels[i].FontSize = 15;

                labels[i].TextColor = Color.FromHex("#000000");
                labels[i].FontAttributes = FontAttributes.None;//refresh the font first.
                labels[i].GestureRecognizers.Clear();


                // make the label bold if there are images in that day.
                foreach (string date in GooglePhotoService.allDates)
                {
                    DateTime currentDate = DateTime.Parse(date);
                    int Year = currentDate.Year;
                    int Month = currentDate.Month;
                    int Day = currentDate.Day;

                    if (Year == year && Month == month && Day == j)
                    {
                        labels[i].FontAttributes = FontAttributes.Bold;
                        labels[i].TextColor = Color.FromHex("#f5cb42");
                        var OnLabelClicked = new TapGestureRecognizer();
                        OnLabelClicked.Tapped += async (s, e) => {
                            await Navigation.PushAsync(new PhotoDisplayPage(date));
                        };
                        labels[i].GestureRecognizers.Add(OnLabelClicked);

                    }
                }
                j++;
            }

            //update the calendar after the maxDay.
            int m = 1;
            for (int i = startDay + maxDay; i < 42; i++)
            {
                labels[i].Text = m + "";
                labels[i].FontSize = 15;

                labels[i].TextColor = Color.FromHex("#c2c6cc");
                labels[i].FontAttributes = FontAttributes.None;//refresh the font first.
                labels[i].GestureRecognizers.Clear();


                m++;
            }

        }
        private int increaseMonth(int month)
        {
            int newMonth = month + 1;
            if (newMonth > 12)
                newMonth = 1;
            return newMonth;
        }
        private int decreaseMonth(int month)
        {
            int newMonth = month - 1;
            if (newMonth < 1)
                newMonth = 12;
            return newMonth;
        }
        private void AddTapGestures()
        {
            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GreetingPage());
            };
            AboutMeButton.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += async (s, e) => {
                await Navigation.PushAsync(new ListViewPage());
            };
            ListViewButton.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
            };
            MyDayButton.GestureRecognizers.Add(tapGestureRecognizer3);

        }


    }
}