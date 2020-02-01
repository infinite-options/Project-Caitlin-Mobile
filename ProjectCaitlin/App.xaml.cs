using System;
using ProjectCaitlin.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin
{
    public partial class App : Application
    {
        public static user user { get; set; } = new user();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

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
