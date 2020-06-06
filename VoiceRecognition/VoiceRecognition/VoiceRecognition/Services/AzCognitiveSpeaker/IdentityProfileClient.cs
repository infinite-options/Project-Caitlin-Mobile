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
            Trace.WriteLine("Async : CreateProfileAsync Started");
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
                Trace.WriteLine(response);
            }

            string resString = await response.Content.ReadAsStringAsync();
            using (var tReader = new StringReader(resString))
            {
                using (var jReader = new JsonTextReader(tReader))
                {
                    Profile profile = serializer.Deserialize<Profile>(
                        jReader);
                    Trace.WriteLine("Async : CreateProfileAsync Completed");
                    return profile;
                }
            }
        }

        public async Task<List<Profile>> GetProfilesAsync()
        {
            try
            {
                string uri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL;
                Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
                responseTask.Wait();
                Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
                List<Profile> profiles = JsonConvert.DeserializeObject<List<Profile>>(contentTask.Result);
                return profiles;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public async Task<Profile> GetProfileAsync(string id)
        {
            try
            {
                string uri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL + "/" + id;
                Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
                responseTask.Wait();
                Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
                Profile profile = JsonConvert.DeserializeObject<Profile>(contentTask.Result);
                return profile;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
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
                await Task.Delay(1000);
                OperationStatus res = await GetOperationStatusAsync(trackingUrl);
                int request_trails = 1;
                while (res.Status == Status.running || res.Status == Status.notstarted)
                {
                    await Task.Delay(2000);
                    res = await GetOperationStatusAsync(trackingUrl);
                    if (request_trails > 5)
                    {
                        break;
                    }
                }
                return res;
            }
            catch(Exception e)
            {
                Debug.WriteLine("EXCEPTION: " + e.Message);
                throw;
            }
        }

        public OperationStatus Enroll(Profile profile, string audioFilePath)
        {
            Trace.WriteLine("IdentityProfileClient.Enroll : started");
            try
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["shortAudio"] = $"{true}";
                var requestUri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL + "/" + profile.IdentificationProfileId + "/enroll?" + queryString;
                var request = PrepareMediaRequest(audioFilePath, requestUri);
                var responseTask = client.SendAsync(request);
                responseTask.Wait();
                var response = responseTask.Result;
                if (!response.IsSuccessStatusCode)
                {
                    var resultTask = response.Content.ReadAsStringAsync();
                    resultTask.Wait();
                    throw JsonConvert.DeserializeObject<SpeakerRecognitionException>(resultTask.Result);
                }

                System.Collections.Generic.IEnumerable<string> operationTrackingHeaders = response.Headers.GetValues("Operation-Location");
                var trackingUrl = operationTrackingHeaders.FirstOrDefault();
                Task.Delay(1000);
                OperationStatus res = GetOperationStatus(trackingUrl);
                int request_trails = 1;
                while (res.Status == Status.running || res.Status == Status.notstarted)
                {
                    Task.Delay(2000);
                    res = GetOperationStatus(trackingUrl);
                    if (request_trails > 5)
                    {
                        break;
                    }
                }
                Trace.WriteLine("IdentityProfileClient.Enroll : Completed");
                return res;
            }
            catch (Exception e)
            {
                Debug.WriteLine("IdentiityProfileClient.OperationStatus : EXCEPTION: " + e.Message);
                throw;
            }
        }

        public async Task<OperationStatus> GetOperationStatusAsync(string uri)
        {
            try
            {
                Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
                responseTask.Wait();
                Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
                //string content = contentTask.Result;
                OperationStatus opStatus = JsonConvert.DeserializeObject<OperationStatus>(contentTask.Result);
                return opStatus;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public OperationStatus GetOperationStatus(string uri)
        {
            try
            {
                Task<HttpResponseMessage> responseTask = client.GetAsync(uri);
                responseTask.Wait();
                Task<string> contentTask = responseTask.Result.Content.ReadAsStringAsync();
                OperationStatus opStatus = JsonConvert.DeserializeObject<OperationStatus>(contentTask.Result);
                return opStatus;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw e;
            }
        }

        public async Task<OperationStatus> IdentifyProfile(List<Profile> profiles, string audioFilePath)
        {

            try
            {
                string commaDelimitedProfileIds = String.Join(",", profiles);
                var queryString = HttpUtility.ParseQueryString(commaDelimitedProfileIds);
                queryString["shortAudio"] = $"{true}";
                var requestUri = client.BaseAddress + AzCognitiveSpeakerAPI.SPEAKER_IDENTIFICATION_URL + "?identificationProfileIds" + queryString;
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
                OperationStatus res = await GetOperationStatusAsync(trackingUrl);
                int trials = 1;
                while (res.Status == Status.running || res.Status == Status.notstarted)
                {
                    await Task.Delay(3000);
                    res = await GetOperationStatusAsync(trackingUrl);
                    if (trials > 10)
                    {
                        break;
                    }
                    trials += 1;
                }
                return res;
            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION: " + e.Message);
                throw;
            }
        }

        public async Task<Boolean> DeleteProfile(Profile profile)
        {
            try
            {
                var requestUri = client.BaseAddress + AzCognitiveSpeakerAPI.IDENTITY_URL + "/" + profile.IdentificationProfileId;

                var responseTask = client.DeleteAsync(requestUri);
                responseTask.Wait();
                if (responseTask.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch( Exception e)
            {
                Debug.WriteLine("EXCEPTION: " + e.Message);
                throw;
            }
        }

        protected HttpRequestMessage PrepareMediaRequest(string audioFilePath, string requestUri)
        {
            Trace.WriteLine("IdentityProfileClient.PrepareMediaRequest : Started");
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.TransferEncodingChunked = true;
            request.Headers.ExpectContinue = true;
            request.Headers.Accept.ParseAdd(MimeTypes.Json);
            request.Headers.Accept.ParseAdd(MimeTypes.Xml);
            request.Content = MediaRequestHelper.PopulateRequestContent(audioFilePath);
            Trace.WriteLine("IdentityProfileClient.PrepareMediaRequest : Completed");
            return request;
        }

    }
}
