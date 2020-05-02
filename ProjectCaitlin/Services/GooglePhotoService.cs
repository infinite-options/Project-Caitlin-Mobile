﻿using Newtonsoft.Json;
using ProjectCaitlin.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectCaitlin.Services
{
    public class GooglePhotoService
    {
        public static async Task<List<List<string>>> GetPhotos()
        {
            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://photoslibrary.googleapis.com/v1/mediaItems?pageSize=100");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", App.User.access_token);
            request.Headers.Add("Authorization", bearerString);
            request.Headers.Add("Accept", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            HttpContent content = response.Content;
            var json = await content.ReadAsStringAsync();

            //return json;
            //Deserialize JSON Result
            var result = JsonConvert.DeserializeObject<ProjectCaitlin.Methods.GetPhotoAlbumMethod>(json);
            //Create itemList
            var itemList = new List<List<string>>();
            //var itemMap = new Dictionary<string, string>();

            String creationTime = "";
            String storePicUri = "";
            String date = "";
            String thumbNailAlbumUri = "";
            String description = "";
            String id = "";
            //String note = "";
            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            Console.WriteLine("request.RequestUri : " + request.RequestUri);

            Console.WriteLine("NextPageToken" + result.NextPageToken);
            /* if (!result.NextPageToken.Equals("")) {
                 request.RequestUri += "?" + result.NextPageToken;
             }*/
            Console.WriteLine("GetPhoto being called.");
            try
            {
                foreach (var product in result.MediaItems)
                {
                    var subList = new List<string>();

                    DateTimeOffset GreenwichMeanTime = product.MediaMetadata.CreationTime; //Google photo api sends time in GreenwichMeanTime. 
                    DateTimeOffset utcTime = GreenwichMeanTime.ToLocalTime();  //convert GreenwichMeanTime to local time.

                    //creationTime = utcTime.ToString();
                    creationTime = utcTime.ToString();
                    date = creationTime.Substring(0, creationTime.IndexOf(" "));// date = yyyy/mm/dd
                    creationTime = utcTime.TimeOfDay.ToString();
                    id = product.Id;
                    storePicUri = product.BaseUrl.ToString();
                    description = product.Description + "";

                    subList.Add(product.BaseUrl.ToString());
                    subList.Add(date);
                    subList.Add(description);
                    subList.Add(creationTime);
                    subList.Add(id);

                    itemList.Add(subList);

                    App.User.allDates.Add(date);

                    
                    App.User.photoURIs.Add(subList);

                    //await FirebaseFunctionsService.PostPhoto(id, " ", " ");

                }
            }
            catch (NullReferenceException e)
            {
                
                var googleService = new GoogleService();
                if (await googleService.RefreshToken())
                {
                    Console.WriteLine("RefreshToken Done!");
                    App.User.photoURIs = await GooglePhotoService.GetPhotos();
                }
               
            }
            if (itemList.Count == 0)
                return new List<List<string>>();
            else
                return itemList;
        }

        
    }
}