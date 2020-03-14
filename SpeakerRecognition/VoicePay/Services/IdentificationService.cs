using System;
using SpeakerRecognitionAPI;

namespace VoicePay.Services
{
    public class IdentificationService
    {
        static readonly SpeakerIdentificationClient _instance = new SpeakerIdentificationClient(Constants.Keys.SpeakerRecognitionApiKey);
        public static SpeakerIdentificationClient Instance => _instance ;
    }
}
