using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VoicePay.ViewModels;
using VoicePay.ViewModels.Enrollment;
using Xamarin.Forms;
using Plugin.AudioRecorder;


namespace VoicePay.Views.Enrollment
{
    public partial class AudioRecordingPage : ContentPage
    {
        AudioTrainingViewModel ViewModel => BindingContext as AudioTrainingViewModel;

        //AudioPlayer player;

        public AudioRecordingPage()
        {
            InitializeComponent();
            CompletedAnimated.PropertyChanged += CompletedAnimated_PropertyChanged;
        }

        protected override async void OnAppearing()
        {
            await Task.Delay(5000);
            await ViewModel.StartRecording();
        }

        async void CompletedAnimated_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsVisible"))
            {
                CompletedAnimated.Play();
                await Task.Delay(3000);
                //CompletedAnimated.Pause();
                //await BaseViewModel.MasterDetail.Detail.Navigation.PopToRootAsync(false);
            }
        }

        //  Prevent from going back
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
