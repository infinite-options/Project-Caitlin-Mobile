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
    public class UserServiceTests
    {
        private UserClient UserService = new UserClient();

        [TestMethod]
        public void GetUserFromIdTest()
        {
            string TestId = "VMn4ruzDtoz5P66W3mf6";
            Task<User> UserTask = UserService.GetUserFromId(TestId);
            UserTask.Wait();
            Assert.AreEqual(UserTask.Result.firstName, "tama");
        }
    }
}
