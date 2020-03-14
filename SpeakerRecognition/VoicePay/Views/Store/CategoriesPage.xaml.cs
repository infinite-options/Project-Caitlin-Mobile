using System;
using System.Collections.Generic;
using VoicePay.ViewModels.Store;
using Xamarin.Forms;

namespace VoicePay.Views.Store
{
    public partial class CategoriesPage : ContentPage
    {
        private CategoriesViewModel viewModel;
        public CategoriesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new CategoriesViewModel();
        }

        protected async override void OnAppearing()
        {
            await viewModel.OnAppearing();
        }
    }
}
