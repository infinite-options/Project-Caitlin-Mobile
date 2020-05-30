using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Config
{
    public enum Environment
    {
        debug, prod
    }

    public class AppConfig
    {
        static Environment env = Environment.debug;

        public static Boolean IsDebug()
        {
            return env == Environment.debug ? true : false;
        }

        public static Boolean IsProd()
        {
            return env == Environment.prod ? true : false;
        }

        public static Environment GetEnvironment()
        {
            return env;
        }
    }
}
