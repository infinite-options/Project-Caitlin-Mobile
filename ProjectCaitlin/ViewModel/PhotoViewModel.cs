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

        public PhotoViewModel(CachedImage webImage, string date, string description, string creationTime, string note)
        {
            Items = new ObservableCollection<object>();
            Items.Add(new
            {
                Source = webImage.Source,
                Description = description,
                CreationTime = creationTime,
                Note = note
            }) ;
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
            try
            {
                
                foreach (List<string> list in App.User.photoURIs)
                {
                    string photoURI = list[0];
                    string photoDate = list[1];
                    string description = list[2];
                    string creationTime = list[3];
                    string note = list[5];

                    if (date.Equals(photoDate) && !(source.Equals(photoURI)))
                    {
                        Items.Add(new { Source = photoURI, Description = description, CreationTime = creationTime, Note = note});
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
            try
            {
                foreach (List<string> list in App.User.photoURIs)
                {
                    string photoURI = list[0];
                    string photoDate = list[1];
                    string description = list[2];
                    string creationTime = list[3];
                    string note = list[5];
                    if (date.Equals(photoDate))
                    {
                        Items.Add(new { Source = photoURI, Description = description, CreationTime = creationTime, Note = note });
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
