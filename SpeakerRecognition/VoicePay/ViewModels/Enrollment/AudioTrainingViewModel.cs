using System;
using System.Threading.Tasks;
using SpeakerRecognitionAPI.Helpers;
using SpeakerRecognitionAPI.Interfaces;
using VoicePay.Helpers;
using VoicePay.Services;
using VoicePay.Services.Interfaces;
using Xamarin.Forms;

namespace VoicePay.ViewModels.Enrollment
{
    public class AudioTrainingViewModel : AudioRecordingBaseViewModel
    {
        private readonly IBeepPlayer _beeper;
        //private readonly ISpeakerVerification _verificationService;
        private readonly ISpeakerIdentification _verificationService;
        private string PhraseMessage => $"\"{EnrollmentProcess.SelectedPhrase}\"";

        private bool _isCompleted;
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set { _isCompleted = value; RaisePropertyChanged(); }
        }

        //public AudioTrainingViewModel() : this(VerificationService.Instance) { }
        public AudioTrainingViewModel() : this(IdentificationService.Instance) { }
        //public AudioTrainingViewModel(ISpeakerVerification verificationService)
        public AudioTrainingViewModel(ISpeakerIdentification verificationService)
        {
            StateMessage = "Hold on...";

            _beeper = DependencyService.Get<IBeepPlayer>();
            _verificationService = verificationService;

            Recorder.AudioInputReceived += async (object sender, string e) => { await Recorder_AudioInputReceived(sender, e); };
        }


        private async Task Recorder_AudioInputReceived(object sender, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
            {
                StateMessage = "We can't hear you :/";
                Message = "Try speaking louder";
                await WaitAndStartRecording();
                return;
            }

            IsBusy = true;

            StateMessage = "Analyzing...";
            Message = string.Empty;

            try
            {
                //var enrollmentResult = await VerificationService.Instance.EnrollAsync(audioFilePath, Settings.UserIdentificationId);
                var enrollmentResult = await IdentificationService.Instance.EnrollAsync(audioFilePath, Settings.UserIdentificationId);
                await Task.Delay(3000);
                string idResult = String.Format("Your ID Number Is {0}", Settings.UserIdentificationId);
                //var enrollmentResponse = await IdentificationService.Instance.CheckEnrollmentStatusAsync(enrollmentResult);
                //var enrollmentResult = await IdentificationService.Instance.EnrollAsync(audioFilePath, "7267ebc4-5996-40b3-89f5-bfad02d5c465");
                //var checkEnrollment = await IdentificationService.Instance.CheckEnrollmentStatusAsync(enrollmentResult);


                System.Diagnostics.Debug.WriteLine(idResult);
                //System.Diagnostics.Debug.WriteLine(enrollmentResult);
                //System.Diagnostics.Debug.WriteLine(enrollmentResponse);
                //System.Diagnostics.Debug.WriteLine(enrollmentResult);
                //System.Diagnostics.Debug.WriteLine(checkEnrollment);


                //EnrollmentProcess.SelectedPhrase = enrollmentResult.Phrase;

                //if (enrollmentResult.RemainingEnrollments > 0)
                if (enrollmentResult == null)
                {
                    await Recorder.StopRecording();
                    await Application.Current.MainPage.Navigation.PushModalAsync(new Views.Enrollment.MainPage());
                }
                else
                {
                    IsCompleted = true;
                    StateMessage = "Good! We've just finished.";
                    //await Task.Delay(2000);
                    //Application.Current.MainPage = new Views.Enrollment.InputName();
                    //Settings.EnrolledPhrase = EnrollmentProcess.SelectedPhrase;
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new Views.Enrollment.InputName());
                    //DisplayAlert("Success!", nameMSG, "OK");
                });
            }
            catch (SpeakerRecognitionException ex)
            {
                if (ex.DetailedError.Message.Equals("InvalidPhrase", StringComparison.OrdinalIgnoreCase))
                {
                    StateMessage = "¡Oops! That was an invalid phrase.";
                    Message = "Let's try again...";
                    await WaitAndStartRecording();
                }
            }
            //catch
            //{
            //    StateMessage = "¡Oops! Something happened";
            //    Message = "Let's try again...";
            //    await WaitAndStartRecording();
            //}
            finally
            {
                IsBusy = false;
            }
        }

        public override async Task StartRecording()
        {
            await Recorder.StartRecording();
            _beeper.Beep();
            StateMessage = "Listening...";
            //Message = PhraseMessage;
        }

    }
}
