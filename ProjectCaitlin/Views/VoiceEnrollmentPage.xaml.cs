using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognition.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoiceEnrollmentPage : ContentPage
    {
        GreetingViewModel en;

        public VoiceEnrollmentPage()
        {
            InitializeComponent();
            en = new GreetingViewModel(Navigation);

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string name = Name.Text;
            string phoneNumber = PhoneNumber.Text;
            People people = new People()
            {
                FirstName = name,
                PhoneNumber = phoneNumber
            };
            //if in Azure but not in Firebase
            //People result = await en.AddFireBasePeople(people);

            //else
            if (GreetingViewModel.addFirebaseOnly)
            {
                People result = await en.AddFireBasePeople(people);
            }
            else
            {
                en.AzIdNotFound_createNewProfileBackground(people);
            }

            await Navigation.PopAsync();
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
}
}