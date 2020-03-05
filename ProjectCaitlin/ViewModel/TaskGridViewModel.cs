using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
namespace ProjectCaitlin.ViewModel
{
    public class TaskGridViewModel : BindableObject
    {
        private TaskPage mainPage;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopImage { get; set; }
        public string TopLabel { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TitleTextColor { get; set; }
        public Color SoundButton { get; set; }

        public ICommand NavigateCommand { private set; get; }

        List<bool> complete;

        private ObservableCollection<object> _items;

        public TaskGridViewModel(TaskPage mainPage, int a, bool isRoutine)
        {
            this.mainPage = mainPage;
            _items = new ObservableCollection<object>();

            if (isRoutine)
            {
                TopImage = App.User.routines[a].photo;
                TopLabel = App.User.routines[a].title;
                BackgroundColor = Color.WhiteSmoke;
                TitleTextColor = Color.FromHex("#272E32");

                //if(App.user.routines[a].audio != "") SoundButton = "waveicon.png";

                int taskIdx = 0;
                foreach (task task in App.User.routines[a].tasks)
                {
                    _items.Add(new
                    {
                        Source = App.User.routines[a].tasks[taskIdx].photo,
                        Text = App.User.routines[a].tasks[taskIdx].title,
                        TextColor = Color.FromHex("#272E32"),

                        isComplete = App.User.routines[a].tasks[taskIdx].isComplete,
                        Navigate = new Command<int>(
                            async (int _taskIdx) =>
                            {
                                await mainPage.Navigation.PushAsync(new StepsPage(a, _taskIdx, isRoutine));
                            }),
                        NavigateIdx = taskIdx,
                    }) ;
                    taskIdx++;
                }
            }
            else
            {
                TopImage = App.User.goals[a].photo;
                TopLabel = App.User.goals[a].title;
                BackgroundColor = Color.FromHex("#272E32");
                TitleTextColor = Color.WhiteSmoke;

                int taskIdx = 0;
                foreach (action action in App.User.goals[a].actions)
                {
                    _items.Add(new
                    {
                        Source = App.User.goals[a].actions[taskIdx].photo,
                        Text = App.User.goals[a].actions[taskIdx].title,
                        TextColor = Color.WhiteSmoke,

                        isComplete = App.User.goals[a].actions[taskIdx].isComplete,
                        Navigate = new Command<int>(
                            async (int _taskIdx) =>
                            {
                                await mainPage.Navigation.PushAsync(new TaskCompletePage(a, _taskIdx, isRoutine));
                            }),
                        NavigateIdx = taskIdx,
                    });
                    taskIdx++;
                }
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