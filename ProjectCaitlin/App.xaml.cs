using System;
using ProjectCaitlin.Models;
using ProjectCaitlin.Methods;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using ProjectCaitlin.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Push;

namespace ProjectCaitlin
{
    public partial class App : Application
    {
        public static user User { get; set; } = new user();

        public static string ParentPage { get; set; } = "";

        public static bool IsPushNotifyEnabled { get; set; }

        public static double ListPageScrollPosY { get; set; } = -20;

        [assembly: XamlCompilation(XamlCompilationsOptions.Compile)]
        public App()
        {
            InitializeComponent();

            // use the dependency service to get a platform-specific implementation and initialize it
            DependencyService.Get<INotificationManager>().Initialize();

            MainPage = new NavigationPage(new LoginPage());

            //{
            //    BarBackgroundColor = Color.FromHex("#4682B4"),
            //    BarTextColor = Color.White
            //};

        }

        protected override void OnStart()
        {
            Microsoft.AppCenter.AppCenter.Start("ios=" + Constants.AppCenteriOSKey + ";" +
                  "uwp={Your UWP App secret here};" +
                  "android={Your Android App secret here}",
                  typeof(Analytics), typeof(Crashes), typeof(Push));

            IsPushNotifyEnabled = Push.IsEnabledAsync().Result;
            Console.WriteLine("IsPushNotifyEnabled: " + IsPushNotifyEnabled);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
