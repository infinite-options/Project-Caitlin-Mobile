using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace FlowListViewSample
{
    public class TaskGridViewModel : BindableObject
    {
        private TaskPage mainPage;
        public string title { get; set; }

        public string photo { get; set; }

        public TaskGridViewModel(TaskPage mainPage)
        {
            this.mainPage = mainPage;
        }

        
        private ObservableCollection<object> _items = new ObservableCollection<object>() {
        new { Source = "ManOnMountain.png",
                    Text = "Wake up" },

                new { Source = "Airplane.png",
                    Text = "Meeting with Not Impossible Labs" },

                new { Source = "Oranges.png",
                    Text = "Make myself some lunch"},

                new { Source = "WateringPlant.png",
                    Text = "Water Plants"},

                new { Source = "Face.png",
                    Text = "Wash and dry my face" },

                new { Source = "Scale.png",
                    Text = "Weigh myself"},

                new { Source = "Face.png",
                    Text = "Wash and dry my face" },

                new { Source = "Scale.png",
                    Text = "Weigh myself"}
        };
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

        public Command ItemTappedCommand
        {
            get
            {
                return new Command((data) =>
                {
                    mainPage.DisplayAlert("FlowListView", data + "", "Ok");
                    //in xmal FlowItemTappedCommand="{Binding ItemTappedCommand}"  
                });
            }
        }
    }
}