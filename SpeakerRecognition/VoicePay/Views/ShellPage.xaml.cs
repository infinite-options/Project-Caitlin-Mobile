using System;
using System.Collections.Generic;
using VoicePay.Helpers;
using VoicePay.ViewModels;
using Xamarin.Forms;

namespace VoicePay.Views
{
    public partial class ShellPage : MasterDetailPage
    {
        public ShellPage()
        {
            InitializeComponent();
            MasterPage.ItemsListView.ItemSelected += ItemsListView_ItemSelected;
            MasterBehavior = MasterBehavior.Popover;

            //Page detailPage;
            //if (Settings.Instance.Contains(Settings.EnrolledPhrase))
            //    detailPage = new Store.CategoriesPage();
            //else
                //detailPage = new Enrollment.WelcomePage();

            Detail = new NavigationPage(new Enrollment.WelcomePage());
        }

        protected override void OnAppearing()
        {
            BaseViewModel.Navigation = Navigation;
            BaseViewModel.MasterDetail = this;
        }

        void ItemsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageMenuItem;
            if (item == null)
                return;

            if (item.TargetType != null)
            {
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
            }

            IsPresented = false;
            MasterPage.ItemsListView.SelectedItem = null;
        }
    }

    public class MasterPageMenuItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Type TargetType { get; set; }
        public string IconPath { get; set; }
    }
}
