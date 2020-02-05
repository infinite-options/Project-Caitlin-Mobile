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
            var controlGrid = new Grid { RowSpacing = 1, ColumnSpacing = 1 };

            foreach (string photoURI in photoURIs)
            {
                controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                var webImage = new Image
                {
                    Source = ImageSource.FromUri(new Uri(photoURI))
                };

                controlGrid.Children.Add(webImage);

                layout.Children.Add(webImage);
            }
            scrollView.Content = controlGrid;
            Content = scrollView;
        }
    }
}
