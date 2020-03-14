using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace VoicePay.Views.Authentication
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new ShellPage();
        }
    }
}
