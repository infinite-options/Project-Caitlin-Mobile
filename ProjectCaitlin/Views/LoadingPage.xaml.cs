using System;
using System.Collections.Generic;
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class LoadingPage : ContentPage
    {
        private FirestoreService firestoreService;
        private FirebaseFunctionsService firebaseFunctionsService;
        private GoogleService googleService;

        public LoadingPage()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            if (Application.Current.Properties.ContainsKey("access_token")
                && Application.Current.Properties.ContainsKey("refreshToken")
                && Application.Current.Properties.ContainsKey("user_id"))
            {
                App.LoadApplicationProperties();

                firestoreService = new FirestoreService();
                firebaseFunctionsService = new FirebaseFunctionsService();
                googleService = new GoogleService();

                if (await googleService.RefreshToken())
                {
                    await firestoreService.LoadUser();
                    await GoogleService.LoadTodaysEvents();
                    await Navigation.PushAsync(new GoalsRoutinesTemplate());
                }
            }
        }
    }
}
