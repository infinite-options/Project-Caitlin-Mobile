using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceRecognition.Model;

namespace VoiceRecognition.Services.Firebase
{
    public class UserJson
    {
        public class Fields
        {
            public class FirstName
            {
                public string stringValue { get; set; }
            }

            public class LastName
            {
                public string stringValue { get; set; }
            }

            public FirstName first_name { get; set; }
            public LastName last_name { get; set; }

        }

        public string name { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public Fields fields { get; set; }

        public user ToUser()
        {
            return new user()
            {
                firstName = fields.first_name.stringValue
            };
        }
    }
}
