using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        readonly TaskGridViewModel pageModel;
        public TaskPage(int routineNum, bool isRoutine, List<bool> complete)
        {
            InitializeComponent();
            pageModel = new TaskGridViewModel(this, routineNum, isRoutine, complete);
            BindingContext = pageModel;

        }
        public async void close(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());
            //await Navigation.PushAsync(new GoalsRoutinesTemplate());
        }


    }
}