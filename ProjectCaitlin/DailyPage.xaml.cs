using System;
using System.Collections.Generic;
using ProjectCaitlin.Services;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class DailyPage : ContentPage
    {
        GooglePhotoService GooglePhotoService = new GooglePhotoService();

        public DailyPage()
        {
            InitializeComponent();
            setupUI();
        }

        public void setupUI()
        {
            GooglePhotoService.GetPhotos();
            var image = new Image { Source = "waterfront.jpg" };
        }
    }
}
