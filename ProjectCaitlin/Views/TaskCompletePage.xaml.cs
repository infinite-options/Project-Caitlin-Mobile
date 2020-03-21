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
        public TaskCompletePage(int a,int b,bool isRoutine)
        {
            InitializeComponent();
            firebaseFunctionsService = new FirebaseFunctionsService();

            this.a = a;
            this.b = b;
            this.isRoutine = isRoutine;
            pageModel = new TaskCompletePageViewModel(this, a, b,isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;


        }
        public async void nextpage(object sender, EventArgs args)
        {
            if (NextButton.Text == "Done")
            {
                NextButton.IsEnabled = false;
                var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

                var completeActionCounter = 0;
                var goalId = App.User.goals[a].id;
                var actionId = App.User.goals[a].actions[b].id;

                var isActionComplete = await firebaseFunctionsService.UpdateTask(goalId, actionId, App.User.goals[a].actions[b].dbIdx.ToString());
                if (isActionComplete)
                {
                    App.User.goals[a].actions[b].isComplete = true;
                    App.User.goals[a].actions[b].dateTimeCompleted = DateTime.Now;
                    TaskPage.pageModel.Items[b].IsComplete = true;
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

                await Navigation.PopAsync();
            }

            else if (NextButton.Text == "Start")
            {
                CarouselTasks.Position = 0;
                NextButton.Text = "Next";
            }
            else if (CarouselTasks.Position != App.User.goals[a].actions[b].instructions.Count - 1)
            {

                CarouselTasks.Position += 1;
                Console.WriteLine("You are in page " + CarouselTasks.Position);

            }
            else if (CarouselTasks.Position == App.User.goals[a].actions[b].instructions.Count - 1)
            {
                NextButton.Text = "Done";
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
