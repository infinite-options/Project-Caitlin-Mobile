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
            pageModel = new GoalsRoutinesTemplateViewModel(this);
            BindingContext = pageModel;
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
                await Navigation.PushAsync(new MonthlyViewPage());
            };
            MyPhotosButton.GestureRecognizers.Add(tapGestureRecognizer3);
        }
    }
}
