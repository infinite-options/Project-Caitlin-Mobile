
using Xamarin.Forms;
using VoicePay.ViewModels;
using VoicePay.Helpers;

namespace VoicePay.Views.Enrollment
{
    public partial class CorrectResultPage : ContentPage
    {
        public CorrectResultPage(string nameFromML)
        {
            InitializeComponent();
            NameLabel.Text = nameFromML;
            if (nameFromML == "John Baer")
            {
                ProfileImage.Source = "facePic.png";
            }
            else if(nameFromML == "Kyle Hoefer")
            {
                ProfileImage.Source = "kylePic.png";
            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new Views.Enrollment.MainPage();
            //await BaseViewModel.MasterDetail.Detail.Navigation.PopToRootAsync();
            //await BaseViewModel.MasterDetail.Detail.Navigation.PopModalAsync();
            //Cart.Instance.Clear();
        }
    }
}
