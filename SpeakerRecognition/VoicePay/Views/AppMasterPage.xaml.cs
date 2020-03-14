using System;
using System.Collections.Generic;
using VoicePay.ViewModels;
using Xamarin.Forms;

namespace VoicePay.Views
{
    public partial class AppMasterPage : ContentPage
    {
        private AppMasterViewModel viewModel;
        public ListView ItemsListView;

        public AppMasterPage()
        {
            InitializeComponent();
            ItemsListView = MenuItemsListView;

            BindingContext = viewModel = new AppMasterViewModel();
        }
    }
}
