using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using VoiceRecognition.View;

namespace VoiceRecognition
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            //MainPage = new PeoplePage();
            MainPage = new NavigationPage(new EnrollmentPage());
            //MainPage = new ProfilePageMaster();
            //MainPage = new ProfielPage();
            //MainPage = new ProfielPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
