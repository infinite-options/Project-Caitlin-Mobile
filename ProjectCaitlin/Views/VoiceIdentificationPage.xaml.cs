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
    public partial class VoiceIdentificationPage : ContentPage
    {
        readonly VoiceIdentificationViewModel pageModel;


        //Identified person list page
        public VoiceIdentificationPage()
        {
            InitializeComponent();
            pageModel = new VoiceIdentificationViewModel();
            BindingContext = pageModel;
        }

        public VoiceIdentificationPage(People people)
        {
            InitializeComponent();
            pageModel = new VoiceIdentificationViewModel(people);
            BindingContext = pageModel;
        }

        private async void okButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GreetingPage());
        }
    }
}