using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using ProjectCaitlin.Services;
using FFImageLoading.Transformations;
using FFImageLoading.Work;

namespace ProjectCaitlin.ViewModel
{
    public class PhotoViewModel : BindableObject
    {
        public ObservableCollection<object> Items { get; set; }
        Dictionary<string,string> photoURIs = new Dictionary<string,string>();
        GooglePhotoService GooglePhotoService = new GooglePhotoService();

        public PhotoViewModel(CachedImage webImage, string date)
        {
            Items = new ObservableCollection<object>();
            Items.Add(new
            {
                Source = webImage.Source,
            });
            string source = webImage.Source +"";
            source = source.Substring(5);
            SetupUI(date, source);
        }

        public async void SetupUI(string date, string source)
        {
            photoURIs = await GooglePhotoService.GetPhotos();

            try
            {
                foreach (var pair in photoURIs)
                {
                    string photoURI = pair.Key;
                    string photoDate = pair.Value + "";
                    if (date.Equals(photoDate) && !(source.Equals(photoURI))) {
                        Items.Add(new { Source = photoURI });
                        Console.WriteLine("Source: "+source);
                        Console.WriteLine("photoURI: " + photoURI);
                        Console.WriteLine(source.Equals(photoURI));
                    }
                    
                }
            }
            catch (NullReferenceException e)
            {
                var googleService = new GoogleService();
                await googleService.RefreshToken();
            }


        }
    }
}
