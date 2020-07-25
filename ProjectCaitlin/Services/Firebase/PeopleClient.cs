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
                People person = ToPeople(peep);
                if (peep != null) peeps.Add(person);
            }
            return peeps;
        }

        public People ToPeople(IDictionary<string, object> peep)
        {
            if (peep.Count == 0) return null;
            People person = new People();
            foreach (String p in peep.Keys)
            {
                switch (p)
                {
                    case "name":
                        {
                            person.FirstName = (peep["name"] == null) ? null : peep["name"].ToString();
                            break;
                        }
                    case "unique_id":
                        {
                            person.Id = (peep["unique_id"] == null) ? null : peep["unique_id"].ToString();
                            break;
                        }
                    case "have_pic":
                        {
                            person.HavePic = (peep["have_pic"] == null || (Boolean)peep["have_pic"] == false) ? false : true;
                            break;
                        }
                    case "speaker_id":
                        {
                            person.SpeakerId = (peep["speaker_id"] == null) ? null : peep["speaker_id"].ToString();
                            break;
                        }
                    case "pic":
                        {
                            person.picUrl = (peep["pic"] == null) ? null : peep["pic"].ToString();
                            break;
                        }
                    case "phone_number":
                        {
                            person.PhoneNumber = (peep["phone_number"] == null) ? null : peep["phone_number"].ToString();
                            break;
                        }
                    case "relationship":
                        {
                            person.PhoneNumber = (peep["relationship"] == null) ? null : peep["relationship"].ToString();
                            break;
                        }
                    case "important":
                        {
                            person.HavePic = (peep["important"] == null || (Boolean)peep["important"] == false) ? false : true;
                            break;
                        }
                }
            }
            return person;
            }

        public async Task<People> GetPeopleFromSpeakerIdAsync(string id)
        {
            var peopleCollection = await CrossCloudFirestore.Current.Instance.GetCollection("users")
                                    .GetDocument(UserId)
                                    .GetCollection("people")
                                    .WhereEqualsTo("speaker_id", id)
                                    .GetDocumentsAsync();
            if (peopleCollection.IsEmpty) { return null; }

            var peep = peopleCollection.Documents.First().Data;
            People person = ToPeople(peep);
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
