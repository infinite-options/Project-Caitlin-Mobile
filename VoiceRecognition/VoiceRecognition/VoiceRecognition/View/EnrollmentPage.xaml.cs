using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        readonly EnrollVoice en;
        public EnrollmentPage()
        {
            InitializeComponent();
            en = new EnrollVoice(Navigation);
            BindingContext = en;
        }

        string tempAzSpeakerId { get; set; }

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
            en.DisplayProfile = false;
            Name.Text = "";
            PhoneNumber.Text = "";
            try
            {
                _ = await en.AddFireBasePeople(people);
            }
            catch(FireBaseFailureException fbe)
            {
                en.Message = "Call to Firebase failed:\nCode"+fbe.Code+"\nMessage: "+fbe.Message;
            }
            catch (Exception exp)
            {
                en.Message = "Something went wrong: Message: "+exp.Message;
            }
        }

        private void CreateNewUser(object sender, EventArgs e)
        {
            string name = Name.Text;
            string phoneNumber = PhoneNumber.Text;
            People people = new Model.People()
            {
                FirstName = name,
                PhoneNumber = phoneNumber,
                Important = IsImportant.IsEnabled
            };
            en.DisplayForm = false;
            en.DisplayProfile = false;
            Name.Text = "";
            PhoneNumber.Text = "";
            en.Message = "Creating New User";
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