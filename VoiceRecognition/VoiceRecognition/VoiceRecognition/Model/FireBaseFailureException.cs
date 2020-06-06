using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Model
{
    class FireBaseFailureException : Exception
    {
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
