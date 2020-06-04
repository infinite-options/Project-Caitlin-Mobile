using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Plugin.AudioRecorder;
using PCLStorage;
using Xamarin.Forms;
using System.Diagnostics;
using System.Linq;

using VoiceRecognition.Services.AzCognitiveSpeaker;
using VoiceRecognition.Model.AzCognitiveSpeaker;
using VoiceRecognition.View;
using VoiceRecognition.Config;
using VoiceRecognition.Services.Firebase;
using VoiceRecognition.Model;
using System.Linq.Expressions;
using System.Security.Cryptography;
using ProjectCaitlin;

namespace VoiceRecognition.ViewModel
{
    public class EnrollVoice: ViewModelBase
    {
        private static readonly Profile UNKNOWN_PROFILE = new Profile { IdentificationProfileId = "00000000-0000-0000-0000-000000000000" };
        private readonly AudioRecorderService RecorderClient;
        //TODO: HardCoded Value, Need to have a Session Object
        //private PeopleClient PeopleService = new PeopleClient("7R6hAVmDrNutRkG3sVRy");
        private PeopleClient PeopleService = new PeopleClient("Ph2u3nRSZeYsWHitLSnv");
        //private PeopleClient PeopleService = new PeopleClient(App.user.id);
        Stopwatch sw;

        private readonly IdentityProfileClient IdClient;
        private static List<Profile> _AzProfiles = new List<Profile>();

        //Misclennious Variable
        private Profile TempProfile=null;
        private string TempAudioAddress = null;
        private string _PictureSource;
        public string PictureSource { get { return _PictureSource; } set { _PictureSource = value; NotifyPropertyChanged("PictureSource"); } }
        private string _Message = "Hello!";
        public string Message { get { return _Message; } set { _Message = value; NotifyPropertyChanged("Message"); } }
        private Boolean _DisplayForm = false;
        public Boolean DisplayForm { get { return _DisplayForm; } set { _DisplayForm = value; NotifyPropertyChanged("DisplayForm"); } }
        private Boolean _DisplayProfile = false;
        public Boolean DisplayProfile { get { return _DisplayProfile; } set { _DisplayProfile = value; NotifyPropertyChanged("DisplayProfile"); } }

        // TODO: Please Check if needed in this class
        INavigation Navigation;

        private List<Profile> AzProfiles
        {
            get
            {
                return _AzProfiles;
            }
            set
            {
                _AzProfiles = value;
                NotifyPropertyChanged("AzProfiles");
            }
        }



        public EnrollVoice()
        {
            Commands.Add("StartRecording", new Command(CMDStartRecording));
            Commands.Add("StopRecording", new Command(CMDStopRecording));
            Commands.Add("IdentifyAndEnroll", new Command(CMDIdentifyAndEnroll));
            Commands.Add("AddPeopleToFireBase", new Command(CMDAddPeopleToFireBase)); 

            IdClient = new IdentityProfileClient();

            RecorderClient = new AudioRecorderService()
            {
                StopRecordingOnSilence = true, // will stop recording after 2 seconds (default)
                StopRecordingAfterTimeout = false,  // stop recording after a max timeout (defined below)
                PreferredSampleRate = 16000, // sample rate of recording
                SilenceThreshold = 0.1f
            };
            sw = new Stopwatch();
            //RecorderClient.AudioInputReceived += EnrollIdentifyWrapper;
            RefreshAzProfiles();
        }

        public EnrollVoice(INavigation navigation) : this()
        {
            Navigation = navigation;
        }

        /* --------- 
         * UI Commands
         * Use these for binding them with UI
         * after creating these methods, don't forget to add these to Commands in the constructor
         * ------ */
#pragma warning disable CS1998
        public async void CMDStartRecording()
#pragma warning restore CS1998
        {
            _ = RecorderClient.StartRecording();
            Message = "Started";
        }

#pragma warning disable CS1998
        public async void CMDStopRecording()
#pragma warning restore CS1998
        {
            _ = RecorderClient.StopRecording();
            Message = "Stopped";
        }

        public void CMDIdentifyAndEnroll()
        {
            RecorderClient.AudioInputReceived += EnrollIdentifyWrapper;
            _ = RecorderClient.StartRecording();
            Message = "Identify and Enroll";

        }

        public void CMDAddPeopleToFireBase()
        {
            Navigation.PushAsync(new PeoplePage());
        }

