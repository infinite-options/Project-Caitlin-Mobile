using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.CloudFirestore;
using ProjectCaitlin;
using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognition.Config;
using VoiceRecognition.Model;


namespace VoiceRecognition.Services.Firebase
{
    public class PeopleClient
    {
        private readonly string UserId;

        public PeopleClient(string UserId)
        {
            this.UserId = UserId;
        }

        public PeopleClient(user user) : this(user.id) { }

        /*--------------------------------------
         * @return: fresh list of People
         * If unsuccessfull return an emtpy list
         --------------------------------------*/
        public async Task<List<People>> GetAllPeopleAsync()
        {
            var documents = await CrossCloudFirestore.Current
                                    .Instance
                                    .GetCollection("users")
                                    .GetDocument(UserId)
                                    .GetCollection("people")
                                    .GetDocumentsAsync();
            List<People> peeps = new List<People>();
            if (documents.IsEmpty) return peeps;
                foreach(var doc in documents.Documents)
            {
                var peep = doc.Data;
                People person = new People()
                {
                    FirstName = peep["name"].ToString(),
                    Id = !peep.ContainsKey("unique_id") ? null : peep["unique_id"].ToString(),
                    HavePic = peep.ContainsKey("have_pic") && (Boolean)peep["have_pic"],
                    SpeakerId = !peep.ContainsKey("speaker_id") ? null : peep["speaker_id"].ToString(),
                    picUrl = !peep.ContainsKey("pic") ? null : peep["pic"].ToString(),
                    Important = peep.ContainsKey("important") && (Boolean)peep["important"],
                    PhoneNumber = !peep.ContainsKey("phone_number") ? null : peep["phone_number"].ToString(),
                    Relation = !peep.ContainsKey("relationship") ? null : peep["relationship"].ToString(),
                };
                peeps.Add(person);
            }
            return peeps;
        }

        public async Task<People> GetPeopleFromSpeakerIdAsync(string id)
        {
            People person = null;
            var peopleCollection = await CrossCloudFirestore.Current.Instance.GetCollection("users")
                                    .GetDocument(UserId)
                                    .GetCollection("people")
                                    .WhereEqualsTo("speaker_id", id)
                                    .GetDocumentsAsync();
            if (peopleCollection.IsEmpty) { return null; }

            var peep = peopleCollection.Documents.First().Data;
            if (peep != null && peep.ContainsKey("name")) {
                person = new People()
                {
                    FirstName = peep["name"].ToString(),
                    Id = !peep.ContainsKey("unique_id") ? null : peep["unique_id"].ToString(),
                    HavePic = peep.ContainsKey("have_pic") && (Boolean)peep["have_pic"],
                    SpeakerId = !peep.ContainsKey("speaker_id") ? null : peep["speaker_id"].ToString(),
                    picUrl = !peep.ContainsKey("pic") ? null : peep["pic"].ToString(),
                    Important = peep.ContainsKey("important") && (Boolean)peep["important"],
                    PhoneNumber = !peep.ContainsKey("phone_number") ? null : peep["phone_number"].ToString(),
                    Relation = !peep.ContainsKey("relationship") ? null : peep["relationship"].ToString(),
                };
            }
            return person;
        }

        public async Task<People> PostPeopleAsync(People people)
        {
            ProjectCaitlin.Services.Firebase.PeopleDto peopleDto = new ProjectCaitlin.Services.Firebase.PeopleDto()
            {
                pic = people.picUrl,
                have_pic = people.HavePic,
                speaker_id = people.SpeakerId,
                unique_id = people.Id,
                important = people.Important,
                name = people.FirstName,
                relation = people.Relation,
                phone_number = people.PhoneNumber,
            };
            await CrossCloudFirestore.Current
                         .Instance
                         .GetCollection("users")
                         .GetDocument(UserId)
                         .GetCollection("people")
                         .AddDocumentAsync(peopleDto);
            return people;
        }
    }
}
