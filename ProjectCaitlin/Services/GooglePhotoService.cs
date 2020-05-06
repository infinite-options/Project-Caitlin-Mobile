using Newtonsoft.Json;
using ProjectCaitlin.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ProjectCaitlin.Models;
using ProjectCaitlin.Services;

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
                    string fileName = product.Filename;
                    storePicUri = product.BaseUrl.ToString();
                    description = product.Description + "";

                    App.User.allDates.Add(date);
                    subList.Add(product.BaseUrl.ToString());
                    subList.Add(date);
                    subList.Add(description);
                    subList.Add(creationTime);
                    subList.Add(id);
                    
                    bool post = true;
                    foreach (photo photo in App.User.FirebasePhotos)
                    {
                        if (id.Equals(photo.id))
                            post = false;
                    }

                    // If there is a photo in Google but not in Firebase, post it. 
                    if (post)
                    {
                        //Post photo to Firebase
                        await FirebaseFunctionsService.PostPhoto(id, description, " ");
                        subList.Add("");
                    }
                    else
                    {
                        //Get photo from Firebase and add note
                        photo photo = await FirebaseFunctionsService.GetPhoto(id);
                        subList.Add(photo.note);
                    }
                    App.User.photoURIs.Add(subList);
                    itemList.Add(subList);
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