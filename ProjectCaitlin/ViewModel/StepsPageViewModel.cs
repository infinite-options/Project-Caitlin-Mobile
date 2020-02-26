using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using ProjectCaitlin.Methods;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace ProjectCaitlin.ViewModel
{
    public class StepsPageViewModel : BindableObject, INotifyPropertyChanged
    {
        private StepsPage mainPage;
        public int count = 0;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopImage { get; set; }
        public string TopLabel { get; set; }
        public string TaskName { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public ObservableCollection<ListViewItemModel> Items { get; set; }

        public StepsPageViewModel(StepsPage mainPage, int a, int b, bool isRoutine)
        {
            this.mainPage = mainPage;
            Items = new ObservableCollection<ListViewItemModel>();
            var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

            if (isRoutine)
            {
                TopImage = App.user.routines[a].tasks[b].photo;
                TopLabel = App.user.routines[a].tasks[b].title;
                TaskName = App.user.routines[a].title;
                int stepIdx = 0;
                int stepNum = 1;

                Console.WriteLine("step count: " + App.user.routines[a].tasks[b].steps.Count);

                foreach (step step in App.user.routines[a].tasks[b].steps)
                {
                    string _checkmarkIcon;
                    Command<int> _completeStep;

                    if (App.user.routines[a].tasks[b].steps[stepIdx].isComplete)
                    {
                        _checkmarkIcon = "greencheckmarkicon.png";
                        _completeStep = new Command<int>((int _stepIdx) => { });
                    }
                    else
                    {
                        _checkmarkIcon = "graycheckmarkicon.png";
                        _completeStep = new Command<int>(
                            async (int _stepIdx) =>
                            {
                                var routineId = App.user.routines[a].id;
                                var taskId = App.user.routines[a].tasks[b].id;
                                var indexForCheckmark = _stepIdx;
                                var okToCheckmark = await firestoreService.UpdateStep(routineId, taskId, indexForCheckmark.ToString());

                                Console.WriteLine("-----------------------------");
                                Console.WriteLine("indexForCheckmark: " + indexForCheckmark);
                                Console.WriteLine("-----------------------------");

                                if (okToCheckmark)
                                {
                                    App.user.routines[a].tasks[b].steps[indexForCheckmark].isComplete = true;
                                    Items[_stepIdx].CheckmarkIcon = "greencheckmarkicon.png";
                                }

                            }
                        );
                    }

                    Items.Add(new ListViewItemModel
                    (
                        stepNum + ". " + App.user.routines[a].tasks[b].steps[stepIdx].title,
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

    public class ListViewItemModel : INotifyPropertyChanged
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

        public ListViewItemModel(string _text, string _checkmarkIcon, int _completeIdx, Command<int> _completeStep)
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
