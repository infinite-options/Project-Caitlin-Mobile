using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;


using VoiceRecognition.Config;
using VoiceRecognition.Model;
using VoiceRecognition.Model.AzCognitiveSpeaker;

namespace VoiceRecognition.Services.AzCognitiveSpeaker
{
    public class IdentityProfileClient
    {
        private readonly HttpClient client;

        public IdentityProfileClient()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(AzCognitiveSpeakerAPI.END_POINT)
            };

            //Request Headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", AzCognitiveSpeakerAPI.KEY_1);
        }

        public async Task<Profile> CreateProfileAsync()
        {
            string uri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL;

            HttpResponseMessage response;
            var serializer = new JsonSerializer();

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{\"locale\": \"en-us\"}");
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                Task<HttpResponseMessage> resTask = client.PostAsync(uri, content);
                resTask.Wait();
                response = resTask.Result;
                Console.WriteLine(response);
            }

            string resString = await response.Content.ReadAsStringAsync();
            using (var tReader = new StringReader(resString))
            {
                using (var jReader = new JsonTextReader(tReader))
                {
                    Profile profile = serializer.Deserialize<Profile>(
                        jReader);
                    return profile;
                }
            }
        }

        public async Task<List<Profile>> GetProfilesAsync()
        {
            string uri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL;
            Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
            responseTask.Wait();
            Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
            List<Profile> profiles = JsonConvert.DeserializeObject<List<Profile>>(contentTask.Result);
            return profiles;
        }

        public async Task<Profile> GetProfileAsync(string id)
        {
            string uri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL + "/" + id;
            Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
            responseTask.Wait();
            Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
            Profile profile = JsonConvert.DeserializeObject<Profile>(contentTask.Result);
            return profile;
        }

        public async Task<OperationStatus> EnrollAsync(Profile profile, string audioFilePath)
        {
            try
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["shortAudio"] = $"{true}";
                var requestUri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL + "/"+profile.IdentificationProfileId+"/enroll?" + queryString;
                var request = PrepareMediaRequest(audioFilePath, requestUri);

                var responseTask = client.SendAsync(request);
                responseTask.Wait();
                var response = responseTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    throw JsonConvert.DeserializeObject<SpeakerRecognitionException>(result);
                }

                System.Collections.Generic.IEnumerable<string> operationTrackingHeaders = response.Headers.GetValues("Operation-Location");
                var trackingUrl = operationTrackingHeaders.FirstOrDefault();
                OperationStatus res = await GetOperationStatus(trackingUrl);
                while(res.Status == Status.running)
                {
                    Task.Delay(3000);
                    res = await GetOperationStatus(trackingUrl);
                }
                //Task<OperationStatus> contentTask = responseTask.Result.Content.ReadAsStringAsync();
                //OperationStatus operationStatus = contentTask.Result;
                //OperationStatus opStatus = JsonConvert.DeserializeObject<OperationStatus>(contentTask.Result);
                //return opStatus;
                //return trackingUrl;
                return res;
            }
            catch(Exception ex)
            {
                Debug.WriteLine("EXCEPTION: " + ex.Message);
                throw;
            }
        }

        public async Task<OperationStatus> GetOperationStatus(string uri)
        {
            //string uri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL;
            Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
            responseTask.Wait();
            Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
            string content = contentTask.Result;
            OperationStatus opStatus = JsonConvert.DeserializeObject<OperationStatus>(contentTask.Result);
            return opStatus;
        }

        protected HttpRequestMessage PrepareMediaRequest(string audioFilePath, string requestUri)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.TransferEncodingChunked = true;
            request.Headers.ExpectContinue = true;
            request.Headers.Accept.ParseAdd(MimeTypes.Json);
            request.Headers.Accept.ParseAdd(MimeTypes.Xml);
            request.Content = MediaRequestHelper.PopulateRequestContent(audioFilePath);
            return request;
        }

    }
}
