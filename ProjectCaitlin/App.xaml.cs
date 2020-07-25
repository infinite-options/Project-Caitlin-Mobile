using System;
using ProjectCaitlin.Models;
using ProjectCaitlin.Methods;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using ProjectCaitlin.Views;
using ProjectCaitlin.Services;
//using Firebase.Iid;

namespace ProjectCaitlin
{
    public partial class App : Application
    {
        public static user User { get; set; } = new user();

        public static string ParentPage { get; set; } = "";

        public static bool IsPushNotifyEnabled { get; set; }

        public static double ListPageScrollPosY { get; set; } = -20;

        public static bool isAuthenticating { get; set; } = false;

        public static bool isFirstSetup { get; set; } = true;

        public static bool IsInForeground { get; set; } = false;

        public static string deviceToken { get; set; } = "";

        

        [assembly: XamlCompilation(XamlCompilationsOptions.Compile)]
        public App()
        {
            InitializeComponent();

            // use the dependency service to get a platform-specific implementation and initialize it
            DependencyService.Get<INotificationManager>().Initialize();

            if (Current.Properties.ContainsKey("access_token")
                && Current.Properties.ContainsKey("refreshToken")
                && Current.Properties.ContainsKey("user_id"))
                MainPage = new NavigationPage(new LoadingPage());
            else
                MainPage = new NavigationPage(new LoginPage());


            //{
            //    BarBackgroundColor = Color.FromHex("#4682B4"),
            //    BarTextColor = Color.White
            //};

        }

        public static void LoadApplicationProperties()
        {
            User.id = Current.Properties["user_id"].ToString();
            sendDeviceToken();
        }

        public static void sendDeviceToken()
        {
            new FirebaseFunctionsService().sendDeviceToken(App.User.id, App.deviceToken);
        }

        protected override void OnStart()
        {
            IsInForeground = true;
        }

        protected override void OnSleep()
        {
            IsInForeground = false;

        }

        protected override void OnResume()
        {
            IsInForeground = true;

        }
    }
}
