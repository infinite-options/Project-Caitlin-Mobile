using System;
using System.Collections.Generic;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
using Xamarin.Forms;
using ProjectCaitlin.ViewModel;
using System.Globalization;
using System.Text.RegularExpressions;

using ProjectCaitlin.Models;

namespace ProjectCaitlin
{
    public partial class PhotoDisplayPage : ContentPage
    {
        readonly PhotoViewModel pageModel;

        string date;
        string description;
        string id;

        bool disableBackButton;

        public PhotoDisplayPage(CachedImage webImage, string date, string description, string id, string creationTime, string note)
        {
            InitializeComponent();
            AddTapGestures();
            this.date = date;
            this.description = description;
            this.id = id;
            disableBackButton = false;
            pageModel = new PhotoViewModel(webImage, date, description, creationTime,note);
            BindingContext = pageModel;

            dateLabel.Text = date;
            var button1 = this.FindByName<ImageButton>("button1");
            button1.Clicked += ButtonOne;

            var button2 = this.FindByName<ImageButton>("button2");
            button2.Clicked += ButtonTwo;

        }

        public PhotoDisplayPage(string date)
        {
            InitializeComponent();
            AddTapGestures();
            this.date = date;
            pageModel = new PhotoViewModel(date);
            
            BindingContext = pageModel;

            dateLabel.Text = date;
            disableBackButton = false;
            var button1 = this.FindByName<ImageButton>("button1");
            button1.Clicked += ButtonOne;

            var button2 = this.FindByName<ImageButton>("button2");
            button2.Clicked += ButtonTwo;
        }

        async void ButtonOne(object sender, EventArgs args)
        {
            date = PreviousDate();
            //pageModel.SetupUI(date);
            await Navigation.PushAsync(new PhotoDisplayPage(date));
        }

        async void ButtonTwo(object sender, EventArgs args)
        {
            date = NextDate();
            await Navigation.PushAsync(new PhotoDisplayPage(date));
        }

        public string PreviousDate()
        {

            DateTime currentDate = DateTime.Parse(date);
            int Year = currentDate.Year;
            int Month = currentDate.Month;
            int Day = currentDate.Day;

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

            if (Day - 1 != 0)
            {
                Day = Day - 1;
            }
            else
            {
                Day = lastMonth;
                Month -= 1;
                if (Month == 0)
                {
                    Month = 12;
                    Year -= 1;
                }
            }

            string dateTime = new DateTime(Year, Month, Day) + "";
            dateTime = dateTime.Substring(0, dateTime.IndexOf(" "));
            return dateTime;
        }
        public string NextDate()
        {
            DateTime currentDate = DateTime.Parse(date);
            int Year = currentDate.Year;
            int Month = currentDate.Month;
            int Day = currentDate.Day;


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

            if (Day + 1 <= maxDay)
            {
                Day = Day + 1;
            }
            else
            {
                Day = 1;
                Month += 1;
                if (Month == 13)
                {
                    Month = 1;
                    Year += 1;
                }
            }

            string dateTime = new DateTime(Year, Month, Day) + "";
            dateTime = dateTime.Substring(0, dateTime.IndexOf(" "));
            return dateTime;
        }
        /*async void EditorCompleted(object sender, EventArgs e)
        {
            var text = ((Editor)sender).Text; // sender is cast to an Editor to enable reading the `Text` property of the view.
            Console.WriteLine("Doing something" + carousel.CurrentItem.ToString());

            //Painful process to get the source of the photo on the carousel view.
            string pain = carousel.CurrentItem.ToString();
            int startIndex = pain.IndexOf("=") + 2;
            int endIndex = pain.IndexOf(", ");
            string photo_source = pain.Substring(startIndex, endIndex-startIndex);

            //evil code, don't change anything in this if statement.
            if(photo_source.Substring(0,5)=="Uri: ")
                photo_source = "" + photo_source.Substring(5);

            Console.Write("pain" + pain);
            Console.Write("photo_source" + photo_source);
            foreach (List<string> list in App.User.photoURIs)
            {
                if (photo_source == list[0])
                {
                    list[2] = text;
                    await FirebaseFunctionsService.PostPhoto(list[4], text, list[5]);
                    Console.WriteLine(list[4] + "Description : " + text);
                }
            }
            disableBackButton = true;
        }*/

        async void EditorCompleted2(object sender, EventArgs e)
        {
            var text = ((Editor)sender).Text; // sender is cast to an Editor to enable reading the `Text` property of the view.

            //Painful process to get the source of the photo on the carousel view.
            string pain = carousel.CurrentItem.ToString();
            int startIndex = pain.IndexOf("=") + 2;
            int endIndex = pain.IndexOf(", ");
            string photo_source = pain.Substring(startIndex, endIndex - startIndex);

            //evil code, don't change anything in this if statement.
            if (photo_source.Substring(0, 5) == "Uri: ")
                photo_source = "" + photo_source.Substring(5);

            Console.Write("pain" + pain);
            Console.Write("photo_source" + photo_source);
            foreach (List<string> list in App.User.photoURIs)
            {
                if (photo_source == list[0])
                {
                    list[5] = text;
                    await FirebaseFunctionsService.PostPhoto(list[4], list[2], text);
                    Console.WriteLine(list[4] + "Description : " + text);
                }
            }
            disableBackButton = true;
        }


        protected override bool OnBackButtonPressed()
        {
            if (disableBackButton)
            {
                Navigation.PushAsync(new MonthlyViewPage());
                return true;
            }
            else
                return base.OnBackButtonPressed();
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
                await Navigation.PushAsync(new MonthlyViewPage());
            };
            MyPhotosButton.GestureRecognizers.Add(tapGestureRecognizer3);

            var tapGestureRecognizer4 = new TapGestureRecognizer();
            tapGestureRecognizer4.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
            };
            MyDayButton.GestureRecognizers.Add(tapGestureRecognizer4);
        }
    }
}