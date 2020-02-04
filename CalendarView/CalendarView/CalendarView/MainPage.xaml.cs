using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CalendarView
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 1;
        Label[] labels = new Label[42];
        Image[] images = new Image[42];
        public MainPage()
        {
            InitializeComponent();

            int row = 1;
            int col = 0;
            //Add 42 cells in grid.
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
                    row++;
                    col = 0;
                }
                
                StackLayoutMap.Children.Add(labels[i]);
            }

            int row2 = 2;
            for (int i = 0; i < 42; i++)
            {
                images[i] = new Image();
                images[i].Source = "bear.jpg";

                Grid.SetRow(images[i], row2);
                Grid.SetColumn(images[i], col);

                col++;
                if (col == 7)
                {
                    row2++;
                    row2++;
                    col = 0;
                }

                StackLayoutMap.Children.Add(images[i]);
            }


            var button1 = this.FindByName<Button>("btn1");
            button1.Clicked += ButtonOne;

            var button2 = this.FindByName<Button>("btn2");
            button2.Clicked += ButtonTwo;

            DateTime localDate = DateTime.Now;
            // Uses the default calendar of the InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            var currentYear = myCal.GetYear(localDate);
            var currentMonth = myCal.GetMonth(localDate);
            var currentDay = myCal.GetDayOfWeek(localDate);

            Year = currentYear; // It will be shown at your label
            Month = currentMonth; // It will be shown at your label
            yearLabel.Text = Year + "";
            setMonthLabel(Month);
            SetCalendar(currentYear, currentMonth);
        }
        void setMonthLabel(int month) {
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
            SetCalendar(Year,Month);
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
            Console.WriteLine(currentDay);
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

            else if (Month == 5){
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

            for (int i = startDay - 1; i >= 0; i--) {
                labels[i].Text = lastMonth + "";
                lastMonth--;
            }
            for (int i = startDay; i < startDay + maxDay; i++) {
                labels[i].Text = j + "";
                j++;
            }
            int m = 1;
            for (int i = startDay + maxDay; i < 42; i++)
            {
                labels[i].Text = m + "";
                m++;
            }
        }
    }
}
