using System;
using System.Threading.Tasks;
using SpeakerRecognitionAPI;
using SpeakerRecognitionAPI.Interfaces;
using SpeakerRecognitionAPI.Models;
using VoicePay.Helpers;
using VoicePay.Services;
using VoicePay.Services.Interfaces;
using VoicePay.Views.Enrollment;
using Xamarin.Forms;
using System.Net.Http.Headers;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace VoicePay.ViewModels.Enrollment
{
    public class AudioVerifyViewModel : AudioRecordingBaseViewModel
    {
        //private string fullids;
        private readonly IBeepPlayer _beeper;
        public List<string> idList = new List<string>();
        //private readonly ISpeakerVerification _verificationService;
        private readonly ISpeakerIdentification _verificationService;
        //private SpeakerIdentificationClient instance;
        public SpeakerServiceBase speaker;

        //private string PhraseMessage => $"\"{Settings.EnrolledPhrase}\"";
        public bool IsPageActive { get; set; } = true;

        //public AudioVerifyViewModel() : this(VerificationService.Instance) { }
        public AudioVerifyViewModel() : this(IdentificationService.Instance) { }
        //public AudioVerifyViewModel(ISpeakerVerification verificationService)
        public AudioVerifyViewModel(ISpeakerIdentification verificationService)
        {
            StateMessage = "Hold on...";

            _beeper = DependencyService.Get<IBeepPlayer>();
            _verificationService = verificationService;

            Recorder.AudioInputReceived += async (object sender, string e) => { await Recorder_AudioInputReceived(sender, e); };
        }

        //public AudioVerifyViewModel(SpeakerIdentificationClient instance)
        //{
        //    this.instance = instance;
        //}

        public async Task Recorder_AudioInputReceived(object sender, string audioFilePath)
        {
            if (string.IsNullOrEmpty(audioFilePath))
            {
                StateMessage = "We can't hear you :/";
                Message = "Try speaking louder";
                if(IsPageActive)
                    await WaitAndStartRecording();
                return;
            }

            IsBusy = true;

            StateMessage = "Identifying...";
            Message = string.Empty;

            try
            {
                //var verificationResponse = await _verificationService.VerifyAsync(audioFilePath, Settings.UserIdentificationId);
                //var verificationResponse = await _verificationService.IdentifyAsync(audioFilePath, Settings.UserIdentificationId);
                //await MakeRequest();

                //wait MakeRequest();
                await MakeRequestAWS();
                string fullIDstring = string.Join(",", idList.ToArray());
                System.Diagnostics.Debug.WriteLine(fullIDstring);
                //System.Diagnostics.Debug.WriteLine(audioFilePath);


                //string ids = ("9e6bcff7-f36b-41bd-adc9-4dd0960aa02f,c6f5c1ec-6890-4bc0-aa48-7db8df4ff077");
                //var res = await SpeakerServiceBase;
                //string[] ids = {fullIDstring};
                //System.Diagnostics.Debug.WriteLine(ids[0]);
                //System.Diagnostics.Debug.WriteLine(fullids);
                var verificationResponse = await _verificationService.IdentifyAsync(audioFilePath, fullIDstring);
                //System.Diagnostics.Debug.WriteLine(verificationResponse);
                await Task.Delay(3000);
                var identificationResponse = await _verificationService.CheckIdentificationStatusAsync(verificationResponse);
                //System.Diagnostics.Debug.WriteLine(identificationResponse);
                //await Task.Delay(1000);
                string identifiedID = (identificationResponse.ProcessingResult.IdentifiedProfileId);
                ////System.Diagnostics.Debug.WriteLine(identificationResponse);

                //await ReturnProfileName(identifiedID);

                if (identifiedID == "00000000-0000-0000-0000-000000000000")
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //THIS needs to be a new page, where we send audio file and retrain

                        await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                        DisplayAlert("Error", "Speaker ID not detected, have you trained you voice?", "OK");
                    });
                }
                else
            {

                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(String.Format("https://tlusrtjjq5.execute-api.us-west-1.amazonaws.com/dev/api/v1/profile/{0}", identifiedID));
                request.Method = HttpMethod.Get;
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);

                HttpContent content = response.Content;
                var jsonResponse = await content.ReadAsStringAsync();

                string overrideme = String.Empty;
                string nameMSG = overrideme;

                //System.Diagnostics.Debug.WriteLine(kitchensString);

                    foreach (Match m in Regex.Matches(jsonResponse, "profileName?"))
                {
                    //string fullids = String.Format("{0},", jsonResponse.Substring(m.Index + 26, 36));
                    string fullNameJSON = String.Format(jsonResponse.Substring(m.Index + 26, 36));
                    //idList.Add(fullids);
                    //fullids = String.Format("{0},", ids);
                    //System.Diagnostics.Debug.WriteLine(fullids);

                    //string phrase = "blah blah";
                    string[] words = fullNameJSON.Split(':');
                    string fullName = words[1];
                    System.Diagnostics.Debug.WriteLine(fullName);
                    nameMSG = String.Format("Hello, {0}", fullName);
                    


                    //foreach (var word in words)
                    //{
                    //    System.Diagnostics.Debug.WriteLine($"{word}");
                    //}
                }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Application.Current.MainPage.Navigation.PushModalAsync(new MainPage());
                        DisplayAlert("Success!", nameMSG, "OK");
                    });
                }


                    if (identifiedID == null)
                {
                    DisplayAlert("Error", "Speaker Not Recognized", "OK");
                }

                var idResponse = String.Format("The Identified Speaker is {0}", identifiedID);
                idList.Clear();



                //System.Diagnostics.Debug.WriteLine(Settings.UserIdentificationId);
                //System.Diagnostics.Debug.WriteLine(verificationResponse);
                //System.Diagnostics.Debug.WriteLine(identificationResponse.ProcessingResult.IdentifiedProfileId);
                System.Diagnostics.Debug.WriteLine(idResponse);

                //if (verificationResponse.Result == Result.Accept && verificationResponse.Confidence != Confidence.Low)
                //if (verificationResponse == "null")
                //if(identifiedID == "edd7c8d3-a84e-4096-afea-ec331d302201")
                //{

                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        await Application.Current.MainPage.Navigation.PushModalAsync(new CorrectResultPage("John Baer"));
                //        //DisplayAlert("Success!", "Hello, John", "OK");
                //    });
                //}
                //else if (identifiedID == "c53374e1-ea3c-410c-ae42-7c3c299e04e3")
                //{
                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        await Application.Current.MainPage.Navigation.PushModalAsync(new CorrectResultPage("Kyle Hoefer"));
                //        //DisplayAlert("Success!", "Hello, Kyle", "OK");
                //    });
                //}
                //else
                //{
                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        await Application.Current.MainPage.Navigation.PushModalAsync(new IncorrectResultPage());
                //        //DisplayAlert("Error", "Speaker ID not detected, have you trained you voice?", "OK");
                //    });
                //}
                //else
                //{
                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        await Application.Current.MainPage.Navigation.PushModalAsync(new IncorrectResultPage());
                //        DisplayAlert("Error", "Speaker ID not detected, have you trained you voice?", "OK");
                //    });
                //}

            }
            //catch (Exception ex)
            //{
            //    DisplayAlert("Error", ex.Message, "OK");
            //}
            finally
            {
                IsBusy = false;
            }
        }

        //public async Task<string> MakeRequest()
        //{
        //var client = new HttpClient();
        ////var queryString = HttpUtility.ParseQueryString(string.Empty);
        //// Request headers
        //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4fb1a2da4be64d0ebba737b732740a87");
        //        var uri = "https://westus.api.cognitive.microsoft.com/spid/v1.0/identificationProfiles?";
        //var response = await client.GetAsync(uri);
        //var jsonResponse = await response.Content.ReadAsStringAsync();

        //    foreach (Match m in Regex.Matches(jsonResponse, "identificationProfileId?"))
        //    {
        //        //string fullids = String.Format("{0},", jsonResponse.Substring(m.Index + 26, 36));
        //        string fullids = String.Format(jsonResponse.Substring(m.Index + 26, 36));
        //        idList.Add(fullids);
        //        //fullids = String.Format("{0},", ids);
        //        //System.Diagnostics.Debug.WriteLine(jsonResponse.Substring(m.Index+26,36));

        //    }
        //    return null;
        //}


        public async Task<string> MakeRequestAWS()
        {
            var client = new HttpClient();
            //var queryString = HttpUtility.ParseQueryString(string.Empty);
            // Request headers
            //client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "4fb1a2da4be64d0ebba737b732740a87");
            var uri = "https://tlusrtjjq5.execute-api.us-west-1.amazonaws.com/dev/api/v1/profiles";
            var response = await client.GetAsync(uri);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            foreach (Match m in Regex.Matches(jsonResponse, "profileID?"))
            {
                //string fullids = String.Format("{0},", jsonResponse.Substring(m.Index + 26, 36));
                string fullids = String.Format(jsonResponse.Substring(m.Index + 36, 36));
                idList.Add(fullids);
                //fullids = String.Format("{0},", ids);
                //System.Diagnostics.Debug.WriteLine(fullids);

            }
            return null;
        }

        //public async Task<string> ReturnProfileName()
        //{
        //    //ACCESS DYNAMO DB: SEND IDENTIFIED-ID AND RETURN SPEAKER NAME

        //    var request = new HttpRequestMessage();
        //    request.RequestUri = new Uri(String.Format("https://tlusrtjjq5.execute-api.us-west-1.amazonaws.com/dev/api/v1/profile/{0}", identifiedID));
        //    request.Method = HttpMethod.Get;
        //    var client = new HttpClient();
        //    HttpResponseMessage response = await client.SendAsync(request);

        //    HttpContent content = response.Content;
        //    var jsonResponse = await content.ReadAsStringAsync();

        //    //System.Diagnostics.Debug.WriteLine(kitchensString);

        //    foreach (Match m in Regex.Matches(jsonResponse, "profileName?"))
        //    {
        //        //string fullids = String.Format("{0},", jsonResponse.Substring(m.Index + 26, 36));
        //        string fullids = String.Format(jsonResponse.Substring(m.Index + 36, 36));
        //        idList.Add(fullids);
        //        //fullids = String.Format("{0},", ids);
        //        //System.Diagnostics.Debug.WriteLine(fullids);

        //        string phrase = "blah blah";
        //        string[] words = phrase.Split(' ');

        //        foreach (var word in words)
        //        {
        //            System.Diagnostics.Debug.WriteLine($"<{word}>");
        //        }

        //    }
        //    return null;
        //}

        //foreach (string element in idList)
        //{
        //    //System.Diagnostics.Debug.WriteLine(element);
        //}


        //var json = JsonValue.Parse(jsonResponse);
        //var data = json["data"];

        //foreach (var dataItem in data)
        //{
        //    string myValue = dataItem["identificationProfileId"]; //Here is the compilation error
        //                                                                 //...
        //}

        //System.Diagnostics.Debug.WriteLine(jsonResponse);
        //var result = JsonConvert.DeserializeObject<IdentificationResult>(jsonResponse);
        //System.Diagnostics.Debug.WriteLine(result);

        //}

        public override async Task StartRecording()
        {
            await Recorder.StartRecording();
            _beeper.Beep();
            StateMessage = "Listening...";
            //Message = PhraseMessage;
        }
    }
}
