using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProjectCaitlin.Services;

namespace ProjectCaitlin.ViewModel
{
    public class TaskGridViewModel : BindableObject
    {
        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        private TaskPage mainPage;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopImage { get; set; }
        public string TopLabel { get; set; }
        public Color BackgroundColor { get; set; }
        public Color TitleTextColor { get; set; }
        public string BackImage { get; set; }
        public Color SoundButton { get; set; }

        public ICommand NavigateCommand { private set; get; }

        List<bool> complete;

        public ObservableCollection<TaskItemModel> Items { get; set; }

        public TaskGridViewModel(TaskPage mainPage, int a, bool isRoutine)
        {
            this.mainPage = mainPage;
            Items = new ObservableCollection<TaskItemModel>();

            if (isRoutine)
            {
                TopImage = App.User.routines[a].photo;
                TopLabel = App.User.routines[a].title;
                BackgroundColor = Color.WhiteSmoke;
                TitleTextColor = Color.FromHex("#272E32");
                BackImage = "arrowicon.png";

                //if(App.user.routines[a].audio != "") SoundButton = "waveicon.png";

                int taskIdx = 0;
                foreach (task task in App.User.routines[a].tasks)
                {
                    Items.Add( new TaskItemModel(
                    
                        App.User.routines[a].tasks[taskIdx].photo,
                        App.User.routines[a].tasks[taskIdx].title,
                        Color.FromHex("#272E32"),

                        App.User.routines[a].tasks[taskIdx].isComplete,
                        new Command<int>(
                            async (int _taskIdx) =>
                            {
                                if (App.User.routines[a].tasks[_taskIdx].isSublistAvailable)
                                {
                                    await mainPage.Navigation.PushAsync(new StepsPage(a, _taskIdx, isRoutine));
                                }
                                else
                                {
                                    string routineId = App.User.routines[a].id;
                                    string taskId = App.User.routines[a].tasks[_taskIdx].id;
                                    int taskDbIdx = App.User.routines[a].tasks[_taskIdx].dbIdx;
                                    bool isTaskInProgress = App.User.routines[a].tasks[_taskIdx].isInProgress;
                                    bool isTaskComplete = App.User.routines[a].tasks[_taskIdx].isComplete;

                                    if (!isTaskComplete)
                                    {
                                        if (isTaskInProgress)
                                        {
                                            App.User.routines[a].tasks[_taskIdx].isInProgress = false;
                                            App.User.routines[a].tasks[_taskIdx].isComplete = true;
                                            await firebaseFunctionsService.UpdateTask(routineId, taskId, taskDbIdx.ToString());
                                        }
                                        else
                                        {
                                            App.User.routines[a].tasks[_taskIdx].isInProgress = true;
                                            await firebaseFunctionsService.StartAT(routineId, taskId, taskDbIdx.ToString());
                                        }
                                    }
                                }
                            }),
                        taskIdx
                    ));
                    taskIdx++;
                }
            }
            else
            {
                TopImage = App.User.goals[a].photo;
                TopLabel = App.User.goals[a].title;
                BackgroundColor = Color.FromHex("#272E32");
                TitleTextColor = Color.WhiteSmoke;
                BackImage = "arrowiconwhite.png";

                int taskIdx = 0;
                foreach (action action in App.User.goals[a].actions)
                {
                    Items.Add(new TaskItemModel(

                        App.User.goals[a].actions[taskIdx].photo,
                        App.User.goals[a].actions[taskIdx].title,
                        Color.WhiteSmoke,

                        App.User.goals[a].actions[taskIdx].isComplete,
                        new Command<int>(
                            async (int _taskIdx) =>
                            {
                                await mainPage.Navigation.PushAsync(new TaskCompletePage(a, _taskIdx, isRoutine));
                            }),
                       taskIdx
                    ));
                    taskIdx++;
                }
            }
        }
    }

    public class TaskItemModel : INotifyPropertyChanged
    {

        private string source;
        public string Source
        {
            get => source;
        }

        private string text;
        public string Text
        {
            get => text;
        }

        private Color textColor;
        public Color TextColor
        {
            get => textColor;
        }

        private bool isComplete;
        public bool IsComplete
        {
            get => isComplete;
            set
            {
                if (isComplete != value)
                {
                    isComplete = value;
                    OnPropertyChanged(nameof(IsComplete));
                }
            }
        }

        private Command<int> navigate;
        public Command<int> Navigate
        {
            get => navigate;
            set
            {
                if (navigate != value)
                {
                    navigate = value;
                    OnPropertyChanged(nameof(Navigate));
                }
            }
        }

        private int navigateIdx;
        public int NavigateIdx
        {
            get => navigateIdx;
            set
            {
                if (navigateIdx != value)
                {
                    navigateIdx = value;
                    OnPropertyChanged(nameof(NavigateIdx));
                }
            }
        }

        public TaskItemModel(string _source, string _text, Color _textColor, bool _isComplete, Command<int> _navigate, int _navigateIdx)
        {
            source = _source;
            text = _text;
            textColor = _textColor;
            isComplete = _isComplete;
            navigate = _navigate;
            navigateIdx = _navigateIdx;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}