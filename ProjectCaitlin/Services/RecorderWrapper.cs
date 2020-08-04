using System;
using Plugin.AudioRecorder;

namespace ProjectCaitlin.Services
{
    public class RecorderWrapper
    {
        public static RecorderWrapper Instance { get { return Nested.instance; } }

        private RecorderWrapper()
        {
        }

        private static AudioRecorderService _audioRecorderService = new AudioRecorderService
        {
            StopRecordingOnSilence = true, // will stop recording after 2 seconds (default)
            StopRecordingAfterTimeout = true,  // stop recording after a max timeout (defined below)
            TotalAudioTimeout = TimeSpan.FromSeconds(31), // will stop recording after 31 second
            PreferredSampleRate = 16000, // sample rate of recording
            SilenceThreshold = 0.1f
        };

        public AudioRecorderService audioRecorderService { get { return _audioRecorderService; } }

        private static Boolean _autoMode = false;
        public Boolean autoMode { get { return _autoMode; } set { if (value != _autoMode) _autoMode = value; } }


        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly RecorderWrapper instance = new RecorderWrapper();
        }
    }
}
