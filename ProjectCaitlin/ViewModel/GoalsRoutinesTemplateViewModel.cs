using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using FFImageLoading;
using PanCardView.Extensions;
using ProjectCaitlin.Views;
using System;
using ProjectCaitlin;
using ProjectCaitlin.Models;
using System.Collections.Generic;
using ProjectCaitlin.Views;
using System.Globalization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProjectCaitlin.Services;

namespace ProjectCaitlin.ViewModel
{
    public class GoalsRoutinesTemplateViewModel : BindableObject
    {
        private GoalsRoutinesTemplate mainPage;
        List<bool> complete;
        private int _currentIndex;
        private int _imageCount = 1078;
        private FirebaseFunctionsService firebaseFunctionsService;

        public ObservableCollection<object> Items { get; set; }

        public ICommand AboutMeCommand { private set; get; }
        public string DayLabel { get; set; }
        public string TimeLabel { get; set; }
        public string DayImage { get; set; }
        
        public GoalsRoutinesTemplateViewModel(GoalsRoutinesTemplate mainPage)
        {
            this.mainPage = mainPage;
            firebaseFunctionsService = new FirebaseFunctionsService();

            setUpTime();
            complete = new List<bool>();
            Items = new ObservableCollection<object>();
            AboutMeCommand = new Command(
            async () =>
            {
                await mainPage.Navigation.PushAsync(new GreetingPage());
            });
            Items.Add(new
            {
                Source = App.User.Me.pic,
                Title = "About Me",
                Ind = _imageCount++,
                BackgroundColor = Color.Default,
                TextColor = Color.Black,
                Length = App.User.Me.message_day,
                Text = "Tap to Learn More",
                IsComplete = false,
                IsInProgress = false,
                Navigate = AboutMeCommand
            });
            int itemCount = 1;

            int routineNum = 0;
            foreach (routine routine in App.User.routines)
            {
                //calculate the sum duration for the routine from step level.
                if (routine.isSublistAvailable == true)
                {
                    int sum_duration = 0;
                    foreach (task task in routine.tasks)
                    {
                        if (task.isSublistAvailable == true)
                        {
                            int step_duration = 0;
                            foreach (step step in task.steps)
                            {
                                step_duration += (int)step.expectedCompletionTime.TotalMinutes;
                            }
                            if (step_duration == 0)
                                sum_duration += (int)task.expectedCompletionTime.TotalMinutes;
                            else
                                sum_duration += step_duration;
                        }
                        else
                        {
                            sum_duration += (int)task.expectedCompletionTime.TotalMinutes;
                        }
                    }
                    // update the duration for routine
                    if (sum_duration != 0)
                        routine.expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);
                }

                if (isInTimeRange(routine.availableStartTime, routine.availableEndTime))
                {
                   
                    string buttonText = "Tap to Start";
                    if (routine.isInProgress)
                        buttonText = "Tap to Continue";
                    else if (routine.isComplete)
                        buttonText = "Done";

                    complete.Add(false);
                    Items.Add(new GRItemModel(
                       routine.photo,
                       routine.title,
                       Color.Default,
                       Color.Black,
                       buttonText,
                       "Expected to take " + routine.expectedCompletionTime.TotalMinutes + " minutes",
                       (routine.isComplete || routine.isInProgress) ? .6 : 1,
                       routine.isComplete,
                       routine.isInProgress,

                       new Command<MyDayIndexes>(
                            async (MyDayIndexes indexes) =>
                            {
                                string routineId = App.User.routines[indexes.RoutineIndex].id;
                                string routineDbIdx = App.User.routines[indexes.RoutineIndex].dbIdx.ToString();
                                bool isRoutineInProgress = App.User.routines[indexes.RoutineIndex].isInProgress;
                                bool isRoutineComplete = App.User.routines[indexes.RoutineIndex].isComplete;

                                Console.WriteLine("indexes.ItemsIndex: " + indexes.ItemsIndex);
                                Console.WriteLine("indexes.RoutineIndex: " + indexes.RoutineIndex);

                                ((GRItemModel)Items[indexes.ItemsIndex]).GrImageOpactiy = .6;

                                if (App.User.routines[indexes.RoutineIndex].isSublistAvailable)
                                {
                                    if (!isRoutineComplete)
                                    {
                                        App.User.routines[indexes.RoutineIndex].isInProgress = true;
                                        ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                        ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                        firebaseFunctionsService.startGR(routineId, routineDbIdx);
                                    }
                                    await mainPage.Navigation.PushAsync(new TaskPage(indexes.RoutineIndex, true, (GRItemModel)Items[indexes.ItemsIndex]));
                                }
                                else
                                {
                                    if (!isRoutineComplete)
                                    {
                                        if (isRoutineInProgress)
                                        {
                                            App.User.routines[indexes.RoutineIndex].isInProgress = false;
                                            App.User.routines[indexes.RoutineIndex].isComplete = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = false;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsComplete = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Done";
                                            firebaseFunctionsService.CompleteRoutine(routineId, routineDbIdx);
                                        }
                                        else
                                        {
                                            App.User.routines[indexes.RoutineIndex].isInProgress = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                            firebaseFunctionsService.startGR(routineId, routineDbIdx);
                                        }
                                    }
                                }
                            }),
                        new MyDayIndexes(itemCount, routineNum, 0)
                        ));
                    itemCount++;
                }
                routineNum++;
            }

