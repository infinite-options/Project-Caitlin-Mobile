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
using ProjectCaitlin.Services;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskPage : ContentPage
    {
        public TaskGridViewModel taskGridViewModel;

        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        public TaskPage(int routineNum, bool isRoutine)
        {
            InitializeComponent();

            if (isRoutine)
            {
                string routineId = App.User.routines[routineNum].id.ToString();
                string routineDbIdx = App.User.routines[routineNum].dbIdx.ToString();

                if (!App.User.routines[routineNum].isComplete)
                {
                    App.User.routines[routineNum].isInProgress = true;
                    firebaseFunctionsService.startGR(routineId, routineDbIdx);
                }
            }
            else
            {
                string goalId = App.User.goals[routineNum].id.ToString();
                string goalDbIdx = App.User.goals[routineNum].dbIdx.ToString();

                if (!App.User.goals[routineNum].isComplete)
                {
                    App.User.goals[routineNum].isInProgress = true;
                    firebaseFunctionsService.startGR(goalId, goalDbIdx);
                }

            }

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
