using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using ProjectCaitlin.Services;
using ProjectCaitlin.Models;


namespace ProjectCaitlin.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskCompletePageCopy : ContentPage
    {
        FirebaseFunctionsService firebaseFunctionsService;
        GRItemModel GRItemModel;
        TaskItemModel TaskItemModel;

        private int itemcount;
        int a;
        int b;
        bool isRoutine;
        readonly TaskCompletePageViewModelCopy pageModel;
        UpdateParentGoalTask updateParentTask;
        int routineNum;

        //public TaskCompletePageCopy(int a, int b, bool isRoutine, TaskItemModel _TaskItemModel, GRItemModel _GRItemModel)
        //{
        //    InitializeComponent();
        //    firebaseFunctionsService = new FirebaseFunctionsService();

        //    GRItemModel = _GRItemModel;
        //    TaskItemModel = _TaskItemModel;
        //    this.a = a;
        //    this.b = b;
        //    this.isRoutine = isRoutine;
        //    pageModel = new TaskCompletePageViewModelCopy(this, a, b, isRoutine);
        //    BindingContext = pageModel;
        //    itemcount = pageModel.count;

        //}

        //public TaskCompletePageCopy(int a, int b, bool isRoutine, TaskItemModel _TaskItemModel, GRItemModel _GRItemModel, UpdateParentGoalTask updateParentTask) : this(a, b, isRoutine, _TaskItemModel, _GRItemModel)
        //{
        //    this.updateParentTask = updateParentTask;
        //}

        public TaskCompletePageCopy(int routineNum, bool isRoutine, GRItemModel _GRItemModel)
        {
            InitializeComponent();

            GRItemModel = _GRItemModel;
            this.routineNum = routineNum;
            a = routineNum;
            //taskGridViewModel = new TaskGridViewModel(this, routineNum, isRoutine, GRItemModel);
            //BindingContext = taskGridViewModel;
            this.isRoutine = isRoutine;
            pageModel = new TaskCompletePageViewModelCopy(this, a, b, isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;

        }

        public TaskCompletePageCopy(int routineNum, bool isRoutine, GRItemModel _GRItemModel, UpdateParentGoalTask updateParentTask) : this(routineNum, isRoutine, _GRItemModel)
        {
            this.updateParentTask = updateParentTask;
        }

        public async void nextpage(object sender, EventArgs args)
        {
            var completeActionCounter = 0;
            var goalId = App.User.goals[a].id;
            var actionId = App.User.goals[a].actions[b].id;

            if (next.Text == "Done")
            {
                var firestoreService = new FirestoreService();

                firebaseFunctionsService.updateGratisStatus(App.User.goals[a].actions[b], "actions&tasks", true);

                // Set data model completion status
                App.User.goals[a].actions[b].isComplete = true;
                App.User.goals[a].actions[b].isInProgress = false;
                TaskItemModel.IsComplete = true;
                TaskItemModel.IsInProgress = false;
                App.User.goals[a].actions[b].dateTimeCompleted = DateTime.Now;

                foreach (action action in App.User.goals[a].actions)
                {
                    if (action.isComplete)
                    {
                        completeActionCounter++;
                    }
                }

                if (completeActionCounter == App.User.goals[a].actions.Count)
                {
                    firebaseFunctionsService.updateGratisStatus(App.User.goals[a], "goals&routines", true);

                    // Set data model completion status
                    App.User.goals[a].isComplete = true;
                    App.User.goals[a].isInProgress = false;
                    if (App.ParentPage != "ListView")
                    {
                        GRItemModel.IsComplete = true;
                        GRItemModel.IsInProgress = false;
                        GRItemModel.Text = "Done";
                    }
                    App.User.goals[a].dateTimeCompleted = DateTime.Now;
                }

                await Navigation.PopAsync();
            }

            else if (next.Text == "Start")
            {
                int idx = 0;
                while (App.User.goals[a].actions[idx].isComplete)
                {
                    idx++;
                    Console.WriteLine("instruction complete idx: " + idx);
                }
                //App.user.goals[a].actions[b].instructions[0].isComplete = true;
                next.Text = "Next";
                CarouselTasks.Position = idx;
                Console.WriteLine("CarouselTasks.Position: " + CarouselTasks.Position);
                updateParentTask?.Invoke();
                //await Task.Delay(2000);
            }

            else if (CarouselTasks.Position != App.User.goals[a].actions.Count - 1)
            {
                action instruction = App.User.goals[a].actions[CarouselTasks.Position];

                App.User.goals[a].actions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;
                firebaseFunctionsService.updateGratisStatus(instruction, "actions&tasks", true);

                CarouselTasks.Position = CarouselTasks.Position + 1;
            }
            else if (CarouselTasks.Position == App.User.goals[a].actions.Count - 1)
            {
                action instruction = App.User.goals[a].actions[CarouselTasks.Position];

                App.User.goals[a].actions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;
                firebaseFunctionsService.updateGratisStatus(instruction, "actions&tasks", true);
                next.Text = "Done";

            }
            else if (CarouselTasks.Position != App.User.goals[a].actions.Count - 1)
            {
                action instruction = App.User.goals[a].actions[CarouselTasks.Position];

                App.User.goals[a].actions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;
                firebaseFunctionsService.updateGratisStatus(instruction, "actions&tasks", true);

                next.Text = "Done";

            }


        }

        public async void prepage(object sender, EventArgs args)
        {
            if (CarouselTasks.Position != 0)
            {
                next.Text = "Done";
            }
        }

        public async void close(object sender, EventArgs args)
        {
            Navigation.PopAsync();
            //Navigation.PopAsync();
        }

        public async void back(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}
