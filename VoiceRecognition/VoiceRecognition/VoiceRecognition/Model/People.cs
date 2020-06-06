using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Model
{
    public class People
    {
        public string Url { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string PicUrl { get; set; }
        public Boolean HavePic { get; set; }
        public string SpeakerId { get; set; }
        public string Id { get; set; }
        public Boolean Important { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Relation { get; set; }
        public string PhoneNumber { get; set; }
    }
}
