using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using Xamarin.Forms.Xaml;
using ProjectCaitlin.Services;

namespace ProjectCaitlin.Views
{
    public partial class StepsPage : ContentPage
    {
        FirebaseFunctionsService firebaseFunctionsService;

        private int itemcount;
        int a;
        public int b;
        bool isRoutine;
        List<bool> complete;
        readonly StepsPageViewModel pageModel;
        TaskItemModel taskItemModel;
        GRItemModel GRItemModel;

        public StepsPage(int a, int b, bool isRoutine, TaskItemModel _taskItemModel, GRItemModel _GRItemModel)
        {
            InitializeComponent();


            this.a = a;
            this.b = b;
            taskItemModel = _taskItemModel;
            GRItemModel = _GRItemModel;
            this.isRoutine = isRoutine;
            pageModel = new StepsPageViewModel(this, a, b, isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;
            StepListView.HeightRequest = GetListViewHeight();
        }

        private double GetListViewHeight()
        {
            double result = 0;
            foreach (step step in App.User.routines[a].tasks[b].steps)
            {
                result += 50 + (10 * step.title.Length / 40);

            }

            return result;
        }

        public async void close(object sender, EventArgs args)
        {
            Navigation.PopAsync();
            Navigation.PopAsync();
        }

        public async void back(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

        public async void DoneClicked(object sender, EventArgs args)
        {
            DoneButton.IsEnabled = false;

            var completeCounter = 0;

            var completeTasksCounter = 0;

            foreach (step step in App.User.routines[a].tasks[b].steps)
            {
                if (step.isComplete)
                {
                    completeCounter++;
                }
            }

            if (completeCounter == App.User.routines[a].tasks[b].steps.Count)
            {
                DoneButton.IsEnabled = false;
                var routineId = App.User.routines[a].id;
                var taskId = App.User.routines[a].tasks[b].id;

                firebaseFunctionsService = new FirebaseFunctionsService();

                taskItemModel.IsComplete = true;
                taskItemModel.IsInProgress = false;

                App.User.routines[a].tasks[b].isComplete = true;
                App.User.routines[a].tasks[b].isInProgress = false;
                App.User.routines[a].tasks[b].dateTimeCompleted = DateTime.Now;
                //TaskPage.pageModel.Items[b].IsComplete = true;

                firebaseFunctionsService.UpdateTask(routineId, taskId, App.User.routines[a].tasks[b].dbIdx.ToString());
                await Navigation.PopAsync();
            }
            else
            {
                DoneButton.IsEnabled = true;
                await DisplayAlert("Oops!", "Please complete all steps before marking this task as done", "OK");
            }

            foreach (task task in App.User.routines[a].tasks)
            {
                if (task.isComplete)
                {
                    completeTasksCounter++;
                }
            }

            if (completeTasksCounter == App.User.routines[a].tasks.Count)
            {
                var routineId = App.User.routines[a].id;

                App.User.routines[a].isComplete = true;
                App.User.routines[a].isInProgress = false;
                App.User.routines[a].dateTimeCompleted = DateTime.Now;

                if (App.ParentPage != "ListView")
                {
                    GRItemModel.IsComplete = true;
                    GRItemModel.IsInProgress = false;
                    GRItemModel.Text = "Done";
                }
                
                var firebaseFunctionsService = new FirebaseFunctionsService();

                var okToCheckmark = await firebaseFunctionsService.CompleteRoutine(App.User.routines[a]);
            }
        }
    }
}
