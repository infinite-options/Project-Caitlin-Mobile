﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using ProjectCaitlin.ViewModel;
using ProjectCaitlin.Services;
using Acr.UserDialogs;
namespace ProjectCaitlin.Views
{
    public partial class GoalsRoutinesTemplate : ContentPage
    {
        readonly GoalsRoutinesTemplateViewModel pageModel;

        FirestoreService firestoreService;
        GoogleService googleService = new GoogleService();

        public GoalsRoutinesTemplate()
        {
            InitializeComponent();
            AddTapGestures();
            App.ParentPage = "MyDay";
            pageModel = new GoalsRoutinesTemplateViewModel(this);
            BindingContext = pageModel;
            ContentStackLayout.HeightRequest = Application.Current.MainPage.Height - NavBar.Height;
            firestoreService = new FirestoreService();
        }

        private void AddTapGestures()
        {
            // for nav bar
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GreetingPage());
            };
            AboutMeButton.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += async (s, e) => {
                await Navigation.PushAsync(new ListViewPage());
            };
            ListViewButton.GestureRecognizers.Add(tapGestureRecognizer2);
            
            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += async (s, e) => {
                UserDialogs.Instance.ShowLoading("Loading...");
                if (App.User.photoURIs.Count < 1)
                    await GooglePhotoService.GetPhotos();
                await Navigation.PushAsync(new MonthlyViewPage());
                UserDialogs.Instance.HideLoading();
            };
            MyPhotosButton.GestureRecognizers.Add(tapGestureRecognizer3);


            var tapGestureRecognizer4 = new TapGestureRecognizer();
            tapGestureRecognizer4.Tapped += async (s, e) => {
                UserDialogs.Instance.ShowLoading("Refreshing Page...");
                await firestoreService.LoadDatabase();
                await googleService.LoadTodaysEvents();
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
                UserDialogs.Instance.HideLoading();
            };
            MyDayButton.GestureRecognizers.Add(tapGestureRecognizer4);
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        }

    }
}
