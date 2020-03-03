using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ProjectCaitlin;
using ProjectCaitlin.Authentication;
using ProjectCaitlin.Methods;
using Xamarin.Auth;
using Xamarin.Forms;
using Newtonsoft.Json.Linq;

namespace ProjectCaitlin.Services
{
    public class GoogleService
    {

        public INavigation Navigation;

        public async Task<string> SaveAccessTokenToFireBase(string accessToken)
        {

            //Make HTTP POST Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/SetUserGoogleAuthToken");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userID", "7R6hAVmDrNutRkG3sVRy");
            request.Headers.Add("token", accessToken);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            return json;
        }

        public async Task<string> SaveRefreshTokenToFireBase(string refreshToken)
        {

            //Make HTTP POST Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://us-central1-project-caitlin-c71a9.cloudfunctions.net/SetUserGoogleRefreshToken");
            request.Method = HttpMethod.Post;

            //Format Headers of Request with included Token
            request.Headers.Add("userID", "7R6hAVmDrNutRkG3sVRy");
            request.Headers.Add("token", refreshToken);
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            return json;
        }

        //Use REFRESH TOKEN to receive another ACCESS TOKEN...and UPDATE App.user.access_token.
        public async Task<string> RefreshToken()
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.iOSClientId;
                    redirectUri = Constants.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.AndroidClientId;
                    redirectUri = Constants.AndroidRedirectUrl;
                    break;
            }

            var values = new Dictionary<string, string> {
            { "refresh_token", App.User.refresh_token },
            { "client_id", clientId },
            { "grant_type", "refresh_token"}
            };

            var content = new FormUrlEncodedContent(values);

            Console.WriteLine(content);

            var client = new HttpClient();
            var response = await client.PostAsync(Constants.AccessTokenUrl, content);
            var json = await response.Content.ReadAsStringAsync();

            JObject jsonParsed = JObject.Parse(json);

            try
            {
                App.User.access_token = jsonParsed["access_token"].ToString();
            }
            catch (NullReferenceException e)
            {
                App.User.old_refresh_token = App.User.refresh_token;
                await Application.Current.MainPage.DisplayAlert("Alert", "Please re-login to continue", "OK");
                await Application.Current.MainPage.Navigation.PopToRootAsync();
            }

            await SaveAccessTokenToFireBase(App.User.access_token);

            Console.WriteLine(json);

            return json;

