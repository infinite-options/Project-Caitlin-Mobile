using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectCaitlin.Services;
//using ProjectCaitlin.ViewModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using ProjectCaitlin.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ProjectCaitlin.Views;

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

            /*labelFont = Device.RuntimePlatform == Device.iOS ? "Lobster-Regular" :
                Device.RuntimePlatform == Device.Android ? "Lobster-Regular.ttf#Lobster-Regular" : "Assets/Fonts/Lobster-Regular.ttf#Lobster";*/

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
                    Padding = new Thickness(10, 10, 10, 10),
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
                    Text = "Takes me " + 5.ToString() + " minutes",
                    FontSize = 10,
                    TextColor = Color.DimGray,
                    VerticalOptions = LayoutOptions.EndAndExpand,
                    FontFamily = labelFont

                };

                CachedImage image = new CachedImage()
                {
                    Source = routine.photo,
                    WidthRequest = 50,
                    HeightRequest = 50,
                    HorizontalOptions = LayoutOptions.End,
                    Transformations = new List<ITransformation>()
                    {
                        new CircleTransformation(),
                    },
                    };

                stackLayoutInner.Children.Add(routineTitleLabel);
                stackLayoutInner.Children.Add(expectedTimeLabel);

                stackLayoutOuter.Children.Add(stackLayoutInner);
                stackLayoutOuter.Children.Add(image);

                frame.Content = stackLayoutOuter;

                MorningREStackLayout.Children.Add(frame);
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

        public async void btn1(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GreetingPage());
        }
        
        public async void btn3(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new PhotoDisplayPage());
        }
        public async void btn4(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());
        }
    }
}
