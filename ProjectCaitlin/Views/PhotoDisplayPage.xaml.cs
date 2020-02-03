using System;
using System.Collections.Generic;
using ProjectCaitlin.Services;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class PhotoDisplayPage : ContentPage
    {
        GooglePhotoService GooglePhotoService = new GooglePhotoService();

        string[] photoURIs;

        public PhotoDisplayPage()
        {
            InitializeComponent();

            SetupUI();
        }

        private async void SetupUI()
        {
            photoURIs = await GooglePhotoService.GetPhotos();

            var layout = new StackLayout();
            var scrollView = new ScrollView();

            foreach (string photoURI in photoURIs)
            {
                var webImage = new Image
                {
                    Source = ImageSource.FromUri(new Uri(photoURI))
                };

                layout.Children.Add(webImage);
            }
            scrollView.Content = layout;
            Content = scrollView;
        }
    }
}
