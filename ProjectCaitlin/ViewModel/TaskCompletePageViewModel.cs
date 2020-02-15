using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;

namespace FlowListViewSample
{
    public class TaskCompletePageViewModel : BindableObject
    {
        private TaskCompletePage mainPage;
        public int count = 0;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }



        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public TaskCompletePageViewModel(TaskCompletePage mainPage)
        {
            this.mainPage = mainPage;
            foreach (task step in App.user.routines[0].tasks)
            {
                _items.Add(new { Source = step.photo, Text = step.title });
                OnPropertyChanged(nameof(step));
                Console.WriteLine("user routine photo: " + step.photo);
                Console.WriteLine("user routine title: " + step.title);
                count++;
            }
            _items.Add(new { Source = "WateringThePlant.png", Text = "Pour water into the plant" });
            count++;

            _items.Add(new { Source = "WateringThePlant.png", Text = "Pour water into the plant" });
            count++;

            _items.Add(new { Source = "WateringThePlant.png", Text = "Pour water into the plant" });
            count++;

            _items.Add(new { Source = "WateringThePlant.png", Text = "Pour water into the plant" });
            count++;


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