using DLToolkit.Forms.Controls;
using VoicePay.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace VoicePay
{
    
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            FlowListView.Init();
            //MainPage = new Views.Enrollment.WelcomePage();
            //MainPage = new NavigationPage(new Views.Store.CategoriesPage());

            MainPage = new Views.Enrollment.MainPage();

            //MainPage = new Views.Authentication.LoginPage();

        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
