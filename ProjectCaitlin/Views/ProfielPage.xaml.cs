using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoiceRecognition.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfielPage : ContentPage
    {
        public string FirstName;
        public Boolean Important;
        public string PhoneNumber;
        public ProfielPage()
        {
            InitializeComponent();
        }
    }
}