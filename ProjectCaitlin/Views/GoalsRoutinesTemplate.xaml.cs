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
            AddTapGestures();
            App.ParentPage = "MyDay";
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
            await Navigation.PushAsync(new PhotoDisplayPage());
        }

        private void AddTapGestures()
        {
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
                await Navigation.PushAsync(new PhotoDisplayPage());
            };
            MyPhotosButton.GestureRecognizers.Add(tapGestureRecognizer3);
        }
    }
}
