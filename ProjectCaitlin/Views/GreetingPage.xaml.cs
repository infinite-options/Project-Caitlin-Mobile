using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectCaitlin.ViewModel;
using ProjectCaitlin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.AudioRecorder;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GreetingPage : ContentPage
    {

        public GreetingViewModel greetingViewModel;

        TimeSpan morningStart = new TimeSpan(6, 0, 0);
        TimeSpan morningEnd = new TimeSpan(11, 0, 0);
        TimeSpan afternoonStart = new TimeSpan(11, 0, 0);
        TimeSpan afternoonEnd = new TimeSpan(18, 0, 0);
        TimeSpan eveningStart = new TimeSpan(18, 0, 0);
        TimeSpan eveningEnd = new TimeSpan(23, 59, 59);
        TimeSpan nightStart = new TimeSpan(0, 0, 0);
        TimeSpan nightEnd = new TimeSpan(6, 0, 0);
        AudioRecorderService recorder;

        public GreetingPage()
        {

            InitializeComponent();
            greetingViewModel = new GreetingViewModel(this);
            BindingContext = greetingViewModel;

            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#E9E8E8");
            SetupUI();

            recorder = new AudioRecorderService
            {
                StopRecordingOnSilence = true, //will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = true,  //stop recording after a max timeout (defined below)
                TotalAudioTimeout = TimeSpan.FromSeconds(10) //audio will stop recording after 15 seconds
            };

        }
        private void SetupUI()
        {
            UserImage.Source = App.User.aboutMe.pic;
            GreetingsTitleLabel.Text = GetTitleDayMessage();
            FirstNameLabel.Text = App.User.firstName;
            MessageCardLabel.Text = App.User.aboutMe.message_card;
            MessageLabel.Text = App.User.aboutMe.message_day;

            if (App.User.people.Count == 0)
            {
                importantPeopleSL.IsVisible = false;
            }

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
            Application.Current.Properties.Remove("access_token");
            Application.Current.Properties.Remove("refreshToken");
            Application.Current.Properties.Remove("user_id");

            await Navigation.PushAsync(new LoginPage());
        }

        async void RecordButton_Click(object sender, EventArgs e)
        {
            await RecordAudio();
        }

        async Task RecordAudio()
        {
            // Recorder works in Android, but not in iOS. Some permissions need to be set. 

            /*try
            {
                var recordTask = await recorder.StartRecording();

                var audioFile = await recordTask;

                if (audioFile != null) //non-null audioFile indicates audio was successfully recorded
                {
                    //do something with the file
                }
                else {
                    Console.WriteLine("Audio File : " + audioFile);
                }
            }
            catch (Exception ex)
            {

            }*/

        }


    }
}