        /* 
         * Legacy UI functions
         * same as command UI commands
         * TODO: need to remove these for cleanup
         */

        public async Task StartRecordingAsync()
        {
            _ = RecorderClient.StartRecording();
        }

        public async Task StopRecordingAsync()
        {
            _ = RecorderClient.StopRecording();
        }

        public async Task<People> AddFireBasePeople(People people)
        {
            printTraceMsg("Check if people present: " + (people != null));
            DisplayForm = false;
            printTraceMsg("TempProfile if people present: " + (TempProfile != null));
            people.SpeakerId = TempProfile.IdentificationProfileId;
            printTraceMsg("Adding entry to Firebase Name: "+people.FirstName+"\nSpeaker Id: "+people.SpeakerId);
            sw.Start();
            People peep = await PeopleService.PostPeopleAsync(people);
            sw.Stop();
            if (peep == null)
            {
                printTraceMsg("Unable to add a person to firebase " + "Time Elapsed: " + sw.ElapsedMilliseconds);
                sw.Reset();
            }
            else
            {
                printTraceMsg("Adding entry Firebase Complete: " + people.Url + "Time Elapsed: " + sw.ElapsedMilliseconds);
                sw.Reset();
            }
            AzIdFound_FirebaseFound(peep);
            return peep;
        }

