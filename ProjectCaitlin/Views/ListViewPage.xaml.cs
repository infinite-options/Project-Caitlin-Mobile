using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectCaitlin.Services;
//using ProjectCaitlin.ViewModel;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace ProjectCaitlin
{
    public partial class ListViewPage : ContentPage
    {
        private static List<string> eventNameList;
        public int oldDate;

        public int publicYear;
        public int publicMonth;
        public int publicDay;
        public int uTCHour;
        public int currentLocalUTCMinute;

        DateTime dateTimeNow;
        //public DailyViewModel dailyViewModel;

        public ListViewPage()
        {
            InitializeComponent();
            //BindingContext = DailyViewModel.Instance;
            PrepareRefreshEvents();
            SetupUI();
            //dailyViewModel = (DailyViewModel)BindingContext;           
        }

        void SetupUI()
        {

        }

        public async void PrepareRefreshEvents()
        {
            await Task.Delay(1000);
            dateTimeNow = DateTime.Now;
            await RefreshEvents();
        }

        public async Task RefreshEvents()
        {

        }
    }
}
