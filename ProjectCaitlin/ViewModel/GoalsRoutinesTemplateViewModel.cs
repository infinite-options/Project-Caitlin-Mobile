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
using ProjectCaitlin.Methods;
using Xamarin.Forms;

namespace ProjectCaitlin.ViewModel
{
    public class GoalsRoutinesTemplateViewModel : BindableObject
    {
        private GoalsRoutinesTemplate mainPage;
        List<bool> complete;
        private int _currentIndex;
        private int _imageCount = 1078;
        private FirebaseFunctionsService firebaseFunctionsService;

        public ObservableCollection<GRItemModel> Items { get; set; }

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
            Items = new ObservableCollection<GRItemModel>();
            int itemCount = 0;
            int eventNum = 0;
            int routineNum = 0;
            int goalNum = 0;


            Items.Add(new GRItemModel(
                App.User.aboutMe.pic,
                "About Me",
                Color.Default,
                Color.Black,
                new DateTime(1, 1, 1),
                "Tap to Learn More",
                App.User.aboutMe.message_day,
                1,
                false,
                false,
                new Command<MyDayIndexes>(
                            async (MyDayIndexes indexes) =>
                            {
                                await mainPage.Navigation.PushAsync(new GreetingPage());
                            }),
                        new MyDayIndexes(itemCount, eventNum, 0)
                        ));
            itemCount++;
            eventNum++;


            List<object> list = new List<object>();

            foreach (EventsItems calendarEvent in App.User.CalendarEvents)
            {
                list.Add(calendarEvent);
            }
            foreach (routine routine in App.User.routines)
            {
                list.Add(routine);
            }
            foreach (goal goal in App.User.goals)
            {
                list.Add(goal);
            }

            //insertion sort, sort events,routines,and goals by start time.
            for (int j = 0; j < list.Count; j++ )
            {
                DateTime start;
                if (list[j].GetType().Name == "routine")
                {
                    routine routine = (routine)list[j];
                    start = DateTime.Now.Date + routine.availableStartTime;
                }

                else if (list[j].GetType().Name == "goal")
                {
                    goal goal = (goal)list[j];
                    start = DateTime.Now.Date + goal.availableStartTime;
                }

                else
                {
                    EventsItems item = (EventsItems)list[j];
                    start = item.Start.DateTime.DateTime;
                }
                //int swap = 0;
                object temp = list[j];
                object min = list[j];
                int swap = j;
                for (int i = j+1; i < list.Count; i++)
                {
                    DateTime start2;
                    if (list[i].GetType().Name == "routine")
                    {
                        routine routine2 = (routine)list[i];
                        start2 = DateTime.Now.Date + routine2.availableStartTime;
                    }

                    else if (list[i].GetType().Name == "goal")
                    {
                        goal goal2 = (goal)list[i];
                        start2 = DateTime.Now.Date + goal2.availableStartTime;
                    }

                    else
                    {
                        EventsItems item2 = (EventsItems)list[i];
                        start2 = item2.Start.DateTime.DateTime;
                    }

                    if (start.CompareTo(start2) > 0) {
                        start = start2;
                        min = list[i];
                        swap = i;
                    }
                }
                list[swap] = temp;
                list[j] = min;
            }

