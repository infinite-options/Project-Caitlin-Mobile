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
    public partial class StepsPageCopy : ContentPage
    {
        FirebaseFunctionsService firebaseFunctionsService;

        private int itemcount;
        int a;
        public int b;
        bool isRoutine;
        List<bool> complete;
        readonly StepsPageViewModelCopy pageModel;
        TaskItemModel taskItemModel;
        GRItemModel GRItemModel;
        public UpdateRoutineTaskParent updateParentTask;
        public UpdateParentRoutine updateParentRoutine;
        int routineNum;

        //public StepsPageCopy(int a, int b, bool isRoutine, TaskItemModel _taskItemModel, GRItemModel _GRItemModel)
        //{
        //    InitializeComponent();


        //    this.a = a;
        //    this.b = b;
        //    taskItemModel = _taskItemModel;
        //    GRItemModel = _GRItemModel;
        //    this.isRoutine = isRoutine;
        //    pageModel = new StepsPageViewModel(this, a, b, isRoutine);
        //    BindingContext = pageModel;
        //    itemcount = pageModel.count;
        //    StepListViewCopy.HeightRequest = GetListViewHeight();
        //}

        public StepsPageCopy(int routineNum, bool isRoutine, GRItemModel _GRItemModel)
        {
            InitializeComponent();

            GRItemModel = _GRItemModel;
            this.routineNum = routineNum;
            this.isRoutine = isRoutine;

            //taskGridViewModel = new TaskGridViewModel(this, routineNum, isRoutine, GRItemModel);
            //BindingContext = taskGridViewModel;
            pageModel = new StepsPageViewModelCopy(this, routineNum, 0, isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;
            StepListViewCopy.HeightRequest = GetListViewHeight();
        }

        public StepsPageCopy(int routineNum, bool isRoutine, GRItemModel _GRItemModel, UpdateParentRoutine updateParentRoutine) : this(routineNum, isRoutine, _GRItemModel)
        {
            this.updateParentRoutine = updateParentRoutine;
        }

        //public StepsPageCopy(int a, int b, bool isRoutine, TaskItemModel _taskItemModel, GRItemModel _GRItemModel, UpdateRoutineTaskParent updateParentTask) : this(a, b, isRoutine, _taskItemModel, _GRItemModel)
        //{
        //    this.updateParentTask = updateParentTask;
        //}

        private double GetListViewHeight()
        {
            double result = 0;
            foreach (task step in App.User.routines[routineNum].tasks)
            {
                result += 50 + (10 * step.title.Length / 40);

            }

            return result;
        }

        //private double GetListViewHeight()
        //{
        //    double result = 0;
        //    foreach (step step in App.User.routines[a].tasks[b].steps)
        //    {
        //        result += 50 + (10 * step.title.Length / 40);

        //    }

        //    return result;
        //}

        public async void close(object sender, EventArgs args)
        {
            Navigation.PopAsync();
            //Navigation.PopAsync();
        }

        public async void back(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

        public async void DoneClicked(object sender, EventArgs args)
        {
            DoneButtonCopy.IsEnabled = false;

            var completeCounter = 0;

            var completeTasksCounter = 0;

            foreach (task task in App.User.routines[routineNum].tasks)
            {
                if (task.isComplete)
                {
                    completeCounter++;
                }
            }

            if (completeCounter == App.User.routines[routineNum].tasks.Count)
            {
                DoneButtonCopy.IsEnabled = false;
                var routine = App.User.routines[routineNum];
                firebaseFunctionsService = new FirebaseFunctionsService();
                App.User.routines[routineNum].isComplete = true;
                App.User.routines[routineNum].isInProgress = false;
                App.User.routines[routineNum].dateTimeCompleted = DateTime.Now;

                await firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", true);
                await Navigation.PopAsync();
                updateParentRoutine?.Invoke();
            }
            else
            {
                DoneButtonCopy.IsEnabled = true;
                await DisplayAlert("Oops!", "Please complete all steps before marking this task as done", "OK");
            }
        }

        //public async void DoneClicked(object sender, EventArgs args)
        //{
        //    DoneButton.IsEnabled = false;

        //    var completeCounter = 0;

        //    var completeTasksCounter = 0;

        //    foreach (step step in App.User.routines[routineNum].tasks[b].steps)
        //    {
        //        if (step.isComplete)
        //        {
        //            completeCounter++;
        //        }
        //    }

        //    if (completeCounter == App.User.routines[a].tasks[b].steps.Count)
        //    {
        //        DoneButton.IsEnabled = false;
        //        var routineId = App.User.routines[a].id;
        //        var taskId = App.User.routines[a].tasks[b].id;

        //        var task = App.User.routines[a].tasks[b];

        //        firebaseFunctionsService = new FirebaseFunctionsService();

        //        taskItemModel.IsComplete = true;
        //        taskItemModel.IsInProgress = false;

        //        App.User.routines[a].tasks[b].isComplete = true;
        //        App.User.routines[a].tasks[b].isInProgress = false;
        //        App.User.routines[a].tasks[b].dateTimeCompleted = DateTime.Now;
        //        //TaskPage.pageModel.Items[b].IsComplete = true;

        //        firebaseFunctionsService.updateGratisStatus(task, "actions&tasks", true);
        //        await Navigation.PopAsync();
        //    }
        //    else
        //    {
        //        DoneButton.IsEnabled = true;
        //        await DisplayAlert("Oops!", "Please complete all steps before marking this task as done", "OK");
        //    }

        //    foreach (task task in App.User.routines[a].tasks)
        //    {
        //        if (task.isComplete)
        //        {
        //            completeTasksCounter++;
        //        }
        //    }

        //    if (completeTasksCounter == App.User.routines[a].tasks.Count)
        //    {
        //        var routineId = App.User.routines[a].id;
        //        var routine = App.User.routines[a];

        //        App.User.routines[a].isComplete = true;
        //        App.User.routines[a].isInProgress = false;
        //        App.User.routines[a].dateTimeCompleted = DateTime.Now;

        //        if (App.ParentPage != "ListView")
        //        {
        //            GRItemModel.IsComplete = true;
        //            GRItemModel.IsInProgress = false;
        //            GRItemModel.Text = "Done";
        //        }
                
        //        var firebaseFunctionsService = new FirebaseFunctionsService();

        //        firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", true);

        //        updateParentTask();
        //    }
        //}
    }
}
