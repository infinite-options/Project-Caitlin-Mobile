    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CreateTable
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        public int Year { get; set; } = 0;
        public int Month { get; set; } = 1;


        public MainPage()
        {
            InitializeComponent();

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
            monthLabel.Text = Month + "";
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
            monthLabel.Text = Month + "";
            Console.WriteLine("button1" + Month);
            //setCalendar(year,month);
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
            monthLabel.Text = Month + "";
            Console.WriteLine("button2" + Month);
            //setCalendar(year, month);
        }

      
        public static void SetCalendar(int year, int month)
        {
           
/*
            var layout = new StackLayout { Padding = new Thickness(5, 10) };
            var yearLabel = new Label { Text = "This is a green label.", TextColor = Color.FromHex("#77d065"), FontSize = 20 };
            layout.Children.Add(yearLabel);
            


            // Displays the values of the DateTime.

      */      

        }

        public static void DisplayValues(Calendar myCal, DateTime myDT)
        {
            Console.WriteLine("   Era:          {0}", myCal.GetEra(myDT));
            Console.WriteLine("   Year:         {0}", myCal.GetYear(myDT));
            Console.WriteLine("   Month:        {0}", myCal.GetMonth(myDT));
            Console.WriteLine("   DayOfYear:    {0}", myCal.GetDayOfYear(myDT));
            Console.WriteLine("   DayOfMonth:   {0}", myCal.GetDayOfMonth(myDT));
            Console.WriteLine("   DayOfWeek:    {0}", myCal.GetDayOfWeek(myDT));
            Console.WriteLine("   Hour:         {0}", myCal.GetHour(myDT));
            Console.WriteLine("   Minute:       {0}", myCal.GetMinute(myDT));
            Console.WriteLine("   Second:       {0}", myCal.GetSecond(myDT));
            Console.WriteLine("   Milliseconds: {0}", myCal.GetMilliseconds(myDT));
            Console.WriteLine();
        }


        public void Repaint()
        {
            var grid = new Grid();

            //12 row, 7 col
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(0.07692307692, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.14285714285, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.14285714285, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.14285714285, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.14285714285, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.14285714285, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.14285714285, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.14285714285, GridUnitType.Star) });

            var topLeft = new Label { Text = "i" };
            var topRight = new Label { Text = "Top Right" };
            var bottomLeft = new Label { Text = "Bottom Left" };
            var bottomRight = new Label { Text = "Bottom Right" };

            grid.Children.Add(topLeft, 0, 0);
            grid.Children.Add(topRight, 1, 0);
            grid.Children.Add(bottomLeft, 0, 1);
            grid.Children.Add(bottomRight, 1, 1);

        }


    }
}
