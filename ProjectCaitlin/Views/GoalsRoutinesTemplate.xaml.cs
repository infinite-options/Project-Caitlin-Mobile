using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectCaitlin.ViewModel;

namespace ProjectCaitlin.Views
{
    public partial class GoalsRoutinesTemplate : ContentPage
    {
        readonly GoalsRoutinesTemplateViewModel pageModel;

        public GoalsRoutinesTemplate()
        { 
            InitializeComponent();
            pageModel = new GoalsRoutinesTemplateViewModel(this);
            BindingContext = pageModel;

        }

        public async void btn1(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GreetingPage());
        }
        public async void btn2(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new ListViewPage());
        }
        public async void btn3(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new MonthlyViewPage());
        }
        public async void btn4(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());
        }






    }
}