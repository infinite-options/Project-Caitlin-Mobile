using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Model.AzCognitiveSpeaker
{

    public class Profile
    {
        public string IdentificationProfileId { get; set; }

        public string Locale { get; set; }

        public float EnrollmentSpeechTime { get; set; }

        public float RemainingEnrollmentSpeechTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime LastActionDateTime { get; set; }

        public VoiceRecognition.Model.AzCognitiveSpeaker.EnrollmentStatus EnrollmentStatus { get; set; }

        public override string ToString()
        {
            return IdentificationProfileId;
        }

    }
}
