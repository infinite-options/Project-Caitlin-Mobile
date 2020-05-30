using System;
using ProjectCaitlin.Models;
using ProjectCaitlin.Methods;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using ProjectCaitlin.Views;


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
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
