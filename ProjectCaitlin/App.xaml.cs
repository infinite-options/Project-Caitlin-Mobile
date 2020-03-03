﻿using System;
using ProjectCaitlin.Models;
using ProjectCaitlin.Methods;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using ProjectCaitlin.Views;

namespace ProjectCaitlin
{
    public partial class App : Application
    {
        public static user User { get; set; } = new user();

        public static string ParentPage { get; set; } = "";

        [assembly: XamlCompilation(XamlCompilationsOptions.Compile)]
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());

            //{
            //    BarBackgroundColor = Color.FromHex("#4682B4"),
            //    BarTextColor = Color.White
            //};

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
