using System;
using System.Collections.Generic;
using ProjectCaitlin.Methods;

using Xamarin.Forms;

namespace ProjectCaitlin.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            var FSMethods = new FirestoreMethods("7R6hAVmDrNutRkG3sVRy");
            await FSMethods.LoadUser();

            if (App.user.access_token == "")
            {
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
            }
        }
    }
}
