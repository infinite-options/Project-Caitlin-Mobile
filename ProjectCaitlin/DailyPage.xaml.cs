using System;
using System.Collections.Generic;
using ProjectCaitlin.Models;
using ProjectCaitlin.Services;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class DailyPage : ContentPage
    {

        public DailyPage()
        {
            InitializeComponent();
            BindingContext = this;
            setupUI();
        }

        public void setupUI()
        {
            Console.WriteLine("user.routines.Count: " + LoginPage.user.routines.Count);
            if (LoginPage.user.routines.Count > 0)
                routineTitle.Text = LoginPage.user.routines[0].title;
        }

        async void PhotosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhotoDisplayPage());
        }
    }
}
