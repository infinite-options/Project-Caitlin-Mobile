using System;
using System.Collections.Generic;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
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

            AddTapGestures();

            int rowLength = 3;
            double gridItemSize = (Application.Current.MainPage.Width / rowLength) - (1.2 * rowLength);

            controlGrid.RowDefinitions.Add(new RowDefinition { Height = gridItemSize });

            for (int i = 0; i < rowLength; i ++)
                controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = gridItemSize});

            var photoCount = 0;

            try
            {
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

                    var indicator = new ActivityIndicator { Color = Color.Gray, };
                    indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
                    indicator.BindingContext = webImage;

                    controlGrid.Children.Add(indicator, photoCount % rowLength, photoCount / rowLength);
                    controlGrid.Children.Add(webImage, photoCount % rowLength, photoCount / rowLength);
                    photoCount++;
                }
            }
            catch (NullReferenceException e)
            {
                var googleService = new GoogleService();
                await googleService.RefreshToken();
                SetupUI();
            }

            photoScrollView.Content = controlGrid;

        }

        private void AddTapGestures()
        {


            var tapGestureRecognizer1 = new TapGestureRecognizer();
            tapGestureRecognizer1.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GreetingPage());
            };
            AboutMeButton.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += async (s, e) => {
                await Navigation.PushAsync(new ListViewPage());
            };
            ListViewButton.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += async (s, e) => {
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
            };
            MyDayButton.GestureRecognizers.Add(tapGestureRecognizer3);
        }
    }
}
