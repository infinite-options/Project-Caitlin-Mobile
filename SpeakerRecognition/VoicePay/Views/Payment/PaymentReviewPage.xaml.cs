using System;
using System.Collections.Generic;
using VoicePay.Helpers;
using VoicePay.ViewModels;
using VoicePay.Views.Enrollment;
using Xamarin.Forms;

namespace VoicePay.Views.Payment
{
    public partial class PaymentReviewPage : ContentPage
    {
        public PaymentReviewPage()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            Page navigateToPage;
            if (Settings.Instance.Contains(nameof(Settings.UserIdentificationId)))
                navigateToPage = new AudioVerifyPage();
            else
                navigateToPage = new WelcomePage();

            await BaseViewModel.MasterDetail.Detail.Navigation.PushAsync(navigateToPage);
        }
    }
}
