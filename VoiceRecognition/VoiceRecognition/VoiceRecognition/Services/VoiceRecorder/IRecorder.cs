using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VoiceRecognition.Services.VoiceRecorder
{
    interface IRecorder
    {
        bool IsRecording();
        Task StartRecordingAsync();
        Task StopRecordingAsync();
    }
}
