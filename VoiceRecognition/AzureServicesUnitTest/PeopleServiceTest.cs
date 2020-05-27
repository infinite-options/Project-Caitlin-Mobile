using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognition.Model;
using VoiceRecognition.Services.Firebase;

namespace AzureServicesUnitTest
{
    [TestClass]
    public class PeopleServiceTest
    {
        private PeopleClient PeopleService = new PeopleClient("oJ0ZrTo0R3dIbxiNpC1O");

        [TestMethod]
        public void GetAllPeople()
        {
            Task<List<People>> AllPeopleTask = PeopleService.GetAllPeopleAsync();
            AllPeopleTask.Wait();
            Assert.IsTrue(AllPeopleTask.Result.Count>0);
        }

        [TestMethod]
        public void GetPeopleFromId()
        {
            Task<People> PeopleTask = PeopleService.GetPeopleFromIdAsync("qwcceBRaEQYR6xrCuAN6");
            PeopleTask.Wait();
            Assert.AreEqual("Sarah", PeopleTask.Result.FirstName);
        }

        [TestMethod]
        public void GetPeopleFromSpeakerId()
        {
            Task<People> PeopleTask = PeopleService.GetPeopleFromSpeakerIdAsync("");
            PeopleTask.Wait();
            Assert.AreEqual("Sarah", PeopleTask.Result.FirstName);
        }

        [TestMethod]
        public void CreateNewPeopleEntryinFireBase()
        {
            People outgoing = new People()
            {
                FirstName = "test_name_1",
                Id = "test_Id_1",
                HavePic = false
            };
            Task<People> PeopleTask = PeopleService.PostPeopleAsync(outgoing);
            PeopleTask.Wait();
            Assert.AreEqual("test_name_1", PeopleTask.Result.FirstName);
        }

    }
}
