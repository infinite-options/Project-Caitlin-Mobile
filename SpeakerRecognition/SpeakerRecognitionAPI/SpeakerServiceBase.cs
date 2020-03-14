using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpeakerRecognitionAPI.Constants;
using SpeakerRecognitionAPI.Helpers;
using SpeakerRecognitionAPI.Models;

namespace SpeakerRecognitionAPI
{
    public class SpeakerServiceBase
    {
        //public static string str1;
        protected readonly HttpClient _httpClient;
        protected readonly string _subscriptionKey;

        public SpeakerServiceBase(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
        }

        /// <summary>
        /// Creates an speaker profile for verification or identification with the specified locale.
        /// </summary>
        /// <returns>Profile response that contains the id of the created speaker identification profile.</returns>
        /// <param name="verification">If set to <c>true</c>: verification, false: identification.</param>
        /// <param name="locale">Locale.</param>
        public async Task<string> CreateProfileAsync(bool verification, string locale = "en-us")
        {
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("locale", locale)
            });
            var requestUri = verification ? Endpoints.VerificationCreateProfile.ToString() :
                                            Endpoints.IdentificationCreateProfile.ToString();

            //Debug.WriteLine(requestUri);

            try
            {
                var response = await _httpClient.PostAsync(requestUri, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                //var str = jsonResponse.Substring(32);
                //var str1 =  str.Substring(0, 38);
                //StoreString(str1);
                //Debug.WriteLine(response);
                //Debug.WriteLine(jsonResponse);
                //Debug.WriteLine(str);
                //Debug.WriteLine(str1);
                //Debug.WriteLine(jsonResponse.Substring(33, 69));
                if (!response.IsSuccessStatusCode)
                    throw BuildErrorFromServiceResult(jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION: " + ex.Message);
                throw;
            }
        }

        //public void StoreString(string stringIDs)
        //{
        //    str1 = stringIDs;
        //    Debug.WriteLine(str1);
        //}

        protected SpeakerRecognitionException BuildErrorFromServiceResult(string result, string errorMessage = "Error sending request")
        {
            var errorResponse = JsonConvert.DeserializeObject<ServiceError>(result);
            var ex = new SpeakerRecognitionException(errorMessage)
            {
                DetailedError = errorResponse.Error
            };
            return ex;
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
