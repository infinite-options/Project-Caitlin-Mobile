using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectCaitlin.Services;
//using ProjectCaitlin.ViewModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using ProjectCaitlin.Models;

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

        string labelFont;

        user user;
        //public DailyViewModel dailyViewModel;

        public ListViewPage()
        {
            InitializeComponent();

            labelFont = Device.RuntimePlatform == Device.iOS ? "Lobster-Regular" :
                Device.RuntimePlatform == Device.Android ? "Lobster-Regular.ttf#Lobster-Regular" : "Assets/Fonts/Lobster-Regular.ttf#Lobster";

            user = App.user;

            //BindingContext = DailyViewModel.Instance;
            PrepareRefreshEvents();
            SetupUI();
            //dailyViewModel = (DailyViewModel)BindingContext;           
        }

        void SetupUI()
        {
            MorningRELoad();

        }

        private void MorningRELoad()
        {
            foreach (routine routine in user.routines)
            {
                Frame frame = new Frame
                {
                    CornerRadius = 10,
                    HasShadow = false,
                    Padding = new Thickness(10, 10, 5, 10),
                    Margin = new Thickness(0, 2, 0, 2)
                };

                StackLayout stackLayoutOuter = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal
                };

                StackLayout stackLayoutInner = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.StartAndExpand,

                };

                Label routineTitleLabel = new Label
                {
                    Text = routine.title,
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    FontFamily = labelFont

                };

                Label expectedTimeLabel = new Label
                {
                    Text = routine.title,
                    FontSize = 20,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    FontFamily = labelFont

                };

            }
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
