using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognition.Services.VoiceRecorder;
using Xamarin.Forms;

namespace VoiceRecognition.ViewModel
{
    public class RecorderModel : INotifyPropertyChanged
    {
        private readonly IRecorder Recorder;

        public RecorderModel()
        {
            Recorder = RecorderFactory.GetRecorder();
            _Status = "Starting";                       // TODO: Please check this line again for 
        }

        public RecorderModel(string recorderType = "DEFAULT")
        {
            Recorder = RecorderFactory.GetRecorder(recorderType);
        }

        public void DisplayAlert(string title, string message, string cancel)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(title, message, cancel);
            });
        }

        
        public async Task WaitAndRecord(int seconds = 1000)
        {
            //await Task.Delay(seconds);
            _ = Recorder.StartRecordingAsync();
            Status = "Recording";
            Console.WriteLine(Status);
        }

        public void StopRecording()
        {
            Recorder.StopRecordingAsync();
            Status = "Stopped";
        }

        private string _Status;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Status 
        { 
            get { return _Status; } 
            set 
            { 
                _Status = value;
                NotifyPropertyChanged();
            }
        }


    }
}
