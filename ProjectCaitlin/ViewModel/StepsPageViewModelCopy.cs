using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using ProjectCaitlin.Services;
using System.Threading.Tasks;

namespace ProjectCaitlin.ViewModel
{
    public class StepsPageViewModelCopy : BindableObject, INotifyPropertyChanged
    {
        FirebaseFunctionsService firebaseFunctionsService;

        private StepsPageCopy mainPage;
        public int count = 0;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopImage { get; set; }
        public string TopLabel { get; set; }
        public string TaskName { get; set; }
        public string ExpectedCompletionTime { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<ListViewItemModelCopy> Items { get; set; }

        public StepsPageViewModelCopy(StepsPageCopy mainPage, int a, int b, bool isRoutine)
        {
            this.mainPage = mainPage;
            Items = new ObservableCollection<ListViewItemModelCopy>();
            var firestoreService = new FirestoreService();
            firebaseFunctionsService = new FirebaseFunctionsService();

            if (isRoutine)
            {
                TopImage = App.User.routines[a].photo;
                TopLabel = App.User.routines[a].title;
                TaskName = App.User.routines[a].title;
                ExpectedCompletionTime = "Expected to take " +
                    ((int) App.User.routines[a].expectedCompletionTime.TotalMinutes).ToString()
                    + " minutes";

                int stepIdx = 0;
                int stepNum = 1;

                Console.WriteLine("step count: " + App.User.routines[a].tasks[b].steps.Count);

                foreach (task step in App.User.routines[a].tasks)
                {
                    string _checkmarkIcon = "graycheckmarkicon.png";
                    Command<int> _completeStep = new Command<int>((int _stepIdx) => { }); ;

                    if (App.User.routines[a].tasks[stepIdx].isComplete)
                    {
                        _checkmarkIcon = "greencheckmarkicon.png";
                    }
                    else
                    {
                        if (App.User.routines[a].tasks[stepIdx].isInProgress)
                            _checkmarkIcon = "yellowclockicon.png";

                        _completeStep = new Command<int>(
                            async (int _stepIdx) =>
                            {
                                var routineId = App.User.routines[a].id;
                                var taskId = App.User.routines[a].tasks[_stepIdx].id;
                                string stepDbIdx = App.User.routines[a].tasks[_stepIdx].dbIdx.ToString();
                                bool isPrevStepInProgress =
                                    (_stepIdx == 0) ? false : App.User.routines[a].tasks[_stepIdx - 1].isInProgress;
                                bool isPrevStepComplete =
                                    (_stepIdx == 0) ? true : App.User.routines[a].tasks[_stepIdx - 1].isComplete;
                                bool isStepInProgress = App.User.routines[a].tasks[_stepIdx].isInProgress;
                                bool isStepComplete = App.User.routines[a].tasks[_stepIdx].isComplete;
                                var indexForCheckmark = _stepIdx;


                                if (!isStepComplete)
                                {
                                    if (isPrevStepInProgress)
                                    {
                                        if (_stepIdx - 1 >= 0)
                                        {
                                            App.User.routines[a].tasks[_stepIdx - 1].isInProgress = false;
                                            App.User.routines[a].tasks[_stepIdx - 1].isComplete = true;
                                            Items[_stepIdx - 1].CheckmarkIcon = "greencheckmarkicon.png";
                                            //string prevStepDbIdx = App.User.routines[a].tasks[b].steps[_stepIdx - 1].dbIdx.ToString();


                                            await firebaseFunctionsService.updateGratisStatus(App.User.routines[a].tasks[_stepIdx - 1], "goals&routines", true);
                                        }

                                        Items[_stepIdx].CheckmarkIcon = "yellowclockicon.png";
                                        App.User.routines[a].tasks[indexForCheckmark].isInProgress = true;

                                        await firebaseFunctionsService.updateGratisStatus(step, "goals&routines", false);
                                        mainPage.updateParentTask?.Invoke();
                                    }
                                    else
                                    {
                                        if (isPrevStepComplete)
                                        {
                                            if (isStepInProgress)
                                            {
                                                App.User.routines[a].tasks[indexForCheckmark].isInProgress = false;
                                                App.User.routines[a].tasks[indexForCheckmark].isComplete = true;
                                                Items[_stepIdx].CheckmarkIcon = "greencheckmarkicon.png";

                                                firebaseFunctionsService.updateGratisStatus(step, "goals&routines", true);

                                            }
                                            else
                                            {
                                                Items[_stepIdx].CheckmarkIcon = "yellowclockicon.png";
                                                App.User.routines[a].tasks[indexForCheckmark].isInProgress = true;

                                                firebaseFunctionsService.updateGratisStatus(step, "goals&routines", false);

                                            }
                                            mainPage.updateParentTask?.Invoke();
                                        }
                                        else
                                        {
                                            await Application.Current.MainPage.DisplayAlert("Oops!", "Please complete each step in order", "OK");
                                        }
                                    }
                                }
                            }
                        );
                    }

                    Items.Add(new ListViewItemModelCopy
                    (
                        stepNum + ". " + App.User.routines[a].tasks[stepIdx].title,
                        _checkmarkIcon,
                        stepIdx,
                        _completeStep
                    ));

                    stepIdx++;
                    stepNum++;
                }
            }
        }
    }

    public class ListViewItemModelCopy : INotifyPropertyChanged
    {

        private string text;
        public string Text
        {
            get => text;
        }

        private string checkmarkIcon;
        public string CheckmarkIcon
        {
            get => checkmarkIcon;
            set
            {
                if (checkmarkIcon != value)
                {
                    checkmarkIcon = value;
                    OnPropertyChanged(nameof(CheckmarkIcon));
                }
            }
        }

        private int completeIdx;
        public int CompleteIdx
        {
            get => completeIdx;
            set
            {
                if (completeIdx != value)
                {
                    completeIdx = value;
                    OnPropertyChanged(nameof(CompleteIdx));
                }
            }
        }

        private Command<int> completeStep;
        public Command<int> CompleteStep
        {
            get => completeStep;
            set
            {
                if (completeStep != value)
                {
                    completeStep = value;
                    OnPropertyChanged(nameof(CompleteStep));
                }
            }
        }

        public ListViewItemModelCopy(string _text, string _checkmarkIcon, int _completeIdx, Command<int> _completeStep)
        {
            text = _text;
            checkmarkIcon = _checkmarkIcon;
            completeIdx = _completeIdx;
            completeStep = _completeStep;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
