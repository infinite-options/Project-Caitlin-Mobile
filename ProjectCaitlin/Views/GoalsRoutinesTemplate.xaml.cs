using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoalsRoutinesTemplate : ContentPage
    {


        public GoalsRoutinesTemplate()
        {


            InitializeComponent();
            ObservableCollection<Models.DailyRoutines> routines = new ObservableCollection<Models.DailyRoutines>
            { new Models.DailyRoutines
            {
                ImageUrl = "http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/23c1dd13-333a-459e-9e23-c3784e7cb434/2016-06-02_1049.png",
                Name = "Brush your teeth",
                Button = "btn1"
            },
                new Models.DailyRoutines
            {
                ImageUrl =    "http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/6b60d27e-c1ec-4fe6-bebe-7386d545bb62/2016-06-02_1051.png",
                Name = "Eat breakfast",
                Button = "btn2"
                },
            new Models.DailyRoutines
            {
                ImageUrl = "http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/e8179889-8189-4acb-bac5-812611199a03/2016-06-02_1053.png",
                Name = "Go to bed",
                Button = "btn3"
            }};
            carouselViewRoutines.ItemsSource = routines;



            DateTime localDate = DateTime.Now;
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            var currentDay = myCal.GetDayOfWeek(localDate);
            dayLabel.Text = currentDay + "";




        }

        public void ButtonEvent(object sender, EventArgs args)
        {
            var btn = (Button)sender;
            if (btn.Text == "Complete")
                btn.Text = "Done";
            else
                btn.Text = "Complete";


        }





    }
}