            //generate items to card view.
            foreach (object obj in list)
            {
                if (obj.GetType().Name == "routine")
                {
                    routine routine = (routine)obj;
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
                        string buttonText;
                        if (routine.tasks.Count > 0)
                            buttonText = "Tap for Detail";
                        else
                            buttonText = "Tap to Start";
                        if (routine.isInProgress)
                            buttonText = "Tap to Continue";
                        else if (routine.isComplete)
                            buttonText = "Done";

                        DateTime startTime = DateTime.Now.Date + routine.availableStartTime;

                        complete.Add(false);
                        Items.Add(new GRItemModel(
                           routine.photo,
                           routine.title,
                           Color.Default,
                           Color.Black,
                           startTime,
                           buttonText,
                           "Expected to take " + routine.expectedCompletionTime.TotalMinutes + " minutes",
                           (routine.isComplete || routine.isInProgress) ? .6 : 1,
                           routine.isComplete,
                           routine.isInProgress,

                           new Command<MyDayIndexes>(
                                async (MyDayIndexes indexes) =>
                                {
                                    var routine = App.User.routines[indexes.RoutineIndex];
                                    bool isRoutineInProgress = App.User.routines[indexes.RoutineIndex].isInProgress;
                                    bool isRoutineComplete = App.User.routines[indexes.RoutineIndex].isComplete;

                                    ((GRItemModel)Items[indexes.ItemsIndex]).GrImageOpactiy = .6;

                                    if (App.User.routines[indexes.RoutineIndex].isSublistAvailable)
                                    {
                                        if (!isRoutineComplete)
                                        {
                                            App.User.routines[indexes.RoutineIndex].isInProgress = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                            firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", "in progress");
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
                                                firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", "complete");
                                            }
                                            else
                                            {
                                                App.User.routines[indexes.RoutineIndex].isInProgress = true;
                                                ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                                ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                                firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", "in progress");
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
                else if (obj.GetType().Name == "goal")
                {
                    goal goal = (goal)obj;
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
                        string buttonText;
                        if (goal.actions.Count > 0)
                            buttonText = "Tap for Detail";
                        else
                            buttonText = "Tap to Start";
                        if (goal.isInProgress)
                            buttonText = "Tap to Continue";
                        else if (goal.isComplete)
                            buttonText = "Done";

                        DateTime startTime = DateTime.Now.Date + goal.availableStartTime;

                        complete.Add(false);
                        Items.Add(new GRItemModel(
                           goal.photo,
                           goal.title,
                           Color.FromHex("#272E32"),
                           Color.White,
                           startTime,
                           buttonText,
                           "Expected to take " + goal.expectedCompletionTime.TotalMinutes + " minutes",
                           (goal.isComplete || goal.isInProgress) ? .6 : 1,
                           goal.isComplete,
                           goal.isInProgress,

                           new Command<MyDayIndexes>(
                                async (MyDayIndexes indexes) =>
                                {
                                    var goal = App.User.goals[indexes.GoalIndex];
                                    bool isGoalInProgress = App.User.goals[indexes.GoalIndex].isInProgress;
                                    bool isGoalComplete = App.User.goals[indexes.GoalIndex].isComplete;

                                    ((GRItemModel)Items[indexes.ItemsIndex]).GrImageOpactiy = .6;

                                    if (App.User.goals[indexes.GoalIndex].isSublistAvailable)
                                    {
                                        if (!isGoalComplete)
                                        {
                                            App.User.goals[indexes.GoalIndex].isInProgress = true;
                                            ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                            ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                            firebaseFunctionsService.updateGratisStatus(goal, "goals&routines", "in progress");
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
                                                firebaseFunctionsService.updateGratisStatus(goal, "goals&routines", "complete");
                                            }
                                            else
                                            {
                                                App.User.goals[indexes.GoalIndex].isInProgress = true;
                                                ((GRItemModel)Items[indexes.ItemsIndex]).IsInProgress = true;
                                                ((GRItemModel)Items[indexes.ItemsIndex]).Text = "Tap to Continue";
                                                firebaseFunctionsService.updateGratisStatus(goal, "goals&routines", "complete");
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
                else
                {
                    EventsItems calendarEvent = (EventsItems)obj;
                    if (isInTimeRange(calendarEvent.Start.DateTime.TimeOfDay, calendarEvent.End.DateTime.TimeOfDay))
                    {
                        Items.Add(new GRItemModel(
                           "eventIcon.jpg",
                           calendarEvent.EventName,
                           Color.Goldenrod,
                           Color.Black,
                           calendarEvent.Start.DateTime.DateTime,
                           "",
                           "Start Time: " + calendarEvent.Start.DateTime.TimeOfDay + "\n" +
                           "End Time: " + calendarEvent.End.DateTime.TimeOfDay + "",
                           1,
                           false,
                           false,

                           new Command<MyDayIndexes>(
                                async (MyDayIndexes indexes) =>
                                {

                                }),
                            new MyDayIndexes(itemCount, eventNum, 0)
                            ));
                        itemCount++;
                        eventNum++;
                    }
                }
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
            set
            {
                if (!text.Equals(App.User.routines[0].title))
                {
                    text = value;
                    OnPropertyChanged(nameof(Text));
                }
            }

        }

        public DateTime startTime;
        public DateTime StartTime
        {
            get => startTime;
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
            {
                if (!NavigateIdx.Equals(navigateIdx))
                {
                    navigateIdx = value;
                    OnPropertyChanged(nameof(NavigateIdx));
                }
            }
        }

        public GRItemModel(string _source, string _title, Color _backgroundColor, Color _textColor, DateTime _startTime, string _text, string _length, double _grImageOpactiy, bool _isComplete, bool _isInProgress, Command<MyDayIndexes> _navigate, MyDayIndexes _navigateIdx)
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

            startTime = _startTime;
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
