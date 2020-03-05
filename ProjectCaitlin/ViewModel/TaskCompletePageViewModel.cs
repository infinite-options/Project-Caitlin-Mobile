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