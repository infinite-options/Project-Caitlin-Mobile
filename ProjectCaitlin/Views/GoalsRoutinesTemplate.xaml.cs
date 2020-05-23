using System;
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

        public GoalsRoutinesTemplate()
        {
            InitializeComponent();
            AddTapGestures();
            App.ParentPage = "MyDay";
            pageModel = new GoalsRoutinesTemplateViewModel(this);
            BindingContext = pageModel;
            ContentStackLayout.HeightRequest = Application.Current.MainPage.Height - NavBar.Height;
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
                if(App.User.photoURIs.Count < 1)
                await GooglePhotoService.GetPhotos();
                UserDialogs.Instance.ShowLoading("Loading...");
                await Navigation.PushAsync(new MonthlyViewPage());
                UserDialogs.Instance.HideLoading();
            };
            MyPhotosButton.GestureRecognizers.Add(tapGestureRecognizer3);
            // %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
        }

    }
}
