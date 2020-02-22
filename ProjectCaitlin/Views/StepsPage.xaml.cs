using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    public partial class StepsPage : ContentPage
    {
        private int itemcount;
        int a;
        int b;
        bool isRoutine;
        List<bool> complete;
        readonly StepsPageViewModel pageModel;

        public StepsPage(int a, int b, bool isRoutine, List<bool> complete)
        {
            InitializeComponent();
            this.complete = complete;


            this.a = a;
            this.b = b;
            this.isRoutine = isRoutine;
            pageModel = new StepsPageViewModel(this, a, b, isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;
        }

        public async void close(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}
