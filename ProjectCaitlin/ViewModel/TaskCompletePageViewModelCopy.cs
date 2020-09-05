using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using ProjectCaitlin.Services;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ProjectCaitlin.ViewModel
{
    public class TaskCompletePageViewModelCopy : BindableObject
    {
        private TaskCompletePageCopy mainPage;
        public int count = 0;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopLabel { get; set; }
        public string TopLabel2 { get; set; }

        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        public ObservableCollection<InstructionModelCopy> Items { get; set; }
        public TaskCompletePageViewModelCopy(TaskCompletePageCopy mainPage, int a, int b, bool isRoutine)
        {
            Items = new ObservableCollection<InstructionModelCopy>();
            this.mainPage = mainPage;


            var goalId = App.User.goals[a].id;
            var actionId = App.User.goals[a].actions[b].id;

            TopLabel = App.User.goals[a].title;
            TopLabel2 = App.User.goals[a].actions[b].title;

            int i = 0;
            foreach (action instruction in App.User.goals[a].actions)
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
                                var firestoreService = new FirestoreService(App.User.id);
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

                Items.Add(new InstructionModelCopy
                (
                    instruction.photo, instruction.title, _okToCheckmark
                  )
                { Description = TopLabel});
                Console.WriteLine("instruction " + i + " isComplete : " + instruction.isComplete);
                i++;
            }
        }



    }

    public class InstructionModelCopy : INotifyPropertyChanged
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

        public string Description { get; set; }

        public Command CompleteInstuction { get; set; }

        public InstructionModelCopy(string source, string text, bool _okToCheckmark)
        {

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
