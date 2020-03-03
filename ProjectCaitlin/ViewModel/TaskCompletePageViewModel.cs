using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using ProjectCaitlin.Methods;

namespace ProjectCaitlin.ViewModel
{
    public class TaskCompletePageViewModel : BindableObject
    {
        private TaskCompletePage mainPage;
        public int count = 0;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopLabel { get; set; }
        public string TopLabel2 { get; set; }

        FirestoreService firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public TaskCompletePageViewModel(TaskCompletePage mainPage, int a, int b, bool isRoutine)
        {
            this.mainPage = mainPage;

            var goalId = App.User.goals[a].id;
            var actionId = App.User.goals[a].actions[b].id;

            TopLabel = App.User.goals[a].title;
            TopLabel2 = App.User.goals[a].actions[b].title;

            int i = 0;
            foreach (instruction instruction in App.User.goals[a].actions[b].instructions)
            {
                Command completeStep;
                completeStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.User.goals[a].actions[b].instructions[i].dbIdx.ToString());

                             if (okToCheckmark) { App.User.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    );

                _items.Add(new
                {
                    Source = instruction.photo,
                    Text = instruction.title,
                    CompleteStep = completeStep
                });
                i++;
            }

            /*if (App.user.goals[a].actions[b].instructions.Count >= 1)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 2)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[1].photo,
                    Text = App.user.goals[a].actions[b].instructions[1].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[1].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[1].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 3)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[2].photo,
                    Text = App.user.goals[a].actions[b].instructions[2].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[2].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[2].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 4)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[3].photo,
                    Text = App.user.goals[a].actions[b].instructions[3].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[3].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[3].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 5)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[4].photo,
                    Text = App.user.goals[a].actions[b].instructions[4].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[4].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[4].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 6)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[5].photo,
                    Text = App.user.goals[a].actions[b].instructions[5].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[5].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[5].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 7)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[6].photo,
                    Text = App.user.goals[a].actions[b].instructions[6].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[6].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[6].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 8)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 9)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 10)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 11)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 12)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 13)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 14)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 15)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 16)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 17)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 18)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 19)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }
            if (App.user.goals[a].actions[b].instructions.Count >= 20)
            {
                _items.Add(new
                {
                    Source = App.user.goals[a].actions[b].instructions[0].photo,
                    Text = App.user.goals[a].actions[b].instructions[0].title,

                    CompleteStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.user.goals[a].actions[b].instructions[0].dbIdx.ToString());

                             if (okToCheckmark) { App.user.goals[a].actions[b].instructions[0].isComplete = true; }
                             await mainPage.Navigation.PushAsync(new StepsPage(a, b, isRoutine));

                         }
                    )
                });
            }*/



        }

        public ObservableCollection<object> Items
        {
            get
            {
                return _items;
            }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }


    }
}