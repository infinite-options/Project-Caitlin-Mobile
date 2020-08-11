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
using System.Data;

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

        GRItemModel GRItemModel;

        public TaskGridViewModel(TaskPage mainPage, int a, bool isRoutine, GRItemModel _GRItemModel)
        {
            this.mainPage = mainPage;
            Items = new ObservableCollection<TaskItemModel>();

            GRItemModel = _GRItemModel;

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
                    string title;
                    if (task.steps.Count > 0)
                        title = "· " + App.User.routines[a].tasks[taskIdx].title;
                    else
                        title = App.User.routines[a].tasks[taskIdx].title;
                    Items.Add( new TaskItemModel(
                        App.User.routines[a].tasks[taskIdx].photo,
                        title,
                        Color.FromHex("#272E32"),

                        App.User.routines[a].tasks[taskIdx].isComplete,
                        App.User.routines[a].tasks[taskIdx].isInProgress,
                        new Command<int>(
                            async (int _taskIdx) =>
                            {
                                string routineId = App.User.routines[a].id;
                                string taskId = App.User.routines[a].tasks[_taskIdx].id;
                                int taskDbIdx = App.User.routines[a].tasks[_taskIdx].dbIdx;
                                bool isTaskInProgress = App.User.routines[a].tasks[_taskIdx].isInProgress;
                                bool isTaskComplete = App.User.routines[a].tasks[_taskIdx].isComplete;

                                if (App.User.routines[a].tasks[_taskIdx].isSublistAvailable)
                                {
                                    //if (!isTaskComplete)
                                    //{
                                    //    App.User.routines[a].tasks[_taskIdx].isInProgress = true;
                                    //    Items[_taskIdx].IsInProgress = true;
                                    //    /*if(!App.User.routines[a].isComplete && !App.User.routines[a].isInProgress)
                                    //    {
                                    //        App.User.routines[a].isInProgress = true;
                                    //        firebaseFunctionsService.updateGratisStatus(App.User.routines[a], "goals&routines", false);
                                    //    }*/
                                    //    firebaseFunctionsService.updateGratisStatus(task, "actions&tasks", false);
                                    //}
                                    //await mainPage.Navigation.PushAsync(new StepsPage(a, _taskIdx, isRoutine, Items[_taskIdx], GRItemModel));
                                    await mainPage.Navigation.PushAsync(new StepsPage(a, _taskIdx, isRoutine, Items[_taskIdx], GRItemModel, async ()=>{
                                        if (!isTaskComplete)
                                        {
                                            App.User.routines[a].tasks[_taskIdx].isInProgress = true;
                                            Items[_taskIdx].IsInProgress = true;
                                            firebaseFunctionsService.updateGratisStatus(task, "actions&tasks", false);
                                            mainPage.updateParentRoutine?.Invoke();
                                        }
                                    }));
                                }
                                else
                                {
                                    if (!isTaskComplete)
                                    {
                                        if (isTaskInProgress)
                                        {
                                            App.User.routines[a].tasks[_taskIdx].isInProgress = false;
                                            App.User.routines[a].tasks[_taskIdx].isComplete = true;
                                            Items[_taskIdx].IsInProgress = false;
                                            Items[_taskIdx].IsComplete = true;
                                            firebaseFunctionsService.updateGratisStatus(task, "actions&tasks", true);

                                            if (routineCheckCompletion(App.User.routines[a].tasks))
                                            {
                                                App.User.routines[a].isComplete = true;
                                                App.User.routines[a].isInProgress = false;
                                                if (App.ParentPage != "ListView")
                                                {
                                                    GRItemModel.IsComplete = true;
                                                    GRItemModel.IsInProgress = false;
                                                    GRItemModel.Text = "Done";
                                                }
                                                firebaseFunctionsService.updateGratisStatus(App.User.routines[a], "goals&routines", true);
                                            }


                                        }
                                        else
                                        {
                                            App.User.routines[a].tasks[_taskIdx].isInProgress = true;
                                            Items[_taskIdx].IsInProgress = true;
                                            if (!App.User.routines[a].isComplete && !App.User.routines[a].isInProgress)
                                            {
                                                App.User.routines[a].isInProgress = true;
                                                firebaseFunctionsService.updateGratisStatus(App.User.routines[a], "goals&routines", false);
                                            }
                                            firebaseFunctionsService.updateGratisStatus(task, "actions&tasks", false);
                                        }
                                        mainPage.updateParentRoutine?.Invoke();
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

                int actionIdx = 0;
                foreach (action action in App.User.goals[a].actions)
                {
                    Items.Add(new TaskItemModel(

                        App.User.goals[a].actions[actionIdx].photo,
                        App.User.goals[a].actions[actionIdx].title,
                        Color.WhiteSmoke,

                        App.User.goals[a].actions[actionIdx].isComplete,
                        App.User.goals[a].actions[actionIdx].isInProgress,
                        new Command<int>(
                            async (int _actionIdx) =>
                            {
                                string goalId = App.User.goals[a].id;
                                string actionId = App.User.goals[a].actions[_actionIdx].id;
                                int actionDbIdx = App.User.goals[a].actions[_actionIdx].dbIdx;
                                bool isActionInProgress = App.User.goals[a].actions[_actionIdx].isInProgress;
                                bool isActionComplete = App.User.goals[a].actions[_actionIdx].isComplete;

                                if (App.User.goals[a].actions[_actionIdx].isSublistAvailable)
                                {
                                    //if (!isActionComplete)
                                    //{
                                    //    App.User.goals[a].actions[_actionIdx].isInProgress = true;
                                    //    Items[_actionIdx].IsInProgress = true;
                                    //    /*if (!App.User.goals[a].isComplete && !App.User.goals[a].isInProgress)
                                    //    {
                                    //        App.User.goals[a].isInProgress = true;
                                    //        firebaseFunctionsService.updateGratisStatus(App.User.goals[a], "goals&routines", false);
                                    //    }*/
                                    //    firebaseFunctionsService.updateGratisStatus(action, "actions&tasks", false);
                                    //}

                                    //await mainPage.Navigation.PushAsync(new TaskCompletePage(a, _actionIdx, isRoutine, Items[_actionIdx], GRItemModel));
                                    await mainPage.Navigation.PushAsync(new TaskCompletePage(a, _actionIdx, isRoutine, Items[_actionIdx], GRItemModel, async() => {
                                        if (!isActionComplete)
                                        {
                                            App.User.goals[a].actions[_actionIdx].isInProgress = true;
                                            Items[_actionIdx].IsInProgress = true;
                                            await firebaseFunctionsService.updateGratisStatus(action, "actions&tasks", false);
                                            mainPage.updateParentRoutine?.Invoke();
                                        }
                                    }));
                                }
                                else
                                {
                                    if (!isActionComplete)
                                    {
                                        if (isActionInProgress)
                                        {
                                            App.User.goals[a].actions[_actionIdx].isInProgress = false;
                                            App.User.goals[a].actions[_actionIdx].isComplete = true;
                                            Items[_actionIdx].IsInProgress = false;
                                            Items[_actionIdx].IsComplete = true;
                                            firebaseFunctionsService.updateGratisStatus(action, "actions&tasks", true);

                                            if (goalCheckCompletion(App.User.goals[a].actions))
                                            {
                                                App.User.goals[a].isComplete = true;
                                                App.User.goals[a].isInProgress = false;
                                                if (App.ParentPage != "ListView")
                                                {
                                                    GRItemModel.IsComplete = true;
                                                    GRItemModel.IsInProgress = false;
                                                    GRItemModel.Text = "Done";
                                                }
                                                firebaseFunctionsService.updateGratisStatus(App.User.goals[a], "goals&routines", true);
                                            }
                                        }
                                        else
                                        {
                                            App.User.goals[a].actions[_actionIdx].isInProgress = true;
                                            Items[_actionIdx].IsInProgress = true;
                                            if (!App.User.goals[a].isComplete && !App.User.goals[a].isInProgress)
                                            {
                                                App.User.goals[a].isInProgress = true;
                                                firebaseFunctionsService.updateGratisStatus(App.User.goals[a], "goals&routines", false);
                                            }
                                            firebaseFunctionsService.updateGratisStatus(action, "actions&tasks", false);
                                        }
                                    }
                                    mainPage.updateParentRoutine?.Invoke();
                                }
                            }),
                       actionIdx
                    ));
                    actionIdx++;
                }
            }
        }

        private bool routineCheckCompletion(List<task> taskList)
        {
            int complCount = 0;
            int mustDoTasks = 0;

            
            foreach (task task in taskList)
            {
                if (task.isMustDo)
                {
                    mustDoTasks++;

                    complCount = (task.isComplete) ? complCount + 1 : complCount;
                }
                    

                /*if (task.isComplete && task.isMustDo)
                {
                    complCount++;
                }*/
            }
            return complCount == mustDoTasks ? true : false;
        }

        private bool goalCheckCompletion(List<action> actionList)
        {
            int complCount = 0;
            int mustDoActions = 0;
            foreach (action action in actionList)
            {
                if (action.isMustDo)
                {
                    mustDoActions++;
                    complCount = (action.isComplete) ? complCount + 1 : complCount;
                }
                /*mustDoActions++;

                if (action.isComplete)
                {
                    complCount++;
                }*/
            }
            return complCount == mustDoActions ? true : false;
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

        private bool isInProgress;
        public bool IsInProgress
        {
            get => isInProgress;
            set
            {
                if (isInProgress != value)
                {
                    isInProgress = value;
                    OnPropertyChanged(nameof(IsInProgress));
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

        public TaskItemModel(string _source, string _text, Color _textColor, bool _isComplete, bool _isInProgress, Command<int> _navigate, int _navigateIdx)
        {
            source = _source;
            text = _text;
            textColor = _textColor;
            isComplete = _isComplete;
            isInProgress = _isInProgress;
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
