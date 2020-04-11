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

namespace ProjectCaitlin.ViewModel
{
    public class GoalsRoutinesTemplateViewModel : BindableObject
    {
        private GoalsRoutinesTemplate mainPage;
        List<bool> complete;
        private int _currentIndex;
        private int _imageCount = 1078;
        public ObservableCollection<object> Items { get; set; }

        public ICommand AboutMeCommand { private set; get; }
        public string DayLabel { get; set; }
        public string TimeLabel { get; set; }
        public string DayImage { get; set; }
        public GoalsRoutinesTemplateViewModel(GoalsRoutinesTemplate mainPage)
        {
            this.mainPage = mainPage;

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
                Source = "caitlinheadshot.jpg",
                Title = "About Me",
                Ind = _imageCount++,
                BackgroundColor = Color.Default,
                TextColor = Color.Black,
                Length = "You are a strong, independant person. \n This App helps you take control of your life!",
                Text = "Tap to Learn More",
                ButtonText = "Click for More About Me",
                Navigate = AboutMeCommand
            });

            int routineNum = 0;
            foreach (routine routine in App.User.routines)
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in routine.tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += step.expected_completion_time;
                    }
                }
                // set the duration for routine
                if (sum_duration != 0)
                    routine.expected_completion_time = sum_duration;

                complete.Add(false);
                Items.Add(new GRItemModel(
                   routine.photo,
                   routine.title,
                   Color.Default,
                   Color.Black,
                   "Tap to start",
                   "Takes me " + routine.expected_completion_time + " minutes",

                   new Command<int>(
                        async (int _GRIdx) =>
                        {
                            Console.WriteLine("GRIDX :" + _GRIdx);
                            await mainPage.Navigation.PushAsync(new TaskPage(_GRIdx, true));
                        }),
                   routineNum
                    ));
                routineNum++;
            }

            int goalNum = 0;
            foreach (goal goal in App.User.goals)
            {
                complete.Add(false);
                Items.Add(new GRItemModel(
                   goal.photo,
                   goal.title,
                   Color.Black,
                   Color.White,
                   "Tap to start",
                   "Takes me 30 minutes",

                   new Command<int>(
                        async (int _GRIdx) =>
                        {
                            Console.WriteLine("GRIDX :" + _GRIdx);
                            await mainPage.Navigation.PushAsync(new TaskPage(_GRIdx, false));
                        }),
                   goalNum
                    ));
                goalNum++;
            }

            /*int goalNum = 0;
            foreach (goal goal in App.User.goals)
            {
                Console.WriteLine("goal Num : " + goalNum);
                Items.Add(new
                {
                    Source = goal.photo,
                    Title = goal.title,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,
                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(goalNum, false));
                     })
                });
                goalNum++;
            }

            PanPositionChangedCommand = new Command(v =>
            {
                if (IsAutoAnimationRunning || IsUserInteractionRunning)
                {
                    return;
                }

                var index = CurrentIndex + (bool.Parse(v.ToString()) ? 1 : -1);
                if (index < 0 || index >= Items.Count)
                {
                    return;
                }
                CurrentIndex = index;
            });

            RemoveCurrentItemCommand = new Command(() =>
            {
                if (!Items.Any())
                {
                    return;
                }
                Items.RemoveAt(CurrentIndex.ToCyclicalIndex(Items.Count));
            });

            GoToLastCommand = new Command(() =>
            {
                CurrentIndex = Items.Count - 1;
            });*/
        }

        public ICommand PanPositionChangedCommand { get; }

        public ICommand RemoveCurrentItemCommand { get; }

        public ICommand GoToLastCommand { get; }


        private bool isInTimeRange(TimeSpan start, TimeSpan end)
        {
            DateTime dateTimeNow = DateTime.Now;
            //DateTime dateTimeNow = new DateTime(1999, 12, 1, 23, 59, 59);
            if (start <= dateTimeNow.TimeOfDay.Add(TimeSpan.FromHours(4)) && dateTimeNow.TimeOfDay <= end)
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

        public GRItemModel(string _source, string _title, Color _backgroundColor, Color _textColor, string _text, string _length, Command<int> _navigate, int _navigateIdx)
        {
            source = _source;
            title = _title;
            backgroundColor = _backgroundColor;
            textColor = _textColor;
            text = _text;
            length = _length;
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
