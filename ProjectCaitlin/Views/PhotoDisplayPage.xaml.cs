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

namespace ProjectCaitlin
{
    public partial class PhotoDisplayPage : ContentPage
    {
        readonly PhotoViewModel pageModel;

        GooglePhotoService GooglePhotoService = new GooglePhotoService();

        List<string> photoURIs = new List<string>();

        string date;

        public PhotoDisplayPage(CachedImage webImage,string date, string description)
        {
            InitializeComponent();
            this.date = date;
            pageModel = new PhotoViewModel(webImage,date,description);
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
            this.date = date;
            pageModel = new PhotoViewModel(date);
            BindingContext = pageModel;

            dateLabel.Text = date;
            var button1 = this.FindByName<ImageButton>("button1");
            button1.Clicked += ButtonOne;

            var button2 = this.FindByName<ImageButton>("button2");
            button2.Clicked += ButtonTwo;
        }

        async void ButtonOne(object sender, EventArgs args)
        {
            date = PreviousDate();
            await Navigation.PushAsync(new PhotoDisplayPage(date));
        }

        async void ButtonTwo(object sender, EventArgs args)
        {
            date = NextDate();
            await Navigation.PushAsync(new PhotoDisplayPage(date));
        }

        public string PreviousDate() {

            string currentDate = date;
            int Year = Int32.Parse(currentDate.Substring(0, currentDate.IndexOf("/")));
            string newDate = currentDate.Substring(currentDate.IndexOf("/") + 1);
            int Month = Int32.Parse(newDate.Substring(0, newDate.IndexOf("/")));
            newDate = newDate.Substring(newDate.IndexOf("/") + 1);
            int Day = Int32.Parse(newDate);

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
            dateTime = dateTime.Substring(0,dateTime.IndexOf(" "));
            return dateTime;
        }
        public string NextDate()
        {
            string currentDate = date;
            int Year = Int32.Parse(currentDate.Substring(0, currentDate.IndexOf("/")));
            string newDate = currentDate.Substring(currentDate.IndexOf("/") + 1);
            int Month = Int32.Parse(newDate.Substring(0, newDate.IndexOf("/")));
            newDate = newDate.Substring(newDate.IndexOf("/") + 1);
            int Day = Int32.Parse(newDate);


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
    }
}
