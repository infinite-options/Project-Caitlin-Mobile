using System;
namespace SpeakerRecognitionAPI.Models
{
    public class IdentificationIdentification
    {
        public Status Status { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastActionDateTime { get; set; }
        public ProcessingResult ProcessingResult { get; set; }
        public string Message { get; set; }
    }

    public class IdentificationResult
    {
        public string IdentificationProfileId { get; set; }
        public string Locale { get; set; }
        public double EnrollmentSpeechTime { get; set; }
        public double RemainingEnrollmentSpeechTime { get; set; }
        public string CreatedDateTime { get; set; }
        public string LastActionDateTime { get; set; }
        public string EnrollmentStatus { get; set; }
    }

    //public enum Status
    //{
    //    NotStarted,
    //    Running,
    //    Failed,
    //    Succeeded
    //}

    //public enum Confidence
    //{
    //    Low,
    //    Normal,
    //    High
    //}
}
