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



        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public TaskCompletePageViewModel(TaskCompletePage mainPage,int routineNum, int taskNum)
        {
            this.mainPage = mainPage;

            if(App.user.routines[routineNum].tasks[taskNum].steps.Count>=1)
            _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[0].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[0].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 2)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[1].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[1].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 3)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[2].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[2].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 4)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[3].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[3].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 5)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[4].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[4].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 6)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[5].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[5].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 7)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[6].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[6].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 8)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[7].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[7].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 9)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[8].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[8].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 10)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[9].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[9].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 11)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[10].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[10].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 12)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[11].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[11].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 13)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[12].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[12].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 14)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[13].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[13].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 15)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[14].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[14].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 16)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[15].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[15].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 17)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[16].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[16].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 18)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[17].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[17].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 19)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[18].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[18].title });

            if (App.user.routines[routineNum].tasks[taskNum].steps.Count >= 20)
                _items.Add(new { Source = App.user.routines[routineNum].tasks[taskNum].steps[19].photo, Text = App.user.routines[routineNum].tasks[taskNum].steps[19].title });

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