using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GreetingPage : ContentPage
    {
        public GreetingPage()
        {
            InitializeComponent();
            var navigationPage = Application.Current.MainPage as NavigationPage;
            navigationPage.BarBackgroundColor = Color.FromHex("#E9E8E8");

        }

        async void btn1Clicked(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new GreetingPage());

        }

        async void btn2Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());

        }
    }
}