            int goalNum = 0;
            foreach (goal goal in App.User.goals)
            {
                //calculate the sum duration for the goal from instruction level.
                if (goal.isSublistAvailable == true)
                {
                    int goal_duration = 0;
                    foreach (action action in goal.actions)
                    {
                        if (action.isSublistAvailable == true)
                        {
                            int instruction_duration = 0;
                            foreach (instruction instruction in action.instructions)
                            {
                                instruction_duration += (int)instruction.expectedCompletionTime.TotalMinutes;
                            }
                            if (instruction_duration == 0)
                                goal_duration += (int)action.expectedCompletionTime.TotalMinutes;
                            else
                                goal_duration += instruction_duration;
                        }
                        else
                        {
                            goal_duration += (int)action.expectedCompletionTime.TotalMinutes;
                        }
                    }
                    // update the duration for goal
                    if (goal_duration != 0)
                        goal.expectedCompletionTime = TimeSpan.FromMinutes(goal_duration);
                }

                if (isInTimeRange(goal.availableStartTime, goal.availableEndTime))
                {
                    string buttonText = "Tap to Start";
                    if (goal.isInProgress)
                        buttonText = "Tap to Continue";
                    else if (goal.isComplete)
                        buttonText = "Done";

                    complete.Add(false);
                    Items.Add(new GRItemModel(
                       goal.photo,
                       goal.title,
                       Color.FromHex("#272E32"),
                       Color.White,
                       buttonText,
                       "Expected to take " + goal.expectedCompletionTime.TotalMinutes + " minutes",
                       (goal.isComplete || goal.isInProgress) ? .6 : 1,
                       goal.isComplete,
                       goal.isInProgress,

                       new Command<MyDayIndexes>(
                            async (MyDayIndexes indexes) =>
                            {
                                string goalId = App.User.goals[indexes.GoalIndex].id;
                                string goalDbIdx = App.User.goals[indexes.GoalIndex].dbIdx.ToString();
                                bool isGoalInProgress = App.User.goals[indexes.GoalIndex].isInProgress;
                                bool isGoalComplete = App.User.goals[indexes.GoalIndex].isComplete;

                                Console.WriteLine("indexes.ItemsIndex: " + indexes.ItemsIndex);
                                Console.WriteLine("indexes.GoalIndex: " + indexes.GoalIndex);

                                ((GRItemModel)Items[indexes.ItemsIndex]).GrImageOpactiy = .6;

                                if (App.User.goals[indexes.GoalIndex].isSublistAvailable)
                                {
                                    if (!isGoalComplete)
                                    {
                                        App.User.goals[indexes.GoalIndex].isInProgress = true;
                                        ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                        ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                        firebaseFunctionsService.startGR(goalId, goalDbIdx);
                                    }
                                    await mainPage.Navigation.PushAsync(new TaskPage(indexes.GoalIndex, false, (GRItemModel)Items[indexes.ItemsIndex]));
                                }
                                else
                                {
                                    if (!isGoalComplete)
                                    {
                                        if (isGoalInProgress)
                                        {
                                            App.User.goals[indexes.GoalIndex].isInProgress = false;
                                            App.User.goals[indexes.GoalIndex].isComplete = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = false;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsComplete = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Done";
                                            firebaseFunctionsService.CompleteRoutine(goalId, goalDbIdx);
                                        }
                                        else
                                        {
                                            App.User.goals[indexes.GoalIndex].isInProgress = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                            firebaseFunctionsService.startGR(goalId, goalDbIdx);
                                        }
                                    }
                                }
                            }),
                       new MyDayIndexes(itemCount, 0, goalNum)
                        ));
                    itemCount++;
                }
                goalNum++;
            }
        }

