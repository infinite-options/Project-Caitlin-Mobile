using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VoiceRecognition.Services.AzCognitiveSpeaker;
using VoiceRecognition.Model.AzCognitiveSpeaker;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using VoiceRecognition.ViewModel;
using VoiceRecognition.Config;

namespace AzureServicesUnitTest
{
    [TestClass]
    public class AzureServiceTests
    {
        private readonly IdentityProfileClient Idp = new IdentityProfileClient();
        readonly EnrollVoice enVoice = new EnrollVoice();

        [TestMethod]
        public void GetProfilesTest()
        {
            Task<List<Profile>> profilesTask = Idp.GetProfilesAsync();
            profilesTask.Wait();
            profilesTask.Result.ForEach(delegate(Profile profile){ Console.WriteLine(profile); });
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void IdentifyUnknownProfileTest()
        {
            List<Profile> profiles = new List<Profile>
            {
                new Profile { IdentificationProfileId = "00bb96ec-8089-4f95-999e-8a4af16c4680" },
                new Profile { IdentificationProfileId = "686f6e30-b25e-4862-a7d1-b6fb6ff6bd0a" }
            };
            string audioFilePath = "C:\\InfiniteOptions\\TestVoice\\JillTest.wav";
            Task<OperationStatus> operationTask = Idp.IdentifyProfile(profiles, audioFilePath);
            operationTask.Wait();
            OperationStatus opStatus = operationTask.Result;
            Assert.IsTrue(opStatus.Status == Status.succeeded);
            Assert.AreEqual("00000000-0000-0000-0000-000000000000", opStatus.ProcessingResult.IdentifiedProfileId);
        }

        [TestMethod]
        public void IdentifyKnownProfileTest()
        {
            List<Profile> profiles = new List<Profile>
            {
                new Profile { IdentificationProfileId = "00bb96ec-8089-4f95-999e-8a4af16c4680" },
                new Profile { IdentificationProfileId = "686f6e30-b25e-4862-a7d1-b6fb6ff6bd0a" }
            };
            string audioFilePath = "C:\\InfiniteOptions\\TestVoice\\sahil_test.wav";
            Task<OperationStatus> operationTask = Idp.IdentifyProfile(profiles, audioFilePath);
            operationTask.Wait();
            OperationStatus opStatus = operationTask.Result;
            Assert.IsTrue(opStatus.Status == Status.succeeded);
            Assert.AreEqual("686f6e30-b25e-4862-a7d1-b6fb6ff6bd0a", opStatus.ProcessingResult.IdentifiedProfileId);
        }

        [TestMethod]
        public void EnrollIdentityTest()
        {
            Profile profile = new Profile { IdentificationProfileId = "9e014ae6-1018-4fe5-9173-094a26723b60" };

            Task<OperationStatus> operationTask = Idp.EnrollAsync(profile, "C:\\InfiniteOptions\\TestVoice\\jill_input.wav");
            operationTask.Wait();
            OperationStatus opStatus = operationTask.Result;
            Assert.IsTrue(opStatus.Status == Status.succeeded);
            Assert.IsTrue(opStatus.ProcessingResult.EnrollmentStatus == EnrollmentStatus.Enrolled);
        }

        [TestMethod]
        public void CreateProfileTest()
        {
            Task<Profile> profileTask = Idp.CreateProfileAsync();
            profileTask.Wait();
            Profile profile = profileTask.Result;
            Trace.Write(profile.IdentificationProfileId);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void IdentifyAndEnrollTest()
        {
            Trace.WriteLine(enVoice.Message);
            string audioFilePath = "C:\\InfiniteOptions\\TestVoice\\ashu.wav";
            enVoice.IdentifyAndEnroll(this, audioFilePath).Wait();
            Trace.WriteLine(enVoice.Message);
            Assert.IsTrue(enVoice.Message== "We found you in out database");
        }

        [TestMethod]
        public void DeleteProfileTest()
        {
            //Task<Profile> profileTask = Idp.CreateProfileAsync();
            //profileTask.Wait();
            //Profile profile = profileTask.Result;
            Profile profile = new Profile()
            {
                IdentificationProfileId = "eda6c2b5-e82b-4eb3-86af-2f31d8a6b2e1"
            };
            Task<Boolean> status =  Idp.DeleteProfile(profile);
            status.Wait();
            Assert.IsTrue(status.Result);
        }

        [TestMethod]
        public void CheckDebugEnvironment()
        {
            Assert.IsTrue(AppConfig.IsDebug());
        }
    }
}
