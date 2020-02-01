using System;
using System.Collections.Generic;
using ProjectCaitlin.Models;
using ProjectCaitlin.Services;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class DailyPage : ContentPage
    {
        user user;

        public DailyPage()
        {
            InitializeComponent();
            BindingContext = this;
            user = App.user;
            setupUI();
        }

        public void setupUI()
        {
            Console.WriteLine("user.routines.Count: " + user.routines.Count);
            if (user.routines.Count > 0)
                routineTitle.Text = user.routines[0].title;
        }

        async void PhotosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhotoDisplayPage());
        }
    }
}
