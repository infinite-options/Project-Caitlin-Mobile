using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectCaitlin.Services;
//using ProjectCaitlin.ViewModel;
using Xamarin.Forms;
using Newtonsoft.Json;
using ProjectCaitlin.Models;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;
using FFImageLoading.Work;
using ProjectCaitlin.Views;
using ProjectCaitlin.Methods;
using System.Threading;
using System.ComponentModel;
using System.Globalization;
using Acr.UserDialogs;

namespace ProjectCaitlin
{
    public partial class ListViewPage : ContentPage
    {
        List<StackLayout> EventAndRoutineStackLayouts = new List<StackLayout>();
        List<StackLayout> GoalsStackLayouts = new List<StackLayout>();

        TimeSpan morningStart = new TimeSpan(6, 0, 0);
        TimeSpan morningEnd = new TimeSpan(11, 0, 0);
        TimeSpan afternoonStart = new TimeSpan(11, 0, 0);
        TimeSpan afternoonEnd = new TimeSpan(18, 0, 0);
        TimeSpan eveningStart = new TimeSpan(18, 0, 0);
        TimeSpan eveningEnd = new TimeSpan(23, 59, 59);
        TimeSpan nightStart = new TimeSpan(0, 0, 0);
        TimeSpan nightEnd = new TimeSpan(6, 0, 0);

        DateTime dateTimeNow;

        string labelFont;

        FirestoreService firestoreService;

        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        GoogleService googleService = new GoogleService();

        user user;
        //public DailyViewModel dailyViewModel;

        public ListViewPage()
        {
            InitializeComponent();
            googleService.Navigation = this.Navigation;
            App.ParentPage = "ListView";
            putLayoutsIntoLists();
            user = App.User;

            dateTimeNow = DateTime.Now;
            //dateTimeNow = new DateTime(2020, 2, 19, 10, 0, 0);

            /*labelFont = Device.RuntimePlatform == Device.iOS ? "Lobster-Regular" :
                Device.RuntimePlatform == Device.Android ? "Lobster-Regular.ttf#Lobster-Regular" : "Assets/Fonts/Lobster-Regular.ttf#Lobster";*/

            SetupUI();

            AddTapGestures();
            /*
                        SetupUI();

                        AddTapGestures();*/

            firestoreService = new FirestoreService();

            StartTimer();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            mainScrollView.ScrollToAsync(0, App.ListPageScrollPosY, true);
        }

        void SetupUI()
        {
            DayOfWeekLabel.Text = dateTimeNow.DayOfWeek.ToString();

            //dateTimeNow = DateTime.Now;
            //HidePreviousTimeOfDayElements(dateTimeNow.TimeOfDay);

            try
            {
                DeletePageElements();
                PopulatePage();
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);
            }
            mainScrollView.HeightRequest = Application.Current.MainPage.Height - NavBar.Height;
        }

        public async Task RefreshPage()
        {
            await firestoreService.LoadDatabase();
            await googleService.LoadTodaysEvents();

            //recalculate goals/routines durations
            calculateDuration();

            SetupUI();
            PrintFirebaseUser();

        }

