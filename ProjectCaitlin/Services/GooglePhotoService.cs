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
    public class GooglePhotoService
    {

        public async Task<string> GetPhotos()
        {

            //Make HTTP Request
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://photoslibrary.googleapis.com/v1/albums");
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
            var result = JsonConvert.DeserializeObject<Methods.GetPhotoAlbumMethod>(json);

            //Create itemList
            var itemList = new List<string>();
            String storeAlbumUri = "";
            String thumbNailAlbumUri = "";
            //Try to add "Summary" Items to list from JSON. If null, redirect to Login prompt.
            try
            {
                foreach (var product in result.Albums)
                {
                    itemList.Add(product.ProductUrl.ToString());
                    storeAlbumUri = product.ProductUrl.ToString();
                    thumbNailAlbumUri = product.CoverPhotoBaseUrl.ToString();
                }
            }
            catch (NullReferenceException e)
            {
                return null;
            }

            //Compile these values in to a string list and return to be displayed
            string itemListString = String.Join(", ", itemList);

            return storeAlbumUri;
        }
    }
}
