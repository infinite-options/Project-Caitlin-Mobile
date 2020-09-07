using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ProjectCaitlin.Views
{
    public partial class AboutMe : ContentPage
    {
        public AboutMe()
        {
            InitializeComponent();
        }

        private void OnTodaysList_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