        /* ----------------------
         * Events on recording completion Implementation
         * Logic goes here
         * Use wrappers below for adding the events
         ------------------------*/
         private async Task<Profile> CreateAzProfile()
        {
            Profile profile;
            try
            {
                var profileTask = IdClient.CreateProfileAsync();
                profileTask.Wait();
                profile = profileTask.Result;
                return profile;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }

        private async Task<Profile> EnrollNew(object sender, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
            {
                Message = "Try speaking Louder";
                await StartRecordingAsync();
                return null;
            }
            Profile profile;
            try
            {
                var profileTask = IdClient.CreateProfileAsync();
                profileTask.Wait();
                profile = profileTask.Result;
                var resultTask = IdClient.EnrollAsync(profile, audioFilePath);
                resultTask.Wait();
                var result = resultTask.Result;
                Message = result.ProcessingResult.EnrollmentStatus.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            return profile;
        }

        private async Task<Profile> EnrollExisting(object sender, Profile profile, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath) || profile==null)
            {
                return null;
            }
            try
            {
                var result = IdClient.Enroll(profile, audioFilePath);
                //resultTask.Wait();
                //var result = resultTask.Result;
                if (result.Status== Status.notstarted || result.Status == Status.running)
                {
                    Message = "Request timed out please try again!";
                }else if(result == null || result.Status== Status.failed)
                {
                    Message = "Something went wrong Please try again!";
                }
                else if(result.Status== Status.succeeded)
                {
                    Message = result.ProcessingResult.EnrollmentStatus.ToString();
                    profile.RemainingEnrollmentSpeechTime = result.ProcessingResult.RemainingEnrollmentSpeechTime;
                    profile.EnrollmentStatus = result.ProcessingResult.EnrollmentStatus;
                    profile.EnrollmentSpeechTime = result.ProcessingResult.EnrollmentSpeechTime;
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
            return profile;
        }

        private async Task<Profile> IdentifyProfile(object sender, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
            {
                //Message = "Try speaking Louder";
                await StartRecordingAsync();
                return null;
            }

            IEnumerable<Profile> filteredProfiles = GetAzProfileWthEnrollmentStatus(EnrollmentStatus.Enrolled);

            Profile prof = null;
            foreach (var batch in filteredProfiles.Batch(10))
            {
                try
                {
                    OperationStatus opStatus = await IdClient.IdentifyProfile(batch.ToList(), audioFilePath);
                    if (opStatus.Status == Status.succeeded && opStatus.ProcessingResult != null)
                    {
                        if (opStatus.ProcessingResult.IdentifiedProfileId != UNKNOWN_PROFILE.IdentificationProfileId)
                        {
                            prof = new Profile() { IdentificationProfileId = opStatus.ProcessingResult.IdentifiedProfileId };
                            break;
                        }
                    }

                    if (opStatus.Status == Status.failed)
                    {
                        //TODO : Throw an exception, May need to change the implementation
                        //Message = "something went wrong";
                        throw new SpeakerRecognitionException()
                        {
                            Code = "404",
                            Message = "Request Failed, unable to load Azure Profiles"
                        };
                    }

                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.StackTrace);
                    throw e;
                }
            }
            if (prof != null)
            {
                //Message = "We found you in out database";
                return prof;
            }
            return UNKNOWN_PROFILE;

        }

        public async Task<string> IdentifyAndEnroll(object sender, string audioFilePath)
        {
            printTraceMsg("Starting to Identify");
            sw.Start();
            Profile p = await IdentifyProfile(sender, audioFilePath);
            sw.Stop();
            printTraceMsg("Identification of voice Complete: " + sw.ElapsedMilliseconds);
            sw.Reset();
            //Profile p = new Profile()
            //{
            //    IdentificationProfileId = "00bb96ec-8089-4f95-999e-8a4af16c4680"
            //};
            //Profile p = UNKNOWN_PROFILE;
            if (p == null) 
            {
                Message = "Audio length not enough please try again!";
                return null; 
            }
            if (p != UNKNOWN_PROFILE)
            {
                People peep = await PeopleService.GetPeopleFromSpeakerIdAsync(p.IdentificationProfileId);
                if (peep != null)
                {
                    AzIdFound_FirebaseFound(peep);
                }
                else
                {
                    if (AppConfig.IsDebug()) { Trace.WriteLine("Identification profile Id: "+p.IdentificationProfileId); }
                    TempProfile = p;
                    TempAudioAddress = audioFilePath;
                    AzIdFound_FirebaseNotFound(p);
                }
                return "User Found";
            }
            // TODO: Check what to do when unable to find in Azure
            //AzIdFound_NotCreatedNew(audioFilePath);
            TempAudioAddress = audioFilePath;
            AzIdNotFound();
            return "User Creation Initiated";
        }

        /*
         * Wrapper for events 
         */
        private async void EnrollIdentifyWrapper(object sender,  string audioFilePath)
        {
            try
            {
                _ = IdentifyAndEnroll(sender, audioFilePath);
                

            }
            catch(Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            finally
            {
                RecorderClient.AudioInputReceived -= EnrollIdentifyWrapper;
            }
        }

        private async void EnrollNewWrapper(object sender, string audioFilePath)
        {
            try
            {
                _ = EnrollNew(sender, audioFilePath);
            }catch(Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            finally
            {
                RecorderClient.AudioInputReceived -= EnrollNewWrapper;
            }
        }

        private async void IdentifyProfileWrapper(object sender, string audioFilePath)
        {
            try
            {
                _ = IdentifyProfile(sender, audioFilePath);
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.StackTrace);
            }
            finally
            {
                RecorderClient.AudioInputReceived -= IdentifyProfileWrapper;
            }
        }


        /*
         * Helper Functions
         */
        private async void RefreshAzProfiles()
        {
            try
            {
                AzProfiles = await IdClient.GetProfilesAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        private IEnumerable<Profile> GetAzProfileWthEnrollmentStatus(EnrollmentStatus status)
        {
            return from profile in AzProfiles
                   where profile.EnrollmentStatus == status
                   select profile;
        }

        // Helper Funtions


        public void AzIdFound_FirebaseFound(People people)
        {
            DisplayForm = false;
            PictureSource = people.picUrl;
            Message = "Name: " + people.FirstName + "\nImportant: " + people.Important + "\nPhone Number: " + people.PhoneNumber + "\nSpeaker Id: " + people.SpeakerId;
            DisplayProfile = true;
            //Page profilePage = new ProfielPage()
            //{
            //    FirstName = people.FirstName,
            //    Important = people.Important,
            //    PhoneNumber = people.PhoneNumber
            //};
            //await Navigation.PushAsync(profilePage);
            //Navigation.InsertPageBefore(new EnrollmentPage(), profilePage);
            //Application.Current.MainPage = new NavigationPage(new ProfielPage());
        }

        public void AzIdFound_FirebaseNotFound(Profile azProfile)
        {
            Message = "We have heard this voice("+azProfile.IdentificationProfileId + ") before. However, we were unable to identify this person.\nIf you want you can add the details below";
            DisplayForm = true;
        }

        public void AzIdNotFound()
        {
            Message = "Unable to find the voice.\nIf you wanmt to add Please fill the form and enroll";
            DisplayForm = true;
        }

        public void AzIdNotFound_CreatedNew(Profile profile)
        {
            Message = "Please provide your name for registration\nYour Azure Id is: " + profile.IdentificationProfileId;
            DisplayForm = true;
        }

        public void AzIdNotFound_createNewProfile(People people)
        {
            //AzIdNotFound_NotCreatedNew(TempAudioAddress);
            int min_enroll_sec_recording = 30;
            float secs;
            try
            {
                //Task<float> secsTask = CheckAudioLength(TempAudioAddress, 16, 1, 16000);
                secs = CheckAudioLength(TempAudioAddress, 16, 1, 16000);
                //secsTask.Wait();
                //float secs = secsTask.Result;
                if (secs > min_enroll_sec_recording)
                {
                    printTraceMsg("Starting Create Azure Profile");
                    sw.Start();
                    Task<Profile> createProfileTask = CreateAzProfile();
                    createProfileTask.Wait();
                    sw.Stop();
                    printTraceMsg("Create Azure Profile Complete.\n New Id:"+createProfileTask.Result.IdentificationProfileId+"Time Elapsed: "+sw.ElapsedMilliseconds);
                    sw.Reset();
                    printTraceMsg("Starting Enrolling Azure Profile Id:" + createProfileTask.Result.IdentificationProfileId);
                    sw.Start();
                    Task<Profile> enrollTask = EnrollExisting(null,createProfileTask.Result,TempAudioAddress);
                    enrollTask.Wait();
                    sw.Stop();
                    printTraceMsg("Enroll Azure Profile Complete.\n Id:" + createProfileTask.Result.IdentificationProfileId +
                        "\n Enrollment Status: "+createProfileTask.Result.EnrollmentStatus +
                        "\n Remaining Enrollment Speech Time: "+ createProfileTask.Result.RemainingEnrollmentSpeechTime+
                        "\n Enrollment Speech Time: " + createProfileTask.Result.EnrollmentSpeechTime +
                        "\nTime Elapsed: " + sw.ElapsedMilliseconds);
                    sw.Reset();
                    if (enrollTask.Result.EnrollmentStatus== EnrollmentStatus.Enrolled)
                    {
                        //people.SpeakerId = enrollTask.Result.IdentificationProfileId;
                        //Task<People> postPeopleTask = PeopleService.PostPeopleAsync(people);
                        //postPeopleTask.Wait();
                        TempProfile = enrollTask.Result;
                        Task<People> peepTask = AddFireBasePeople(people);
                        peepTask.Wait();
                        //AzIdFound_FirebaseFound(peepTask.Result);
                        
                    }
                    else
                    {
                        Trace.WriteLine(enrollTask.Result.EnrollmentStatus);
                        Message = "Audio length long but still not enrolled. Need to implement";
                    }
                }
                else
                {
                    Message = "Audio Length not sufficient. Need to implement. Length in secs: "+ secs;
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine("Unable to complete adding new user");
                Trace.WriteLine(e);
                Trace.WriteLine(e.StackTrace);
                Message = "Something Went wrong. Need to implement";
                DisplayForm = true;
            }
        }

        public void AzIdNotFound_createNewProfileBackground(People people)
        {
            Task.Run(()=> { AzIdNotFound_createNewProfile(people); });
        }

        public async Task<List<Profile>> AzIdRemove(List<Profile> profiles)
        {
            List<Profile> deleted = new List<Profile>();
            foreach (var profile in profiles)
            {
                try
                {
                    await Task.Delay(100);
                    Boolean status = await IdClient.DeleteProfile(profile);
                    if (status)
                    {
                        deleted.Add(profile);
                    }
                }
                catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw e;
                }
            }
            return deleted;
        }

        // Returns audio length in seconds
        public static float CheckAudioLength(string audioFilePath, long bitsPerFrame, int channels, long framesPerSeconds)
        {
            long bytesPerSecond = (bitsPerFrame / 8) * channels * framesPerSeconds;
            float Secondslength = 0.0f;
            try
            {
                var fileTask = FileSystem.Current.GetFileFromPathAsync(audioFilePath);
                fileTask.Wait();
                using (var audioStream = fileTask.Result.OpenAsync(FileAccess.Read))
                {
                    audioStream.Wait();
                    long ByteLength = audioStream.Result.Length;
                    Secondslength = ByteLength / bytesPerSecond;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                Trace.WriteLine(ex.StackTrace);
                throw;
            }

            return Secondslength;
        }

        private void printTraceMsg(string msg)
        {
            if (AppConfig.IsDebug())
            {
                Trace.WriteLine("################################################\n"+msg+"\n################################################");
            }
        }
    }
}
