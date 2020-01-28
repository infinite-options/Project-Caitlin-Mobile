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
            string bearerString = string.Format("Bearer {0}", GoogleAuthenticator.superToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept" , "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();

            //Deserialize JSON Result
            var result = JsonConvert.DeserializeObject<Methods.GetCalendarsMethod>(json);

            //Create itemList
            var itemList = new List<string>();

            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            try
            {
                foreach (var sum in result.Items)
                {
                    itemList.Add(sum.Summary);
                }
            }
            catch(NullReferenceException e)
            {
                return null;
            }

            //Compile these values in to a string list and return to be displayed
            string itemListString = String.Join(", ", itemList);

            return itemListString;
        }

        public async Task<string> GetEventsList()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://www.googleapis.com/calendar/v3/calendars/primary/events?orderBy=startTime&singleEvents=true&timeMax=2020-01-22T23%3A59%3A59%2B00%3A00&timeMin=2020-01-22T00%3A00%3A00%2B00%3A00");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", GoogleAuthenticator.superToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();

            //Deserialize JSON Result
            var result = JsonConvert.DeserializeObject<Methods.GetEventsListMethod>(json);

            //Create itemList
            var itemList = new List<string>();
            //var itemList1 = new List<string>();
            //var itemList2 = new List<string>();

            //Create DataTable
            //DataTable table = new DataTable();

            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            try
            {
                foreach (var events in result.Items)
                {
                    itemList.Add(events.EventName);
                    itemList.Add(events.Start.DateTime.ToString());
                    itemList.Add(events.End.DateTime.ToString());
                }

                //foreach (var startTime in result.Items)
                //{
                //    itemList1.Add(startTime.Start.DateTime.ToString());
                //}

                //foreach (var endTime in result.Items)
                //{
                //    itemList1.Add(endTime.End.DateTime.ToString());
                //}
            }
            catch (NullReferenceException e)
            {
                return (null);
            }

            //Compile these values in to a string list and return to be displayed
            string eventNameString = String.Join(", ", itemList);
            //string startTimeString = String.Join(", ", itemList1);
            //string endTimeString = String.Join(", ", itemList2);

            //System.Diagnostics.Debug.WriteLine(itemListString);
            System.Diagnostics.Debug.WriteLine(eventNameString);

            //return (eventNameString, startTimeString, endTimeString);
            return (eventNameString);
        }

        public async Task<string> GetSpecificEventsList(int publicYear, int publicMonth, int publicDay)
        {

            //Make HTTP Request
            string baseUri = "https://www.googleapis.com/calendar/v3/calendars/primary/events?orderBy=startTime&singleEvents=true&";

            string monthString = "";

            if (publicMonth < 10)
            {
                monthString = publicMonth.ToString().PadLeft(2, '0');
                
            }

            string timeMaxMin = String.Format("timeMax={0}-{1}-{2}T23%3A59%3A59%2B00%3A00&timeMin={0}-{1}-{2}T00%3A00%3A00%2B00%3A00", publicYear, monthString, publicDay);

            string fullURI = baseUri + timeMaxMin;

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(fullURI);
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", GoogleAuthenticator.superToken);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();

            //Deserialize JSON Result
            var result = JsonConvert.DeserializeObject<Methods.GetEventsListMethod>(json);

            //Create itemList
            var itemList = new List<string>();


            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            try
            {
                foreach (var events in result.Items)
                {
                    itemList.Add(events.EventName);
                    itemList.Add(events.Start.DateTime.ToString());
                    itemList.Add(events.End.DateTime.ToString());
                }
            }
            catch (NullReferenceException e)
            {
                return (null);
            }

            string eventNameString = String.Join(", ", itemList);

            return (eventNameString);
        }
    }
}
