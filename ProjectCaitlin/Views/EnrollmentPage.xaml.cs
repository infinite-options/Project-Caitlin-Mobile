using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoiceRecognition.Model;
using VoiceRecognition.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoiceRecognition.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnrollmentPage : ContentPage
    {
        EnrollVoice en;
        public EnrollmentPage()
        {
            InitializeComponent();
            en = new EnrollVoice(Navigation);
            BindingContext = en;
        }

        string tempAzSpeakerId { get; set; }
        // Simply add the user to firebase
        private async void AddFireBasePeople(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new PeoplePage());
            string name = Name.Text;
            string phoneNumber = PhoneNumber.Text;
            People people = new Model.People()
            {
                FirstName = name,
                PhoneNumber = phoneNumber
            };
            en.DisplayForm = false;
            People result = await en.AddFireBasePeople(people);
        }

        // Enroll User to Azure then add it to firebase
        private void CreateNewUser(object sender, EventArgs e)
        {
            string name = Name.Text;
            string phoneNumber = PhoneNumber.Text;
            People people = new Model.People()
            {
                FirstName = name,
                PhoneNumber = phoneNumber
            };
            en.DisplayForm = false;
            en.AzIdNotFound_createNewProfileBackground(people);
        }

        private void Tic(object sender, EventArgs e)
        {
            en.Message = "Tic";
        }

        private void Toc(object sender, EventArgs e)
        {
            en.Message = "Toc";
        }
    }
}