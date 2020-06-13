using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Model.AzCognitiveSpeaker
{
    /// <summary>
    /// The status of the operation.
    /// </summary>
    public enum Status
    {
        notstarted,
        running,
        failed,
        succeeded
    }

    public class ProcessingResult
    {
        /// <summary>
        /// The identified speaker identification profile id. 
        /// If this value is 00000000-0000-0000-0000-000000000000, it means there's no speaker identification profile identified and the audio file to be identified belongs to none of the provided speaker identification profiles.
        /// </summary>
        public string IdentifiedProfileId { get; set; }

        public VoiceRecognition.Model.AzCognitiveSpeaker.EnrollmentStatus EnrollmentStatus { get; set; }

        /// <summary>
        /// Remaining number of speech seconds to complete minimum enrollment.
        /// </summary>
        public float RemainingEnrollmentSpeechTime { get; set; }

        public VoiceRecognition.Model.AzCognitiveSpeaker.Confidence Confidence { get; set; }

        /// <summary>
        /// Seconds of useful speech in enrollment audio.
        /// </summary>
        public float SpeechTime { get; set; }

        /// <summary>
        /// Speaker identification profile enrollment length in seconds of speech.
        /// </summary>
        public float EnrollmentSpeechTime { get; set; }
    }

    public class OperationStatus
    {
        public Status Status { get; set; }

        /// <summary>
        /// Created date of the operation.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Last date of usage for this operation.
        /// </summary>
        public DateTime LastActionDateTime { get; set; }

        /// <summary>
        /// Detail message returned by this operation. Used in operations with failed status to show detail failure message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// This object exists only when the operation status is succeeded.
        /// </summary>
        public ProcessingResult ProcessingResult { get; set; }

    }
}
