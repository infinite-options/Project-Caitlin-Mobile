using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Services.VoiceRecorder
{
    class RecorderFactory
    {
        public static IRecorder GetRecorder()
        {
            return GetRecorder("DEFAULT");
        }

        public static IRecorder GetRecorder(string recorder)
        {
            return new DefaultContreteRecorder();
        }
    }
}