        public ICommand PanPositionChangedCommand { get; }

        public ICommand RemoveCurrentItemCommand { get; }

        public ICommand GoToLastCommand { get; }


        private bool isInTimeRange(TimeSpan start, TimeSpan end)
        {
            DateTime dateTimeNow = DateTime.Now;
            //DateTime dateTimeNow = new DateTime(1999, 12, 1, 00, 00, 00);
            if (start.Subtract(TimeSpan.FromHours(4)) <= dateTimeNow.TimeOfDay && dateTimeNow.TimeOfDay <= end)
                return true;
            return false;
        }

        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentIndex)));
            }
        }
        public void setUpTime()
        {
            DateTime localDate = DateTime.Now;
            // Uses the default calendar of the InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            DayLabel = myCal.GetDayOfWeek(localDate) + "";
            int time = localDate.Hour;

            if (time >= 6 && time < 12) { TimeLabel = "Good Morning"; DayImage = "sunrisemid.png"; }
            else if (time >= 12 && time < 18) { TimeLabel = "Good Afternoon"; DayImage = "fullsun.png"; }
            else if (time >= 18 && time <= 23) { TimeLabel = "Good Evening"; DayImage = "sunriselow.png"; }
            else { TimeLabel = "Good Night"; DayImage = "moon.png"; }
        }
        public bool IsAutoAnimationRunning { get; set; }

        public bool IsUserInteractionRunning { get; set; }


    }
    public class GRItemModel : INotifyPropertyChanged
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
            set
            {
                if (!text.Equals(value))
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }
        }



        private string title;
        public string Title
        {
            get => title;
        }


        private Color textColor;
        public Color TextColor
        {
            get => textColor;
        }

        private Color backgroundColor;
        public Color BackgroundColor
        {
            get => backgroundColor;
        }

        private string length;
        public string Length
        {
            get => length;
            set
            {
                if (!length.Equals(value))
                {
                    length = value;
                    OnPropertyChanged(nameof(Length));
                }
            }
        }

        private double grImageOpactiy;
        public double GrImageOpactiy
        {
            get => grImageOpactiy;
            set
            {
                if (grImageOpactiy != value)
                {
                    grImageOpactiy = value;
                    OnPropertyChanged(nameof(GrImageOpactiy));
                }
            }
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

        private Command<MyDayIndexes> navigate;
        public Command<MyDayIndexes> Navigate
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

        private MyDayIndexes navigateIdx;
        public MyDayIndexes NavigateIdx
        {
            get => navigateIdx;
            set
            { if (!NavigateIdx.Equals(navigateIdx))
                {
                    navigateIdx = value;
                    OnPropertyChanged(nameof(NavigateIdx));
                }
            }
        }

        public GRItemModel(string _source, string _title, Color _backgroundColor, Color _textColor, string _text, string _length, double _grImageOpactiy, bool _isComplete, bool _isInProgress, Command<MyDayIndexes> _navigate, MyDayIndexes _navigateIdx)
        {
            source = _source;
            title = _title;
            backgroundColor = _backgroundColor;
            textColor = _textColor;
            text = _text;
            length = _length;
            grImageOpactiy = _grImageOpactiy;
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

    public struct MyDayIndexes
    {
        public MyDayIndexes(int itemsIndex, int routineIndex, int goalIndex)
        {
            ItemsIndex = itemsIndex;
            RoutineIndex = routineIndex;
            GoalIndex = goalIndex;
        }

        public int ItemsIndex { get; }
        public int RoutineIndex { get; }
        public int GoalIndex { get; }

        public override string ToString() => $"(ItemsIndex: {ItemsIndex}, GoalIndex: {GoalIndex}, RoutineIndex: {RoutineIndex})";
    }
}
