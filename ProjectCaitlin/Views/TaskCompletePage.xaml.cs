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
    public partial class TaskCompletePage : ContentPage
    {
        FirebaseFunctionsService firebaseFunctionsService;
        GRItemModel GRItemModel;
        TaskItemModel TaskItemModel;

        private int itemcount;
        int a;
        int b;
        bool isRoutine;
        readonly TaskCompletePageViewModel pageModel;
        public TaskCompletePage(int a, int b, bool isRoutine, TaskItemModel _TaskItemModel, GRItemModel _GRItemModel)
        {
            InitializeComponent();
            firebaseFunctionsService = new FirebaseFunctionsService();

            GRItemModel = _GRItemModel;
            TaskItemModel = _TaskItemModel;
            this.a = a;
            this.b = b;
            this.isRoutine = isRoutine;
            pageModel = new TaskCompletePageViewModel(this, a, b, isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;


        }
        public async void nextpage(object sender, EventArgs args)
        {
            var completeActionCounter = 0;
            var goalId = App.User.goals[a].id;
            var actionId = App.User.goals[a].actions[b].id;

            if (next.Text == "Done")
            {
                var firestoreService = new FirestoreService();

                firebaseFunctionsService.UpdateTask(goalId, actionId, App.User.goals[a].actions[b].dbIdx.ToString());

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
                    firebaseFunctionsService.CompleteRoutine(goalId, App.User.goals[a].dbIdx.ToString());

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
                while (App.User.goals[a].actions[b].instructions[idx].isComplete)
                {
                    idx++;
                    Console.WriteLine("instruction complete idx: " + idx);
                }
                //App.user.goals[a].actions[b].instructions[0].isComplete = true;
                next.Text = "Next";
                CarouselTasks.Position = idx;
                Console.WriteLine("CarouselTasks.Position: " + CarouselTasks.Position);
                await Task.Delay(2000);
            }

            else if (CarouselTasks.Position != App.User.goals[a].actions[b].instructions.Count - 1)
            {

                /*var firestoreService = new FirestoreService(App.User.id);
                var goalId = App.user.goals[a].id;
                var actionId = App.user.goals[a].actions[b].id;
                var isInstructionComplete = await firestoreService.UpdateInstruction(goalId, actionId, App.user.goals[a].actions[b].instructions[CarouselTasks.Position].dbIdx.ToString());
                if (isInstructionComplete)
                {
                    App.user.goals[a].actions[b].instructions[CarouselTasks.Position].isComplete = true;
                    App.user.goals[a].actions[b].instructions[CarouselTasks.Position].dateTimeCompleted = DateTime.Now;
                }*/
                App.User.goals[a].actions[b].instructions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;
                firebaseFunctionsService.UpdateInstruction(goalId, actionId, App.User.goals[a].actions[b].instructions[CarouselTasks.Position].dbIdx.ToString());

                CarouselTasks.Position = CarouselTasks.Position + 1;
            }
            else if (CarouselTasks.Position == App.User.goals[a].actions[b].instructions.Count - 1)
            {

                App.User.goals[a].actions[b].instructions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;
                firebaseFunctionsService.UpdateInstruction(goalId, actionId, App.User.goals[a].actions[b].instructions[CarouselTasks.Position].dbIdx.ToString());

                next.Text = "Done";

            }
            else if (CarouselTasks.Position != App.User.goals[a].actions[b].instructions.Count - 1)
            {

                App.User.goals[a].actions[b].instructions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;
                                firebaseFunctionsService.UpdateInstruction(goalId, actionId, App.User.goals[a].actions[b].instructions[CarouselTasks.Position].dbIdx.ToString());


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
            Navigation.PopAsync();
        }

        public async void back(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }
    }
}
