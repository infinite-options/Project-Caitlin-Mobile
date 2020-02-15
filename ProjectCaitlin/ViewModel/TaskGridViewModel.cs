using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;

namespace FlowListViewSample
{
    public class TaskGridViewModel : BindableObject
    {
        private TaskPage mainPage;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }

       

        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public TaskGridViewModel(TaskPage mainPage)
        {
            this.mainPage = mainPage;
            foreach (task task in App.user.routines[0].tasks)
            {
                _items.Add(new { Source = task.photo, Text = task.title });
                OnPropertyChanged(nameof(routine));
                Console.WriteLine("user routine photo: " + task.photo);
                Console.WriteLine("user routine title: " + task.title);

            }
            foreach (step step in App.user.routines[0].tasks[0].steps)
            {

                Console.WriteLine("user step photo: " + step.photo);
                Console.WriteLine("user step title: " + step.title);


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
       

        
            /* get
             {
                 return new Command((data) =>
                 {
                     mainPage.DisplayAlert("FlowListView", data + "", "Ok");
                     //in xmal FlowItemTappedCommand="{Binding ItemTappedCommand}"  
                 });
             }*/
        
    }
}