using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class DailyViewPage : ContentPage
    {
        public DailyViewPage()
        {
            InitializeComponent();
        }

        public async void MonthlyBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonthlyViewPage());
        }

        public async void ListBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListViewPage());
        }

        public void ReLoginClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}
