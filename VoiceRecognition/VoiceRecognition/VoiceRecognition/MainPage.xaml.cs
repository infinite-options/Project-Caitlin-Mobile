using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using VoiceRecognition.Model;
using VoiceRecognition.Model.AzCognitiveSpeaker;
using VoiceRecognition.Services;
using VoiceRecognition.Services.AzCognitiveSpeaker;
using VoiceRecognition.ViewModel;
using System.Runtime.CompilerServices;

namespace VoiceRecognition
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {

        private event PropertyChangedEventHandler MainPropertyChanged;
        private readonly IdentityProfileClient SpeakerIdentityProfileClient;

        private static string _PageTitle = "Tama";
        public string PageTitle
        {
            get { return _PageTitle; }
            set
            {
                _PageTitle = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
           MainPropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = env = new EnrollVoice();
            SpeakerIdentityProfileClient = new IdentityProfileClient();
        }

        private void Create_Identity(object sender, EventArgs e)
        {
            Profile profile = SpeakerIdentityProfileClient.CreateProfileAsync().Result;
            DisplayAlert("Profile Created", profile.IdentificationProfileId, "OK");
        }

        private void Get_All_Profiles(object sender, EventArgs e)
        {
            List<Profile> profiles = SpeakerIdentityProfileClient.GetProfilesAsync().Result;
            DisplayAlert("Fetched Profiles", "All profiles Done", "OK");
        }

        private void Get_Profile(object sender, EventArgs e)
        {
            string testprofile = "606f3bcb-1c6b-4507-8a7f-2c70aece8f70";
            Profile profile = SpeakerIdentityProfileClient.GetProfileAsync(testprofile).Result;
            DisplayAlert(testprofile, profile.EnrollmentStatus.ToString(), "OK");
        }

        private void RecordButton_Click(object sender, EventArgs e)
        {
            //RecorderModel rm = new RecorderModel();
            //DisplayAlert("Recording Status", rm.Status, "OK");
            //_ = rm.WaitAndRecord(0);
            //DisplayAlert("New Recording Status", rm.Status, "OK");
            //PageTitle = rm.Status;
            //PageTitle = rm.Status;
            //rm.StopRecording();
            //testEnroll();
            _ = env.StartRecordingAsync();
            RecordingStatus.Text = env.Message;
            //DisplayAlert("New Recording Status", rm.Status, "OK");
            //DisplayAlert("Recording Status", env.Message, "OK");
        }

        private void StopRecordButton_Clicked(object sender, EventArgs e)
        {
            _ = env.StopRecordingAsync();
            RecordingStatus.Text = env.Message;
        }

        //private void testEnroll()
        //{
        //    //env = new EnrollVoice();
        //    _ = env.StartRecordingAsync();
        //}

        private EnrollVoice env;


        public void DoNothing(object sender, EventArgs e)
        {
            Task.Delay(5000);
            DisplayAlert("Recording Status", env.Message, "OK");
        }
    }
}