        public void calculateDuration()
        {
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
                }
            }

        }

        void DeletePageElements()
        {
            List<List<View>> ElementsRandE = new List<List<View>>();
            List<List<View>> ElementsGoals = new List<List<View>>();


            for (int i = 0; i < EventAndRoutineStackLayouts.Count; i++)
            {
                int skipNum = 0;
                ElementsRandE.Add(new List<View>());
                foreach (View element in EventAndRoutineStackLayouts[i].Children)
                    if (skipNum++ != 0)
                        ElementsRandE[i].Add(element);
            }

            for (int i = 0; i < GoalsStackLayouts.Count; i++)
            {
                ElementsGoals.Add(new List<View>());
                foreach (View element in GoalsStackLayouts[i].Children)
                    ElementsGoals[i].Add(element);
            }

            for (int i = 0; i < EventAndRoutineStackLayouts.Count; i++)
            {
                foreach (View element in ElementsRandE[i])
                    EventAndRoutineStackLayouts[i].Children.Remove(element);
            }

            for (int i = 0; i < EventAndRoutineStackLayouts.Count; i++)
            {
                foreach (View element in ElementsGoals[i])
                    GoalsStackLayouts[i].Children.Remove(element);
            }
        }

        private void PopulatePage()
        {
            //foreach (goal goal in user.goals)
            //    foreach (StackLayout stackLayout in GetInTimeOfDayList(goal.availableStartTime.TimeOfDay, goal.availableEndTime.TimeOfDay))
            //        PopulateGoal(goal, stackLayout);

            int goalIdx = 0;
            foreach (goal goal in user.goals)
                PopulateGoal(goal, goalIdx++, GetFirstInTimeOfDay("goal", goal.availableStartTime));

            PopulateEventsAndRoutines(0, 0);
        }

        private void PopulateEventsAndRoutines(int eventIdx, int routineIdx)
        {
            //Console.WriteLine("eventIdx: " + eventIdx);
            //Console.WriteLine("routineIdx: " + routineIdx);

            //Console.WriteLine("eventcount: " + user.CalendarEvents.Count);
            //Console.WriteLine("routinecount: " + user.routines.Count);

            if (user.CalendarEvents.Count == eventIdx && user.routines.Count == routineIdx)
                return;

            if (user.CalendarEvents.Count == eventIdx)
            {
                PopulateRoutine(user.routines[routineIdx], routineIdx, GetFirstInTimeOfDay("routine", user.routines[routineIdx].availableStartTime));
                PopulateEventsAndRoutines(eventIdx, ++routineIdx);
                return;
            }
            if (user.routines.Count == routineIdx)
            {
                PopulateEvent(user.CalendarEvents[eventIdx], GetFirstInTimeOfDay("routine", user.CalendarEvents[eventIdx].Start.DateTime.DateTime.TimeOfDay));
                PopulateEventsAndRoutines(++eventIdx, routineIdx);
                return;
            }

            if (user.routines[routineIdx].availableStartTime < user.CalendarEvents[eventIdx].Start.DateTime.TimeOfDay)
            {
                PopulateRoutine(user.routines[routineIdx], routineIdx, GetFirstInTimeOfDay("routine", user.routines[routineIdx].availableStartTime));
                PopulateEventsAndRoutines(eventIdx, ++routineIdx);
                return;
            }
            else
            {
                PopulateEvent(user.CalendarEvents[eventIdx], GetFirstInTimeOfDay("routine", user.CalendarEvents[eventIdx].Start.DateTime.DateTime.TimeOfDay));
                PopulateEventsAndRoutines(++eventIdx, routineIdx);
                return;
            }
        }

        private void PopulateRoutine(routine routine, int routineIdx, StackLayout stackLayout)
        {
            int stackLayoutIdx = stackLayout.Children.Count;

            StackLayout itemStackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal
            };

            Frame frame = new Frame
            {
                CornerRadius = 10,
                HasShadow = false,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 10, 10, 10),
                Margin = new Thickness(0, 2, 0, 2)
            };

            StackLayout stackLayoutOuter = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            StackLayout stackLayoutInner = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };

            Label routineTitleLabel = new Label
            {
                Text = routine.title,
                FontSize = 20,
                VerticalOptions = LayoutOptions.Start,
                FontFamily = labelFont

            };

            Label startTimeLabel = new Label
            {
                Text = "Starts at " + DateTime.ParseExact(routine.availableStartTime.ToString(), "HH:mm:ss", null).ToString("hh:mm tt", CultureInfo.GetCultureInfo("en-US")),
                FontSize = 10,
                TextColor = Color.DimGray,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontFamily = labelFont
            };

            Label expectedTimeLabel = new Label
            {
                Text = "Expected to take " + routine.expectedCompletionTime.TotalMinutes.ToString() + " minutes",
                FontSize = 10,
                TextColor = Color.DimGray,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = labelFont

            };

            CachedImage image = new CachedImage()
            {
                Source = routine.photo,
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                Transformations = new List<ITransformation>()
                {
                    new CircleTransformation(),
                },
            };

            var indicator = new ActivityIndicator { Color = Color.Gray, };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = image;

            stackLayoutInner.Children.Add(routineTitleLabel);
            stackLayoutInner.Children.Add(startTimeLabel);
            stackLayoutInner.Children.Add(expectedTimeLabel);

            stackLayoutOuter.Children.Add(stackLayoutInner);
            stackLayoutOuter.Children.Add(image);

            frame.Content = stackLayoutOuter;

            CachedImage checkmarkImage = new CachedImage()
            {
                Source = "",
                WidthRequest = 0,
                HeightRequest = 0,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            if (routine.isComplete || routine.isInProgress)
            {
                checkmarkImage.IsVisible = true;
                if (routine.isInProgress)
                    checkmarkImage.Source = "yellowclockicon.png ";
                else
                {
                    if (routine.isComplete)
                        checkmarkImage.Source = "greencheckmarkicon.png";
                }
                checkmarkImage.WidthRequest = 30;
                checkmarkImage.HeightRequest = 30;
            }

            itemStackLayout.Children.Add(checkmarkImage);
            itemStackLayout.Children.Add(frame);
            stackLayout.Children.Add(itemStackLayout);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {

                StackLayout updatedStackLayout = (StackLayout)s;
                updatedStackLayout.Children[0].WidthRequest = 30;
                updatedStackLayout.Children[0].HeightRequest = 30;
                ((CachedImage)updatedStackLayout.Children[0]).IsVisible = true;

                routineOnClick(routine, routineIdx, updatedStackLayout);


            };
            stackLayout.Children[stackLayoutIdx].GestureRecognizers.Add(tapGestureRecognizer);
        }

        private void PopulateEvent(EventsItems event_, StackLayout stackLayout)
        {
            Frame frame = new Frame
            {
                CornerRadius = 10,
                HasShadow = false,
                Padding = new Thickness(10, 10, 10, 10),
                Margin = new Thickness(0, 2, 0, 2),
                BackgroundColor = Color.Goldenrod
            };

            StackLayout stackLayoutOuter = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            StackLayout stackLayoutInner = new StackLayout
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };

            Label EventTimeLabel = new Label
            {
                Text = event_.Start.DateTime.ToString("h:mm tt") + " - " + event_.End.DateTime.ToString("h:mm tt"),
                FontSize = 10,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                FontFamily = labelFont
            };

            Label EventTitleLabel = new Label
            {
                Text = event_.EventName,
                FontSize = 20,
                VerticalOptions = LayoutOptions.StartAndExpand,
                FontFamily = labelFont

            };

            Label expectedTimeLabel = new Label
            {
                Text = event_.EventName,
                FontSize = 10,
                TextColor = Color.DimGray,
                VerticalOptions = LayoutOptions.EndAndExpand,
                FontFamily = labelFont

            };

            CachedImage image = new CachedImage()
            {
                Source = Xamarin.Forms.ImageSource.FromFile("eventIcon.jpg"),
                WidthRequest = 50,
                HeightRequest = 50,
                HorizontalOptions = LayoutOptions.End,
                Transformations = new List<ITransformation>()
                {
                    new CircleTransformation(),
                },
            };

            stackLayoutInner.Children.Add(EventTimeLabel);
            stackLayoutInner.Children.Add(EventTitleLabel);
            stackLayoutInner.Children.Add(expectedTimeLabel);

            stackLayoutOuter.Children.Add(stackLayoutInner);
            stackLayoutOuter.Children.Add(image);

            frame.Content = stackLayoutOuter;

            stackLayout.Children.Add(frame);
        }

        private void PopulateGoal(goal goal, int goalIdx, StackLayout stackLayout)
        {
            StackLayout goalStackLayout = new StackLayout()
            {
                Margin = new Thickness(0, 0, 10, 0)
            };

            Grid grid = new Grid();

            CachedImage image = new CachedImage()
            {
                Source = goal.photo,
                WidthRequest = 120,
                HeightRequest = 90,
                HorizontalOptions = LayoutOptions.Start,
                Transformations = new List<ITransformation>()
                {
                    new RoundedTransformation(30, 120, 90),
                },
            };
            if (goal.isComplete || goal.isInProgress)
            {
                image.Opacity = .6;
            }

            CachedImage inProgressImage = new CachedImage()
            {
                Source = Xamarin.Forms.ImageSource.FromFile("yellowclockicon.png"),
                WidthRequest = 70,
                HeightRequest = 70,
                IsVisible = goal.isInProgress,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            CachedImage checkmarkImage = new CachedImage()
            {
                Source = Xamarin.Forms.ImageSource.FromFile("greencheckmarkicon.png"),
                WidthRequest = 70,
                HeightRequest = 70,
                IsVisible = goal.isComplete && !goal.isInProgress,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label goalLabel = new Label
            {
                Text = goal.title,
                TextColor = Color.DimGray,
                FontFamily = labelFont,
                HorizontalOptions = LayoutOptions.Start,

            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                App.ListPageScrollPosY = mainScrollView.ScrollY;

                StackLayout updatedStackLayout = (StackLayout)s;
                Grid updatedGrid = (Grid)updatedStackLayout.Children[0];
                ((CachedImage)updatedGrid.Children[0]).Opacity = .6;

                goalOnClick(goal, goalIdx, updatedGrid);
            };
            goalStackLayout.GestureRecognizers.Add(tapGestureRecognizer);

            var indicator = new ActivityIndicator { Color = Color.Gray, };
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");
            indicator.BindingContext = image;

            grid.Children.Add(image, 0, 0);
            grid.Children.Add(checkmarkImage, 0, 0);
            grid.Children.Add(inProgressImage, 0, 0);

            goalStackLayout.Children.Add(grid);
            goalStackLayout.Children.Add(goalLabel);

            stackLayout.Children.Add(goalStackLayout);
        }

        private void putLayoutsIntoLists()
        {
            EventAndRoutineStackLayouts.Add(MorningREStackLayout);
            EventAndRoutineStackLayouts.Add(AfternoonREStackLayout);
            EventAndRoutineStackLayouts.Add(EveningREStackLayout);
            EventAndRoutineStackLayouts.Add(NightREStackLayout);

            GoalsStackLayouts.Add(MorningGoalsStackLayout);
            GoalsStackLayouts.Add(AfternoonGoalsStackLayout);
            GoalsStackLayouts.Add(EveningGoalsStackLayout);
            GoalsStackLayouts.Add(NightGoalsStackLayout);
        }

        private StackLayout GetFirstInTimeOfDay(string GorR, TimeSpan startTime)
        {
            //Console.WriteLine("startTime: " + startTime.ToString());

            //currentTime = testTime;

            if (startTime == new TimeSpan(0, 0, 0))
            {
                //Console.WriteLine("Morning");

                if (GorR == "routine")
                    return MorningREStackLayout;
                else
                    return MorningGoalsStackLayout;
            }

            if (morningStart <= startTime && startTime < morningEnd)
            {
                //Console.WriteLine("Morning");

                if (GorR == "routine")
                    return MorningREStackLayout;
                else
                    return MorningGoalsStackLayout;
            }
            if (afternoonStart <= startTime && startTime < afternoonEnd)
            {
                //Console.WriteLine("Afternoon");

                if (GorR == "routine")
                    return AfternoonREStackLayout;
                else
                    return AfternoonGoalsStackLayout;
            }
            if (eveningStart <= startTime && startTime < eveningEnd)
            {
                //Console.WriteLine("Evening");

                if (GorR == "routine")
                    return EveningREStackLayout;
                else
                    return EveningGoalsStackLayout;
            }
            if (nightStart <= startTime && startTime < nightEnd)
            {
                //Console.WriteLine("Night");

                if (GorR == "routine")
                    return NightREStackLayout;
                else
                    return NightGoalsStackLayout;
            }

            return new StackLayout();
        }

        private List<StackLayout> GetInTimeOfDayList(TimeSpan startTime, TimeSpan endTime)
        {
            List<StackLayout> result = new List<StackLayout>();

            //Console.WriteLine("startTime: " + startTime.ToString());
            //Console.WriteLine("endTime: " + endTime.ToString());
            if ((startTime < morningEnd && morningStart < endTime)
                || startTime == morningStart || morningEnd == endTime)
            {
                //Console.WriteLine("Morning");

                result.Add(MorningGoalsStackLayout);
            }
            if ((startTime < afternoonEnd && afternoonStart < endTime)
                || startTime == afternoonStart || afternoonEnd == endTime)
            {
                //Console.WriteLine("Afternoon");

                result.Add(AfternoonGoalsStackLayout);
            }
            if ((startTime < eveningEnd && eveningStart < endTime)
                || startTime == eveningStart || eveningEnd == endTime)
            {
                //Console.WriteLine("Evening");

                result.Add(EveningGoalsStackLayout);
            }
            if ((startTime < nightEnd && nightStart < endTime)
                || startTime == nightStart || nightEnd == endTime)
            {
                //Console.WriteLine("Night");

                result.Add(NightGoalsStackLayout);
            }

            return result;
        }

        private StackLayout GetCompleteTimeOfDay(TimeSpan completeTime)
        {
            //Console.WriteLine("completeTime: " + completeTime.ToString());
            if (morningStart < completeTime && completeTime < morningEnd)
            {
                //Console.WriteLine("Morning");

                return MorningREStackLayout;
            }
            if (afternoonStart < completeTime && completeTime < afternoonEnd)
            {
                //Console.WriteLine("Afternoon");

                return AfternoonREStackLayout;
            }
            if (eveningStart < completeTime && completeTime < eveningEnd)
            {
                //Console.WriteLine("Evening");

                return EveningREStackLayout;
            }
            if (nightStart < completeTime && completeTime < nightEnd)
            {
                //Console.WriteLine("Night");

                return NightREStackLayout;
            }

            return new StackLayout();
        }

        internal void StartTimer()
        {
            //int seconds = 10 * 1000;

            //var timer =
            //    new Timer(TimerReset, null, 0, seconds);
        }

        private void TimerReset(object o)
        {
            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    RefreshPage();
            //});
        }

        public async void PrepareRefreshEvents()
        {
            await Task.Delay(1000);
            dateTimeNow = DateTime.Now;
            await RefreshPage();
        }

        public async void routineOnClick(routine routine, int routineIdx, StackLayout updatedStackLayout)
        {
            if (routine.isSublistAvailable)
            {
                if (!routine.isInProgress && !routine.isComplete)
                {
                    ((CachedImage)updatedStackLayout.Children[0]).Source = "yellowclockicon.png";
                    routine.isInProgress = true;
                    firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", false);
                }
                App.ListPageScrollPosY = mainScrollView.ScrollY;
                await Navigation.PushAsync(new TaskPage(routineIdx, true));
            }
            else
            {
                if (!routine.isComplete)
                {

                    if (routine.isInProgress)
                    {
                        ((CachedImage)updatedStackLayout.Children[0]).Source = "greencheckmarkicon.png";
                        routine.isInProgress = false;
                        routine.isComplete = true;

                        firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", true);
                    }
                    else
                    {
                        ((CachedImage)updatedStackLayout.Children[0]).Source = "yellowclockicon.png";
                        routine.isInProgress = true;
                        routine.isComplete = false;
                        firebaseFunctionsService.updateGratisStatus(routine, "goals&routines", false);
                    }
                }
            }
        }

        public async void goalOnClick(goal goal, int goalIdx, Grid updatedGrid)
        {
            if (goal.isSublistAvailable)
            {

                if (!goal.isInProgress && !goal.isComplete)
                {
                    updatedGrid.Children[2].IsVisible = true;
                    goal.isInProgress = true;
                    firebaseFunctionsService.updateGratisStatus(goal, "goals&routines", false);
                }
                App.ListPageScrollPosY = mainScrollView.ScrollY;
                await Navigation.PushAsync(new TaskPage(goalIdx, false));
            }
            else
            {
                if (!goal.isComplete)
                {

                    if (goal.isInProgress)
                    {
                        updatedGrid.Children[1].IsVisible = true;
                        updatedGrid.Children[2].IsVisible = false;
                        goal.isInProgress = false;
                        goal.isComplete = true;

                        firebaseFunctionsService.updateGratisStatus(goal, "goals&routines", true);
                    }
                    else
                    {
                        updatedGrid.Children[2].IsVisible = true;
                        goal.isInProgress = true;
                        goal.isComplete = false;
                        firebaseFunctionsService.updateGratisStatus(goal, "goals&routines", false);
                    }
                }
            }
        }

        private void AddTapGestures()
        {
            //var tapGestureRecognizer1 = new TapGestureRecognizer();
            //tapGestureRecognizer1.Tapped += async (s, e) =>
            //{
            //    ReloadImage.GestureRecognizers.RemoveAt(0);
            //    ReloadImage.Source = "redoGray.png";
            //    await RefreshPage();
            //    ReloadImage.Source = "redo.png";
            //    ReloadImage.GestureRecognizers.Add(tapGestureRecognizer1);
            //};
            //ReloadImage.GestureRecognizers.Add(tapGestureRecognizer1);

            var tapGestureRecognizer2 = new TapGestureRecognizer();
            tapGestureRecognizer2.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new GreetingPage());
            };
            AboutMeButton.GestureRecognizers.Add(tapGestureRecognizer2);

            var tapGestureRecognizer3 = new TapGestureRecognizer();
            tapGestureRecognizer3.Tapped += async (s, e) => {
                UserDialogs.Instance.ShowLoading("Loading...");
                if (App.User.photoURIs.Count < 1)
                    await GooglePhotoService.GetPhotos();
                await Navigation.PushAsync(new MonthlyViewPage());
                UserDialogs.Instance.HideLoading();
            };
            MyPhotosButton.GestureRecognizers.Add(tapGestureRecognizer3);

            var tapGestureRecognizer4 = new TapGestureRecognizer();
            tapGestureRecognizer4.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
            };
            MyDayButton.GestureRecognizers.Add(tapGestureRecognizer4);
        }

        void PrintFirebaseUser()
        {
            OnPropertyChanged(nameof(App.User));
            //Console.WriteLine("user first name: " + App.User.firstName);
            //Console.WriteLine("user last name: " + App.User.lastName);

            foreach (routine routine in App.User.routines)
            {
                OnPropertyChanged(nameof(routine));
                //Console.WriteLine("user routine title: " + routine.title);
                //Console.WriteLine("user routine id: " + routine.id);
                foreach (task task in routine.tasks)
                {
                    OnPropertyChanged(nameof(task));
                    //Console.WriteLine("user task title: " + task.title);
                    //Console.WriteLine("user task id: " + task.id);
                    foreach (step step in task.steps)
                    {
                        OnPropertyChanged(nameof(step));
                        //Console.WriteLine("user step title: " + step.title);
                    }
                }
            }

            foreach (goal goal in App.User.goals)
            {
                OnPropertyChanged(nameof(goal));
                //Console.WriteLine("user goal title: " + goal.title);
                //Console.WriteLine("user goal id: " + goal.id);
                foreach (action action in goal.actions)
                {
                    OnPropertyChanged(nameof(goal));
                    //Console.WriteLine("user action title: " + action.title);
                    //Console.WriteLine("user action id: " + action.id);
                    foreach (instruction instruction in action.instructions)
                    {
                        OnPropertyChanged(nameof(instruction));
                        //Console.WriteLine("user instruction title: " + instruction.title);
                    }
                }
            }
        }
    }
}
