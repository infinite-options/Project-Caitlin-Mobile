using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;

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



        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public TaskCompletePageViewModel(TaskCompletePage mainPage,int a, int b, bool isRoutine)
        {
            this.mainPage = mainPage;

            if (isRoutine)
            {
                TopLabel = App.user.routines[a].title;
                TopLabel2 = App.user.routines[a].tasks[b].title;
            }
            else
            {
                TopLabel = App.user.goals[a].title;
                TopLabel2 = App.user.goals[a].actions[b].title;
            }

            if (isRoutine)
            {
                if (App.user.routines[a].tasks[b].steps.Count >= 1)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[0].photo, Text = App.user.routines[a].tasks[b].steps[0].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 2)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[1].photo, Text = App.user.routines[a].tasks[b].steps[1].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 3)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[2].photo, Text = App.user.routines[a].tasks[b].steps[2].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 4)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[3].photo, Text = App.user.routines[a].tasks[b].steps[3].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 5)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[4].photo, Text = App.user.routines[a].tasks[b].steps[4].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 6)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[5].photo, Text = App.user.routines[a].tasks[b].steps[5].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 7)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[6].photo, Text = App.user.routines[a].tasks[b].steps[6].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 8)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[7].photo, Text = App.user.routines[a].tasks[b].steps[7].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 9)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[8].photo, Text = App.user.routines[a].tasks[b].steps[8].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 10)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[9].photo, Text = App.user.routines[a].tasks[b].steps[9].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 11)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[10].photo, Text = App.user.routines[a].tasks[b].steps[10].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 12)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[11].photo, Text = App.user.routines[a].tasks[b].steps[11].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 13)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[12].photo, Text = App.user.routines[a].tasks[b].steps[12].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 14)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[13].photo, Text = App.user.routines[a].tasks[b].steps[13].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 15)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[14].photo, Text = App.user.routines[a].tasks[b].steps[14].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 16)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[15].photo, Text = App.user.routines[a].tasks[b].steps[15].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 17)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[16].photo, Text = App.user.routines[a].tasks[b].steps[16].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 18)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[17].photo, Text = App.user.routines[a].tasks[b].steps[17].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 19)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[18].photo, Text = App.user.routines[a].tasks[b].steps[18].title });

                if (App.user.routines[a].tasks[b].steps.Count >= 20)
                    _items.Add(new { Source = App.user.routines[a].tasks[b].steps[19].photo, Text = App.user.routines[a].tasks[b].steps[19].title });
            }
            else
            {

                if (App.user.goals[a].actions[b].instructions.Count >= 1)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[0].photo, Text = App.user.goals[a].actions[b].instructions[0].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 2)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[1].photo, Text = App.user.goals[a].actions[b].instructions[1].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 3)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[2].photo, Text = App.user.goals[a].actions[b].instructions[2].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 4)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[3].photo, Text = App.user.goals[a].actions[b].instructions[3].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 5)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[4].photo, Text = App.user.goals[a].actions[b].instructions[4].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 6)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[5].photo, Text = App.user.goals[a].actions[b].instructions[5].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 7)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[6].photo, Text = App.user.goals[a].actions[b].instructions[6].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 8)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[7].photo, Text = App.user.goals[a].actions[b].instructions[7].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 9)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[8].photo, Text = App.user.goals[a].actions[b].instructions[8].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 10)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[9].photo, Text = App.user.goals[a].actions[b].instructions[9].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 11)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[10].photo, Text = App.user.goals[a].actions[b].instructions[10].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 12)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[11].photo, Text = App.user.goals[a].actions[b].instructions[11].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 13)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[12].photo, Text = App.user.goals[a].actions[b].instructions[12].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 14)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[13].photo, Text = App.user.goals[a].actions[b].instructions[13].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 15)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[14].photo, Text = App.user.goals[a].actions[b].instructions[14].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 16)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[15].photo, Text = App.user.goals[a].actions[b].instructions[15].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 17)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[16].photo, Text = App.user.goals[a].actions[b].instructions[16].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 18)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[17].photo, Text = App.user.goals[a].actions[b].instructions[17].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 19)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[18].photo, Text = App.user.goals[a].actions[b].instructions[18].title });
                if (App.user.goals[a].actions[b].instructions.Count >= 20)
                    _items.Add(new { Source = App.user.goals[a].actions[b].instructions[19].photo, Text = App.user.goals[a].actions[b].instructions[19].title });
            }
            /*foreach (step step in App.user.routines[routineNum].tasks[taskNum].steps)
            {
                _items.Add(new { Source = step.photo, Text = step.title });
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