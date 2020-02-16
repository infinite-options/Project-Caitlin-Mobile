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
            foreach (step step in App.user.routines[routineNum].tasks[taskNum].steps)
            {
                _items.Add(new { Source = step.photo, Text = step.title });
                OnPropertyChanged(nameof(step));
                Console.WriteLine("user routine photo: " + step.photo);
                Console.WriteLine("user routine title: " + step.title);
                count++;
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