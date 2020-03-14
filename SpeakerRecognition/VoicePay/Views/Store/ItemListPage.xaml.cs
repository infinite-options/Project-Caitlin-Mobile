using System;
using System.Collections.Generic;
using VoicePay.Models;
using VoicePay.ViewModels.Store;
using Xamarin.Forms;

namespace VoicePay.Views.Store
{
    public partial class ItemListPage : ContentPage
    {
        ItemListViewModel viewModel;
        public ItemListPage(string categoryName)
        {
            InitializeComponent();
            Title = categoryName;
            BindingContext = viewModel = new ItemListViewModel(categoryName);
        }

        protected async override void OnAppearing()
        {
            await viewModel.OnAppearing();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await viewModel.GoToReview(e.Item as Item);
        }
    }
}
