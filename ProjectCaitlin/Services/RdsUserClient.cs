using Newtonsoft.Json.Linq;
using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognition.Model;

namespace ProjectCaitlin.Services
{
    public class RdsUserClient
    {
        RestService restService;
        string baseurl;
        public RdsUserClient()
        {
            restService = new RestService();
            baseurl = "https://3s3sftsr90.execute-api.us-west-1.amazonaws.com";
        }

        public async Task<user> GetUser(string index)
        {
            string url = baseurl + "/dev/api/v2/aboutme/" + index;
            var json = await restService.GetRequest(url);
            return JsonToUser(json);
        }


        private user JsonToUser(JObject json)
        {
            if (json == null)
            {
                return null;
            }
            if (json["result"] == null)
            {
                return null;
            }
            if (json["result"].LongCount() == 0)
            {
                return null;
            }
            user user = null;
            int index = 0;
            while (index<json["result"].LongCount())
            {
                if (json["result"][index]["user_unique_id"]!=null)
                {
                    user = parseUser(json["result"][index]);
                }else if (json["result"][index]["ta_people_id"]!=null)
                {

                }
                index += 1;
            }
            return user;
        }

        private user parseUser(JToken jtk)
        {
            user user = new user();
            user.id = jtk["user_unique_id"]?.ToString();
            user.firstName = jtk["first_name"]?.ToString();
            user.lastName = jtk["last_name"]?.ToString();
            user.aboutMe = parseAboutMe(jtk);
            return user;
        }

        private aboutMe parseAboutMe(JToken jtk)
        {
            aboutMe about = new aboutMe();
            about.message_card = jtk["message_card"]?.ToString();
            about.message_day = jtk["message_day"]?.ToString();
            about.have_pic = jtk["have_pic"]!=null ? (bool)jtk["have_pic"] : false;
            about.pic = jtk["picture"]?.ToString();

            return about;
        }

        //private void parseTrustedAdvisor(JToken jtk)
        //{
        //}
    }
}
