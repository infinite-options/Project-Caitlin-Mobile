using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Plugin.AudioRecorder;

using VoiceRecognition.Services.AzCognitiveSpeaker;
using Xamarin.Forms;

namespace VoiceRecognition.ViewModel
{
    public class EnrollVoice: ViewModelBase
    {
        private readonly AudioRecorderService RecorderClient;
        private string _Message = "No Yet";
        public string Message { get { return _Message; } set{ _Message = value;  NotifyPropertyChanged("Message"); } }
        private IdentityProfileClient IdClient;

        public EnrollVoice()
        {

            IdClient = new IdentityProfileClient();

            RecorderClient = new AudioRecorderService()
            {
                StopRecordingOnSilence = true, // will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = false,  // stop recording after a max timeout (defined below)
                PreferredSampleRate = 16000, // sample rate of recording
                SilenceThreshold = 0.1f
            };

            RecorderClient.AudioInputReceived += async (object sender, string e) => { await AudioInputReceived(sender, e); };

            Commands.Add("StartRecording", new Command(StartRecording));
            Commands.Add("StopRecording", new Command(StopRecording));
        }

        private async Task AudioInputReceived(object sender, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
            {
                Message = "Try speaking Louder";
                await StartRecordingAsync();
                return;
            }

            try
            {
                var profileTask = IdClient.CreateProfileAsync();
                profileTask.Wait();
                var resultTask = IdClient.EnrollAsync(profileTask.Result, audioFilePath);
                resultTask.Wait();
                var result = resultTask.Result;
                Message = "Enrolled";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public async Task StartRecordingAsync()
        {
            _ = RecorderClient.StartRecording();
            Message = "Started";
        }

        public async Task StopRecordingAsync()
        {
            _ = RecorderClient.StopRecording();
        }

        public void StartRecording()
        {
            RecorderClient.StartRecording().Wait();
            Message = "Started";
        }

        public void StopRecording()
        {
            RecorderClient.StopRecording().Wait();
            Message = "Stopped";
        }
    }
}
