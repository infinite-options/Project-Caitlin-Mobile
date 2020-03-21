using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using ProjectCaitlin.Methods;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        public ObservableCollection<InstructionModel> Items { get; set; }
        public TaskCompletePageViewModel(TaskCompletePage mainPage, int a, int b, bool isRoutine)
        {
            Items = new ObservableCollection<InstructionModel>();
            this.mainPage = mainPage;


            var goalId = App.User.goals[a].id;
            var actionId = App.User.goals[a].actions[b].id;

            TopLabel = App.User.goals[a].title;
            TopLabel2 = App.User.goals[a].actions[b].title;

            int i = 0;
            foreach (instruction instruction in App.User.goals[a].actions[b].instructions)
            {
                /*Command completeInstuction;
                if (App.user.routines[a].tasks[b].steps[i].isComplete)
                {
                    completeInstuction = new Command<int>((int _stepIdx) => { });
                }
                else
                {
                    completeInstuction = new Command(
                            async () =>
                            {
                                var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");
                                var goalId = App.user.goals[a].id;
                                var actionId = App.user.goals[a].actions[b].id;
                                var isInstructionComplete = await firestoreService.UpdateInstruction(goalId, actionId, App.user.goals[a].actions[b].instructions[i].dbIdx.ToString());
                                if (isInstructionComplete)
                                {
                                    App.user.goals[a].actions[b].instructions[i].isComplete = true;
                                    App.user.goals[a].actions[b].instructions[i].dateTimeCompleted = DateTime.Now;
                                }
                            }
                        );
                }*/
                bool _okToCheckmark;

                if (instruction.isComplete)
                {
                    _okToCheckmark = true;
                }
                else
                {
                    _okToCheckmark = false;
                }

                Items.Add(new InstructionModel
                (
                    instruction.photo, instruction.title, _okToCheckmark
                  ));
                Console.WriteLine("instruction " + i + " isComplete : " + instruction.isComplete);
                i++;
            }
        }

        public class InstructionModel : INotifyPropertyChanged
        {

            public string Text { get; set; }
            public string Source { get; set; }

            private bool okToCheckmark;

            public bool OkToCheckmark
            {
                get { return okToCheckmark; }
                set
                {
                    if (okToCheckmark != value)
                    {
                        okToCheckmark = value;
                        OnPropertyChanged(nameof(OkToCheckmark));
                    }
                }
            }


            public Command CompleteInstuction { get; set; }

            public InstructionModel(string source, string text, bool _okToCheckmark)
            {
                Command completeStep;
                completeStep = new Command(
                         async () =>
                         {
                             var okToCheckmark = await firestoreService.UpdateStep(goalId, actionId, App.User.goals[a].actions[b].instructions[i].dbIdx.ToString());

                Source = source;
                Text = text;
                okToCheckmark = _okToCheckmark;

            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
