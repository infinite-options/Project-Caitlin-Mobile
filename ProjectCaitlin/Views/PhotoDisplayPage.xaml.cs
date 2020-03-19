using System;
using System.Collections.Generic;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
using Xamarin.Forms;
using ProjectCaitlin.ViewModel;

namespace ProjectCaitlin
{
    public partial class PhotoDisplayPage : ContentPage
    {
        readonly PhotoViewModel pageModel;

        GooglePhotoService GooglePhotoService = new GooglePhotoService();

        List<string> photoURIs = new List<string>();

        public PhotoDisplayPage(CachedImage webImage,string date)
        {
            InitializeComponent();
            pageModel = new PhotoViewModel(webImage,date);
            BindingContext = pageModel;

            dateLabel.Text = date;
        }

    }
}
