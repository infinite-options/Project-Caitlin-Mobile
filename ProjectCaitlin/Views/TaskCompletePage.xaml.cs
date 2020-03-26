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

        private int itemcount;
        int a;
        int b;
        bool isRoutine;
        readonly TaskCompletePageViewModel pageModel;
        public TaskCompletePage(int a, int b, bool isRoutine)
        {
            InitializeComponent();
            firebaseFunctionsService = new FirebaseFunctionsService();

            this.a = a;
            this.b = b;
            this.isRoutine = isRoutine;
            pageModel = new TaskCompletePageViewModel(this, a, b, isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;


        }
        public async void nextpage(object sender, EventArgs args)
        {
            if (next.Text == "Done")
            {
                var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

                var completeActionCounter = 0;
                var goalId = App.User.goals[a].id;
                var actionId = App.User.goals[a].actions[b].id;

                var isActionComplete = await firebaseFunctionsService.UpdateTask(goalId, actionId, App.User.goals[a].actions[b].dbIdx.ToString());
                if (isActionComplete)
                {
                    App.User.goals[a].actions[b].isComplete = true;
                    App.User.goals[a].actions[b].dateTimeCompleted = DateTime.Now;
                }

                foreach (action action in App.User.goals[a].actions)
                {
                    if (action.isComplete)
                    {
                        completeActionCounter++;
                    }
                }

                if (completeActionCounter == App.User.goals[a].actions.Count)
                {
                    var isGoalComplete = await firebaseFunctionsService.CompleteRoutine(goalId, App.User.goals[a].dbIdx.ToString());
                    if (isGoalComplete)
                    {
                        App.User.goals[a].isComplete = true;
                        App.User.goals[a].dateTimeCompleted = DateTime.Now;
                    }
                }

                await Navigation.PushAsync(new TaskPage(a, isRoutine));
            }

            else if (next.Text == "Start")
            {
                CarouselTasks.Position = 0;
                //App.user.goals[a].actions[b].instructions[0].isComplete = true;
                next.Text = "Next";
            }
            else if (CarouselTasks.Position != App.User.goals[a].actions[b].instructions.Count - 1)
            {

                /*var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");
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

                CarouselTasks.Position = CarouselTasks.Position + 1;
            }
            else if (CarouselTasks.Position == App.User.goals[a].actions[b].instructions.Count - 1)
            {

                App.User.goals[a].actions[b].instructions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;

                next.Text = "Done";

            }
            else if (CarouselTasks.Position != App.User.goals[a].actions[b].instructions.Count - 1)
            {

                App.User.goals[a].actions[b].instructions[CarouselTasks.Position].isComplete = true;
                pageModel.Items[CarouselTasks.Position].OkToCheckmark = true;

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
