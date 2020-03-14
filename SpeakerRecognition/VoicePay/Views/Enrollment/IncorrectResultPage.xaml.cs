using System;
using System.Collections.Generic;
using VoicePay.ViewModels;
using Xamarin.Forms;

namespace VoicePay.Views.Enrollment
{
    public partial class IncorrectResultPage : ContentPage
    {
        public IncorrectResultPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {

            Application.Current.MainPage = new Views.Enrollment.MainPage();
            //await BaseViewModel.MasterDetail.Detail.Navigation.PopToRootAsync();
            //await BaseViewModel.MasterDetail.Detail.Navigation.PopModalAsync();
        }
    }
}
