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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
