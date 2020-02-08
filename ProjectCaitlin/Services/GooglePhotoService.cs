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

        public async Task<string[]> GetPhotos()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://photoslibrary.googleapis.com/v1/mediaItems");
            request.Method = HttpMethod.Get;

            //Format Headers of Request with included Token
            string bearerString = string.Format("Bearer {0}", LoginPage.accessToken);
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
            var itemList = new List<string>();
            String creationTime = "";
            String storePicUri = "";
            String date = "";
            String thumbNailAlbumUri = "";
            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            try
            {
                foreach (var product in result.MediaItems)
                {
                    //thumbNailAlbumUri = product.baseUrl.ToString();
                    creationTime = product.MediaMetadata.CreationTime.ToString();
                    date = creationTime.Substring(0, 9);
                    //string datePicker = DateTime.Now.ToString("dd'/'MM'/'yyyy");
                    string datePicker = "5/26/2016";
                    //if (date == datePicker)
                    //{
                    itemList.Add(product.BaseUrl.ToString());
                    storePicUri = product.BaseUrl.ToString();
                    //System.Diagnostics.Debug.WriteLine(storePicUri);
                    //System.Diagnostics.Debug.WriteLine(date);

                    //};
                }
            }
            catch (NullReferenceException e)
            {
                return null;
            }

            //Compile these values in to a string list and return to be displayed
            string itemListString = String.Join(", ", itemList);

            return itemList.ToArray();
        }
    }
}
