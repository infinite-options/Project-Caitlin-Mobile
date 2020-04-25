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
        List<List<string>> photoURIs = new List<List<string>>();
        GooglePhotoService GooglePhotoService = new GooglePhotoService();

        public PhotoViewModel(CachedImage webImage, string date, string description, string creationTime)
        {
            Items = new ObservableCollection<object>();
            Items.Add(new
            {
                Source = webImage.Source,
                Description = description,
                CreationTime = creationTime
            });
            string source = webImage.Source + "";
            source = source.Substring(5);
            SetupUI(date, source);
        }

        public PhotoViewModel(string date)
        {
            Items = new ObservableCollection<object>();
            SetupUI(date);
        }


        public async void SetupUI(string date, string source)
        {
            photoURIs = await GooglePhotoService.GetPhotos();

            try
            {
                foreach (List<string> list in photoURIs)
                {
                    string photoURI = list[0];
                    string photoDate = list[1];
                    string description = list[2];
                    string creationTime = list[3];


                    if (date.Equals(photoDate) && !(source.Equals(photoURI)))
                    {
                        Items.Add(new { Source = photoURI, Description = description, CreationTime = creationTime});
                    }

                }
            }
            catch (NullReferenceException e)
            {
                var googleService = new GoogleService();
                await googleService.RefreshToken();
            }
        }

        public async void SetupUI(string date)
        {
            //photoURIs = await GooglePhotoService.GetPhotos();

            try
            {
                foreach (List<string> list in photoURIs)
                {
                    string photoURI = list[0];
                    string photoDate = list[1];
                    string description = list[2];
                    string creationTime = list[3];

                    if (date.Equals(photoDate))
                    {
                        Items.Add(new { Source = photoURI, Description = description, CreationTime = creationTime});
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
