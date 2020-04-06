using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Plugin.AudioRecorder;

namespace VoiceRecognition.Services.VoiceRecorder
{
    public class DefaultContreteRecorder : IRecorder //, INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        private readonly AudioRecorderService RecorderClient;

        //private string _state;

        public  DefaultContreteRecorder()
        {
            RecorderClient = new AudioRecorderService()
            {
                StopRecordingOnSilence = true, // will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = false,  // stop recording after a max timeout (defined below)
                PreferredSampleRate = 16000, // sample rate of recording
                SilenceThreshold = 0.1f
            };
        }

        public bool IsRecording()
        {
            return RecorderClient.IsRecording;
        }


        public async Task StartRecordingAsync()
        {
            RecorderClient.StartRecording();
        }

        public async Task StopRecordingAsync()
        {
            _ = RecorderClient.StopRecording();
        }

        //public void Notify([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
