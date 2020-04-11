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
        public TaskGridViewModel taskGridViewModel;

        public TaskPage(int routineNum, bool isRoutine)
        {
            InitializeComponent();

            if (!App.User.routines[routineNum].isComplete)
                App.User.routines[routineNum].isInProgress = true;

            taskGridViewModel = new TaskGridViewModel(this, routineNum, isRoutine);
            BindingContext = taskGridViewModel;

        }
        public async void close(object sender, EventArgs args)
        {
            if (App.ParentPage == "ListView")
                await Navigation.PushAsync(new ListViewPage());
            else
                await Navigation.PopAsync();
        }
    }

}
