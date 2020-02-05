using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ProjectCaitlin;
using ProjectCaitlin.Authentication;

namespace ProjectCaitlin.Services
{
    public class GoogleService
    {

        public async Task<string> GetCalendars()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://www.googleapis.com/calendar/v3/users/me/calendarList");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", LoginPage.accessToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept" , "application/json");
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
            string bearerString = string.Format("Bearer {0}", LoginPage.accessToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
            return (json);
        }

        public async Task<string> GetSpecificEventsList(int publicYear, int publicMonth, int publicDay, int uTCHour, int currentLocalUTCMinute, int timeZoneNum)
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
                dayString = publicMonth.ToString();
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

            Console.WriteLine(fullURI);

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(fullURI);
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", LoginPage.accessToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();
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
                dayString = publicMonth.ToString();
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

            Console.WriteLine(fullURI);

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(fullURI);
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", LoginPage.accessToken);
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
