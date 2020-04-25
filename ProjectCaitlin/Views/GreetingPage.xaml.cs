using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GreetingPage : ContentPage
    {
        TimeSpan morningStart = new TimeSpan(6, 0, 0);
        TimeSpan morningEnd = new TimeSpan(11, 0, 0);
        TimeSpan afternoonStart = new TimeSpan(11, 0, 0);
        TimeSpan afternoonEnd = new TimeSpan(18, 0, 0);
        TimeSpan eveningStart = new TimeSpan(18, 0, 0);
        TimeSpan eveningEnd = new TimeSpan(23, 59, 59);
        TimeSpan nightStart = new TimeSpan(0, 0, 0);
        TimeSpan nightEnd = new TimeSpan(6, 0, 0);
        public GreetingPage()
        {
            
            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#E9E8E8");
            SetupUI();
        }
        private void SetupUI()
        {
            UserImage.Source = App.User.Me.pic;
            GreetingsTitleLabel.Text = GetTitleDayMessage();
            MessageCardLabel.Text = App.User.Me.message_card;
            MessageLabel.Text = App.User.Me.message_day;
        }

        public String GetTitleDayMessage()
        {
            var completeTime = DateTime.Now.TimeOfDay;
            if (morningStart <= completeTime && completeTime < morningEnd)
            {
                //Console.WriteLine("Morning");

                return "Good Morning";
            }
            if (afternoonStart <= completeTime && completeTime < afternoonEnd)
            {
                //Console.WriteLine("Afternoon");

                return "Good Afternoon";
            }
            if (eveningStart <= completeTime && completeTime < eveningEnd)
            {
                //Console.WriteLine("Evening");

                return "Good Evening";
            }
            if (nightStart <= completeTime && completeTime <= nightEnd)
            {
                //Console.WriteLine("Night");

                return "Good Night";
            }

            return "";
        }
        async void btn1Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new ListViewPage());

        }

        async void btn2Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());

        }

        async void LogoutBtnClick(object sender, EventArgs e)
        {
            Application.Current.Properties.Remove("accessToken");
            Application.Current.Properties.Remove("refreshToken");
            Application.Current.Properties.Remove("user_id");

            await Navigation.PushAsync(new LoginPage());
        }
    }
}