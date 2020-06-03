using ProjectCaitlin.ViewModel;
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

        //single person identified page
        /*public VoiceIdentificationPage(string n)
        {
            InitializeComponent();
            pageModel = new VoiceIdentificationViewModel(n);
            BindingContext = pageModel;
        }*/
    }
}