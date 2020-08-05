using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using VoiceRecognition.Model.AzCognitiveSpeaker;
using VoiceRecognition.Services.AzCognitiveSpeaker;
using VoiceRecognition.Services.Firebase;

namespace ProjectCaitlin.Services
{

    public class SpeakerRecognition
    {

        public static SpeakerRecognition Instance { get { return Nested.speakerRecognition; } }
        private SpeakerRecognition(){}

        private class Nested
        {
            static Nested() { }

            internal static readonly SpeakerRecognition speakerRecognition = new SpeakerRecognition();
        }

        private RecorderWrapper recorderWrapper = RecorderWrapper.Instance;
        private AudioRecorderService RecorderClient = RecorderWrapper.Instance.audioRecorderService;

        private static readonly Profile UNKNOWN_PROFILE = new Profile { IdentificationProfileId = "00000000-0000-0000-0000-000000000000" };
        private PeopleClient PeopleService = new PeopleClient(App.User.id);
        readonly Stopwatch sw;

        private readonly IdentityProfileClient IdClient;
        private static List<Profile> _AzProfiles = new List<Profile>();

        private Boolean _autoMode = false;
        public Boolean autoMode {
            get { return _autoMode; }
            set {
                if (_autoMode != value) {
                    _autoMode = value;
                    //if (value) startAutoMode();
                }
            }
        }

        // TO-DO Complete AutoMode
        //public void startAutoMode()
        //{
        //    Task.Factory.StartNew(() =>
        //    {

        //    });
        //}



        //public void CMDIdentifyAndEnroll()
        //{
        //    RecorderClient.StopRecordingOnSilence = true; // will stop recording after 2 seconds (default)
        //    CMDStopRecording();
        //    RecorderClient.AudioInputReceived += EnrollIdentifyWrapper;
        //    _ = RecorderClient.StartRecording();
        //}

        //public void CMDManualIdentifyAndEnroll()
        //{
        //    RecorderClient.StopRecordingOnSilence = false; // will stop recording after 2 seconds (default)

        //    RecorderClient.AudioInputReceived += EnrollIdentifyWrapper;
        //    _ = RecorderClient.StartRecording();
        //}

//#pragma warning disable CS1998
//        public async void CMDStopRecording()
//#pragma warning restore CS1998
//        {
//            if (IsRecording())
//            {
//                _ = RecorderClient.StopRecording();
//            }
//        }


    }
}
