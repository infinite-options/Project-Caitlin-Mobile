using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Config
{
    public class FirebaseFirestore
    {
        /* -------------------------------
         * Structure:
         * e.g.
         * Base url / project url / user url / people url
         * -------------------------------
         */

        public const string BASE_URL = "https://firestore.googleapis.com/v1";
        private const string project_name = "";
        public const string PROJECT_URL = "/projects/" + project_name;
        public const string USER_URL = "/databases/(default)/documents/users";
        public const string PEOPLE_URL = "/people";
    }
}
