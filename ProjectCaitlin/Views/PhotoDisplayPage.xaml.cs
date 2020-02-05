using System;
using System.Collections.Generic;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
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

            int rowLength = 3;
            double gridItemSize = Application.Current.MainPage.Width / rowLength;

            var scrollView = new ScrollView();
            var controlGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = gridItemSize });

            for (int i = 0; i < rowLength; i ++)
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = gridItemSize });

            var photoCount = 0;
            foreach (string photoURI in photoURIs)
            {
                if (photoCount % rowLength == 0)
                {
                    controlGrid.RowDefinitions.Add(new RowDefinition { Height = gridItemSize });

                }
                CachedImage webImage = new CachedImage
                {
                    Source = Xamarin.Forms.ImageSource.FromUri(new Uri(photoURI)),
                    Transformations = new List<ITransformation>() {
                        new CropTransformation(),
                    },

                };

                controlGrid.Children.Add(webImage, photoCount % rowLength, photoCount / rowLength);
                photoCount++;
            }
            scrollView.Content = controlGrid;
            Content = scrollView;
        }
    }
}
