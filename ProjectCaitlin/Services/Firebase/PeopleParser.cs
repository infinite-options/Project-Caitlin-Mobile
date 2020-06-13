using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using VoiceRecognition.Model;

namespace VoiceRecognition.Services.Firebase
{
    class PeopleParser
    {
        public People JsonToObject(JToken jtk)
        {
            if (jtk == null || jtk["name"]==null)
            {
                return null;
            }
            People person = new People()
            {
                Url = jtk["name"].ToString(),
                CreateTime = DateTime.Parse(jtk["createTime"].ToString()),
                UpdateTime = DateTime.Parse(jtk["updateTime"].ToString())
            };
            var fields = jtk["fields"];
            if (fields == null)
            {
                return person;
            }
            person.FirstName = fields["name"] == null ? null : fields["name"]["stringValue"].ToString();
            person.Id = fields["unique_id"] == null ? null : fields["unique_id"]["stringValue"].ToString();
            person.HavePic = fields["have_pic"] == null ? false : (Boolean) fields["have_pic"]["booleanValue"];
            person.SpeakerId = fields["speaker_id"] == null ? null : fields["speaker_id"]["stringValue"].ToString();
            person.picUrl = fields["pic"] == null ? null : fields["pic"]["stringValue"].ToString();
            person.Important = fields["important"] == null ? false : (Boolean) fields["important"]["booleanValue"];
            person.PhoneNumber = fields["phone_number"] == null ? null : fields["phone_number"]["stringValue"].ToString();
            person.Relation = fields["relationship"] == null ? null : fields["relationship"]["stringValue"].ToString();
            return person;
        }

        public JObject ObjectToJson(People people)
        {
            if (people == null) { return null; }
            JObject json = new JObject();
            JObject fields = new JObject();
            
            if (people.FirstName != null) { fields.Add("name", StringValue(people.FirstName)); }
            if (people.Id != null) { fields.Add("unique_id", StringValue(people.Id)); }
            fields.Add("have_pic", BooleanObject(people.HavePic));
            fields.Add("important", BooleanObject(people.Important));
            if (people.PhoneNumber != null) { fields.Add("phone_number", StringValue(people.PhoneNumber)); }
            if (people.Relation != null) { fields.Add("relationship", StringValue(people.Relation)); }
            if (people.SpeakerId != null) { fields.Add("speaker_id", StringValue(people.SpeakerId)); }
            json.Add("fields",fields);
            return json;
        }

        private JObject StringValue(string value)
        {
            JObject jo = new JObject();
            jo.Add("stringValue", value);
            return jo;
        }

        private JObject BooleanObject(Boolean value)
        {
            JObject jo = new JObject();
            jo.Add("booleanValue", value);
            return jo;
        }
    }
}
