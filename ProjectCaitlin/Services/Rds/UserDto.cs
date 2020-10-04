using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCaitlin.Services.Rds
{
    // CAUTION: UserDto contains data for User and TAs (person or important people)
    public class UserDto
    {
        //public string ta_unique_id { get; set; }
        //public string name { get; set; }
        //public string ta_first_name { get; set; }
        //public string ta_last_name { get; set; }


        // User Data
        public bool user_have_pic { get; set; }
        public string message_card { get; set; }
        public string message_day { get; set; }
        public string user_picture { get; set; }
        public string user_first_name { get; set; }
        public string user_last_name { get; set; }
        public string user_email_id { get; set; }
        public string evening_time { get; set; }
        public string morning_time { get; set; }
        public string afternoon_time { get; set; }
        public string night_time { get; set; }
        public string day_end { get; set; }
        public string day_start { get; set; }
        public string time_zone { get; set; }


        // Important People or TA details below
        public string ta_people_id { get; set; }
        public string email_id { get; set; }
        public string people_name { get; set; }
        public string have_pic { get; set; }
        public string picture { get; set; }
        public string important { get; set; }
        public string user_unique_id { get; set; }
        public string relation_type { get; set; }



        public user toUser()
        {
            //return new user()
            //{
            //    firstName = ta_first_name,
            //    lastName = ta_last_name,
            //    id = ta_unique_id
            //};
            if (user_first_name == null) { return null; }

            return new user()
            {
                firstName = user_first_name,
                lastName = user_last_name,
                email = user_email_id,
                aboutMe = new aboutMe()
                {
                    have_pic = user_have_pic,
                    pic = user_picture,
                    message_card = message_card,
                    message_day = message_day
                },

            };
        }

        public person toPerson()
        {
            if(people_name==null || ta_people_id == null) { return null; }

            return new person()
            {
                name = people_name,
                relationship = relation_type,
                pic = picture,
                phoneNumber = "0000000000"
            };
        }
    }
}