            //var request = new OAuth2Request("POST", new Uri(Constants.AccessTokenUrl), dictionary, e.Account);
            //var response = await request.GetResponseAsync();
            //return response.ToString();

        }

        public async Task<string> GetCalendars()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://www.googleapis.com/calendar/v3/users/me/calendarList");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", App.User.access_token);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            return json;
        }

        public async Task<string> GetEventsList()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events?orderBy=startTime&singleEvents=true&timeMax=2020-02-26T23%3A59%3A59%2B00%3A00&timeMin=2020-01-23T00%3A00%3A00%2B00%3A00");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", App.User.access_token);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            return (json);
        }

        public async Task<string> GetTodaysEventsList(int publicYear, int publicMonth, int publicDay, int uTCHour, int currentLocalUTCMinute, int timeZoneNum)
        {

            //Make HTTP Request
            string baseUri = "https://www.googleapis.com/calendar/v3/calendars/primary/events?orderBy=startTime&singleEvents=true&";

            string monthString;
            string dayString;
            string paddedHour;
            string paddedMinute;
            string paddedTimeZoneNum;


            //----------  ADD ZERO PADDING AND UTC FIX

            if (uTCHour < 0)
            {
                uTCHour = (24 + uTCHour);
                //publicDay = (publicDay - 1);
            }

            if (timeZoneNum < 10)
            {
                paddedTimeZoneNum = timeZoneNum.ToString().PadLeft(2, '0');

            }

            if (publicMonth < 10)
            {
                monthString = publicMonth.ToString().PadLeft(2, '0');

            }
            else
            {
                monthString = publicMonth.ToString();
            }

            if (publicDay < 10)
            {
                dayString = publicDay.ToString().PadLeft(2, '0');

            }
            else
            {
                dayString = publicDay.ToString();
            }

            if (uTCHour < 10)
            {
                paddedHour = uTCHour.ToString().PadLeft(2, '0');

            }
            else
            {
                paddedHour = uTCHour.ToString();
            }

            if (currentLocalUTCMinute < 10)
            {
                paddedMinute = currentLocalUTCMinute.ToString().PadLeft(2, '0');

            }
            else
            {
                paddedMinute = currentLocalUTCMinute.ToString();
            }

            //------------------------------

            string timeMaxMin = String.Format("timeMax={0}-{1}-{2}T23%3A59%3A59-08%3A00&timeMin={0}-{1}-{2}T{3}%3A{4}%3A00-08%3A00", publicYear, monthString, dayString, paddedHour, paddedMinute);

            string fullURI = baseUri + timeMaxMin;

            //Console.WriteLine(fullURI);

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(fullURI);
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", App.User.access_token);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            //Console.WriteLine(json);
            return (json);
        }

        public async Task<string> GetAllTodaysEventsList(int publicYear, int publicMonth, int publicDay, int timeZoneNum)
        {

            //Make HTTP Request
            string baseUri = "https://www.googleapis.com/calendar/v3/calendars/primary/events?orderBy=startTime&singleEvents=true&";

            string monthString;
            string dayString;
            string paddedTimeZoneNum;

            //----------  ADD ZERO PADDING AND UTC FIX


            if (timeZoneNum < 10)
            {
                paddedTimeZoneNum = timeZoneNum.ToString().PadLeft(2, '0');

            }
            else
            {
                paddedTimeZoneNum = timeZoneNum.ToString();
            }

            if (publicMonth < 10)
            {
                monthString = publicMonth.ToString().PadLeft(2, '0');

            }
            else
            {
                monthString = publicMonth.ToString();
            }

            if (publicDay < 10)
            {
                dayString = publicDay.ToString().PadLeft(2, '0');

            }
            else
            {
                dayString = publicDay.ToString();
            }

            string timeMaxMin = String.Format("timeMax={0}-{1}-{2}T23%3A59%3A59-{3}%3A00&timeMin={0}-{1}-{2}T00%3A00%3A01-{3}%3A00", publicYear, monthString, dayString, paddedTimeZoneNum);

            string fullURI = baseUri + timeMaxMin;

            //Console.WriteLine(fullURI);

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(fullURI);
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", App.User.access_token);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            //Console.WriteLine(json);

            JObject jsonParsed = JObject.Parse(json);

            if (jsonParsed.ContainsKey("error"))
            {
                var googleService = new GoogleService();
                await RefreshToken();
                return await googleService.GetAllTodaysEventsList(publicYear, publicMonth, publicDay, timeZoneNum);
            }

            return (json);
        }

        public async Task<string> GetListPageList(int publicYear, int publicMonth, int publicDay, int uTCHour, int currentLocalUTCMinute, int timeZoneNum)
        {

            //Make HTTP Request
            string baseUri = "https://www.googleapis.com/calendar/v3/calendars/primary/events?orderBy=startTime&singleEvents=true&";

            string monthString;
            string dayString;
            string paddedHour;
            string paddedMinute;
            string paddedTimeZoneNum;


            //----------  ADD ZERO PADDING AND UTC FIX

            if (uTCHour < 0)
            {
                uTCHour = (24 + uTCHour);
                //publicDay = (publicDay - 1);
            }

            if (timeZoneNum < 10)
            {
                paddedTimeZoneNum = timeZoneNum.ToString().PadLeft(2, '0');

            }

            if (publicMonth < 10)
            {
                monthString = publicMonth.ToString().PadLeft(2, '0');

            }
            else
            {
                monthString = publicMonth.ToString();
            }

            if (publicDay < 10)
            {
                dayString = publicDay.ToString().PadLeft(2, '0');

            }
            else
            {
                dayString = publicDay.ToString();
            }

            if (uTCHour < 10)
            {
                paddedHour = uTCHour.ToString().PadLeft(2, '0');

            }
            else
            {
                paddedHour = uTCHour.ToString();
            }

            if (currentLocalUTCMinute < 10)
            {
                paddedMinute = currentLocalUTCMinute.ToString().PadLeft(2, '0');

            }
            else
            {
                paddedMinute = currentLocalUTCMinute.ToString();
            }

            //------------------------------

            string timeMaxMin = String.Format("timeMax={0}-03-{2}T23%3A59%3A59-08%3A00&timeMin={0}-{1}-{2}T{3}%3A{4}%3A00-08%3A00", publicYear, monthString, dayString, paddedHour, paddedMinute);

            string fullURI = baseUri + timeMaxMin;

            //Console.WriteLine(fullURI);

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(fullURI);
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", App.User.access_token);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            return (json);

        }
    }
}