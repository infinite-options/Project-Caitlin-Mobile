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

namespace ProjectCaitlin.ViewModel
{
    public class GoalsRoutinesTemplateViewModel : BindableObject
    {
        private GoalsRoutinesTemplate mainPage;
        List<bool> complete;
        //public event PropertyChangedEventHandler PropertyChanged;

        private int _currentIndex;
        private int _imageCount = 1078;

        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public ICommand AboutMeCommand { private set; get; }
        public ICommand RoutineCommand { private set; get; }

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

            if (App.User.routines.Count >= 1 && !App.User.routines[0].isComplete && isInTimeRange(App.User.routines[0].availableStartTime.TimeOfDay, App.User.routines[0].availableEndTime.TimeOfDay))
            {
                complete.Add(false);

                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[0].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[0].expectedCompletionTime.TotalMinutes)
                    App.User.routines[0].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);

                Items.Add(new
                {
                    Source = App.User.routines[0].photo,
                    Title = App.User.routines[0].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,

                    Length = "Takes me " + App.User.routines[0].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(0, true));
                     })
                });
            }
            if (App.User.routines.Count >= 2 && !App.User.routines[1].isComplete && isInTimeRange(App.User.routines[1].availableStartTime.TimeOfDay, App.User.routines[1].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[1].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[1].expectedCompletionTime.TotalMinutes)
                    App.User.routines[1].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;

                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[1].photo,
                    Title = App.User.routines[1].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[1].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(1, true));
                     })
                });
            }
            if (App.User.routines.Count >= 3 && !App.User.routines[2].isComplete && isInTimeRange(App.User.routines[2].availableStartTime.TimeOfDay, App.User.routines[2].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 2;
                foreach (task task in App.User.routines[2].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[2].expectedCompletionTime.TotalMinutes)
                    App.User.routines[2].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;

                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[2].photo,
                    Title = App.User.routines[2].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[2].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(2, true));
                     })
                });
            }
            if (App.User.routines.Count >= 4 && !App.User.routines[3].isComplete && isInTimeRange(App.User.routines[3].availableStartTime.TimeOfDay, App.User.routines[3].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[3].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[3].expectedCompletionTime.TotalMinutes)
                    App.User.routines[3].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[3].photo,
                    Title = App.User.routines[3].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[3].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(3, true));
                     })
                });
            }
            if (App.User.routines.Count >= 5 && !App.User.routines[4].isComplete && isInTimeRange(App.User.routines[4].availableStartTime.TimeOfDay, App.User.routines[4].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[4].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[4].expectedCompletionTime.TotalMinutes)
                    App.User.routines[4].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[4].photo,
                    Title = App.User.routines[4].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[4].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(4, true));
                     })
                });
            }
            if (App.User.routines.Count >= 6 && !App.User.routines[5].isComplete && isInTimeRange(App.User.routines[5].availableStartTime.TimeOfDay, App.User.routines[5].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[5].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[5].expectedCompletionTime.TotalMinutes)
                    App.User.routines[5].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[5].photo,
                    Title = App.User.routines[5].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[5].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(5, true));
                     })
                });
            }
            if (App.User.routines.Count >= 7 && !App.User.routines[6].isComplete && isInTimeRange(App.User.routines[6].availableStartTime.TimeOfDay, App.User.routines[6].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[6].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[6].expectedCompletionTime.TotalMinutes)
                    App.User.routines[6].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[6].photo,
                    Title = App.User.routines[6].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[6].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(6, true));
                     })
                });
            }
            if (App.User.routines.Count >= 8 && !App.User.routines[7].isComplete && isInTimeRange(App.User.routines[7].availableStartTime.TimeOfDay, App.User.routines[7].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[7].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[7].expectedCompletionTime.TotalMinutes)
                    App.User.routines[7].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[7].photo,
                    Title = App.User.routines[7].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[7].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(7, true));
                     })
                });
            }

            if (App.User.routines.Count >= 9 && !App.User.routines[8].isComplete && isInTimeRange(App.User.routines[8].availableStartTime.TimeOfDay, App.User.routines[8].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[8].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[8].expectedCompletionTime.TotalMinutes)
                    App.User.routines[8].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[8].photo,
                    Title = App.User.routines[8].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[8].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(8, true));
                     })
                });
            }

            if (App.User.routines.Count >= 10 && !App.User.routines[9].isComplete && isInTimeRange(App.User.routines[9].availableStartTime.TimeOfDay, App.User.routines[9].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[9].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[9].expectedCompletionTime.TotalMinutes)
                    App.User.routines[9].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[9].photo,
                    Title = App.User.routines[9].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[9].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(9, true));
                         })
                });
            }

            if (App.User.routines.Count >= 11 && !App.User.routines[10].isComplete && isInTimeRange(App.User.routines[10].availableStartTime.TimeOfDay, App.User.routines[10].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[10].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[10].expectedCompletionTime.TotalMinutes)
                    App.User.routines[10].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);

                Items.Add(new
                {
                    Source = App.User.routines[10].photo,
                    Title = App.User.routines[10].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[10].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(10, true));
                     })
                });
            }
            if (App.User.routines.Count >= 12 && !App.User.routines[11].isComplete && isInTimeRange(App.User.routines[11].availableStartTime.TimeOfDay, App.User.routines[11].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[11].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[11].expectedCompletionTime.TotalMinutes)
                    App.User.routines[11].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[11].photo,
                    Title = App.User.routines[11].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[11].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(11, true));
                     })
                });
            }
            if (App.User.routines.Count >= 13 && !App.User.routines[12].isComplete && isInTimeRange(App.User.routines[12].availableStartTime.TimeOfDay, App.User.routines[12].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[12].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[12].expectedCompletionTime.TotalMinutes)
                    App.User.routines[12].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[12].photo,
                    Title = App.User.routines[12].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[12].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(12, true));
                     })
                });
            }
            if (App.User.routines.Count >= 14 && !App.User.routines[13].isComplete && isInTimeRange(App.User.routines[13].availableStartTime.TimeOfDay, App.User.routines[13].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[13].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[13].expectedCompletionTime.TotalMinutes)
                    App.User.routines[13].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[13].photo,
                    Title = App.User.routines[13].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[13].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(13, true));
                     })
                });
            }
            if (App.User.routines.Count >= 15 && !App.User.routines[14].isComplete && isInTimeRange(App.User.routines[14].availableStartTime.TimeOfDay, App.User.routines[14].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[14].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[14].expectedCompletionTime.TotalMinutes)
                    App.User.routines[14].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[14].photo,
                    Title = App.User.routines[14].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[14].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(14, true));
                     })
                });
            }
            if (App.User.routines.Count >= 16 && !App.User.routines[15].isComplete && isInTimeRange(App.User.routines[15].availableStartTime.TimeOfDay, App.User.routines[15].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[15].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[15].expectedCompletionTime.TotalMinutes)
                    App.User.routines[15].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[15].photo,
                    Title = App.User.routines[15].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[15].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(15, true));
                     })
                });
            }
            if (App.User.routines.Count >= 17 && !App.User.routines[16].isComplete && isInTimeRange(App.User.routines[16].availableStartTime.TimeOfDay, App.User.routines[16].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[16].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[16].expectedCompletionTime.TotalMinutes)
                    App.User.routines[16].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[16].photo,
                    Title = App.User.routines[16].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[16].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(16, true));
                     })
                });
            }
            if (App.User.routines.Count >= 18 && !App.User.routines[17].isComplete && isInTimeRange(App.User.routines[17].availableStartTime.TimeOfDay, App.User.routines[17].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[17].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[17].expectedCompletionTime.TotalMinutes)
                    App.User.routines[17].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[17].photo,
                    Title = App.User.routines[17].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[17].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(17, true));
                     })
                });
            }
            if (App.User.routines.Count >= 19 && !App.User.routines[18].isComplete && isInTimeRange(App.User.routines[18].availableStartTime.TimeOfDay, App.User.routines[18].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[18].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[18].expectedCompletionTime.TotalMinutes)
                    App.User.routines[18].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[18].photo,
                    Title = App.User.routines[18].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[18].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(18, true));
                         })
                });
            }
            if (App.User.routines.Count >= 20 && !App.User.routines[19].isComplete && isInTimeRange(App.User.routines[19].availableStartTime.TimeOfDay, App.User.routines[19].availableEndTime.TimeOfDay))
            {
                //calculate the sum duration for the routine from step level.
                int sum_duration = 0;
                foreach (task task in App.User.routines[19].tasks)
                {
                    foreach (step step in task.steps)
                    {
                        sum_duration += (int) step.expectedCompletionTime.TotalMinutes;
                    }
                }
                // choose the greater of the two.
                if (sum_duration > App.User.routines[19].expectedCompletionTime.TotalMinutes)
                    App.User.routines[19].expectedCompletionTime = TimeSpan.FromMinutes(sum_duration);;
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.routines[19].photo,
                    Title = App.User.routines[19].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + App.User.routines[19].expectedCompletionTime.TotalMinutes + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(19, true));
                         })
                });
            }
            if (App.User.goals.Count >= 1 && !App.User.goals[0].isComplete && isInTimeRange(App.User.goals[0].availableStartTime.TimeOfDay, App.User.goals[0].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[0].photo,
                    Title = App.User.goals[0].title,

                    Ind = _imageCount++,
                    TextColor = Color.White,
                    BackgroundColor = "#272E32",
                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(0, false));
                         })
                });
            }

            if (App.User.goals.Count >= 2 && !App.User.goals[1].isComplete && isInTimeRange(App.User.goals[1].availableStartTime.TimeOfDay, App.User.goals[1].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[1].photo,
                    Title = App.User.goals[1].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(1, false));
                         })
                });
            }
            if (App.User.goals.Count >= 3 && !App.User.goals[2].isComplete && isInTimeRange(App.User.goals[2].availableStartTime.TimeOfDay, App.User.goals[2].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[2].photo,
                    Title = App.User.goals[2].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(2, false));
                         })
                });
            }
            if (App.User.goals.Count >= 4 && !App.User.goals[3].isComplete && isInTimeRange(App.User.goals[3].availableStartTime.TimeOfDay, App.User.goals[3].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[3].photo,
                    Title = App.User.goals[3].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(3, false));
                         })
                });
            }
            if (App.User.goals.Count >= 5 && !App.User.goals[4].isComplete && isInTimeRange(App.User.goals[4].availableStartTime.TimeOfDay, App.User.goals[4].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[4].photo,
                    Title = App.User.goals[4].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(4, false));
                         })
                });
            }
            if (App.User.goals.Count >= 6 && !App.User.goals[5].isComplete && isInTimeRange(App.User.goals[5].availableStartTime.TimeOfDay, App.User.goals[5].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[5].photo,
                    Title = App.User.goals[5].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(5, false));
                         })
                });
            }
            if (App.User.goals.Count >= 7 && !App.User.goals[6].isComplete && isInTimeRange(App.User.goals[6].availableStartTime.TimeOfDay, App.User.goals[6].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[6].photo,
                    Title = App.User.goals[6].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(6, false));
                         })
                });
            }
            if (App.User.goals.Count >= 8 && !App.User.goals[7].isComplete && isInTimeRange(App.User.goals[7].availableStartTime.TimeOfDay, App.User.goals[7].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[7].photo,
                    Title = App.User.goals[7].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(7, false));
                         })
                });
            }
            if (App.User.goals.Count >= 9 && !App.User.goals[8].isComplete && isInTimeRange(App.User.goals[8].availableStartTime.TimeOfDay, App.User.goals[8].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[8].photo,
                    Title = App.User.goals[8].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(8, false));
                         })
                });
            }
            if (App.User.goals.Count >= 10 && !App.User.goals[9].isComplete && isInTimeRange(App.User.goals[9].availableStartTime.TimeOfDay, App.User.goals[9].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[9].photo,
                    Title = App.User.goals[9].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(9, false));
                         })
                });
            }
            if (App.User.goals.Count >= 11 && !App.User.goals[10].isComplete && isInTimeRange(App.User.goals[10].availableStartTime.TimeOfDay, App.User.goals[10].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[10].photo,
                    Title = App.User.goals[10].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(10, false));
                         })
                });
            }
            if (App.User.goals.Count >= 12 && !App.User.goals[11].isComplete && isInTimeRange(App.User.goals[11].availableStartTime.TimeOfDay, App.User.goals[11].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[11].photo,
                    Title = App.User.goals[11].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(11, false));
                         })
                });
            }
            if (App.User.goals.Count >= 13 && !App.User.goals[12].isComplete && isInTimeRange(App.User.goals[12].availableStartTime.TimeOfDay, App.User.goals[12].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[12].photo,
                    Title = App.User.goals[12].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(12, false));
                         })
                });
            }
            if (App.User.goals.Count >= 14 && !App.User.goals[13].isComplete && isInTimeRange(App.User.goals[13].availableStartTime.TimeOfDay, App.User.goals[13].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[13].photo,
                    Title = App.User.goals[13].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(13, false));
                         })
                });
            }
            if (App.User.goals.Count >= 15 && !App.User.goals[14].isComplete && isInTimeRange(App.User.goals[14].availableStartTime.TimeOfDay, App.User.goals[14].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[14].photo,
                    Title = App.User.goals[14].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(14, false));
                         })
                });
            }
            if (App.User.goals.Count >= 16 && !App.User.goals[15].isComplete && isInTimeRange(App.User.goals[15].availableStartTime.TimeOfDay, App.User.goals[15].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[15].photo,
                    Title = App.User.goals[15].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(15, false));
                         })
                });
            }
            if (App.User.goals.Count >= 17 && !App.User.goals[16].isComplete && isInTimeRange(App.User.goals[16].availableStartTime.TimeOfDay, App.User.goals[16].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[16].photo,
                    Title = App.User.goals[16].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(16, false));
                         })
                });
            }
            if (App.User.goals.Count >= 18 && !App.User.goals[17].isComplete && isInTimeRange(App.User.goals[17].availableStartTime.TimeOfDay, App.User.goals[17].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[17].photo,
                    Title = App.User.goals[17].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(17, false));
                         })
                });
            }
            if (App.User.goals.Count >= 19 && !App.User.goals[18].isComplete && isInTimeRange(App.User.goals[18].availableStartTime.TimeOfDay, App.User.goals[18].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[18].photo,
                    Title = App.User.goals[18].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(18, false));
                         })
                });
            }
            if (App.User.goals.Count >= 20 && !App.User.goals[19].isComplete && isInTimeRange(App.User.goals[19].availableStartTime.TimeOfDay, App.User.goals[19].availableEndTime.TimeOfDay))
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.User.goals[19].photo,
                    Title = App.User.goals[19].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(19, false));
                         })
                });
            }

            /*int routineNum = 0;
            foreach (routine routine in App.User.routines)
            {
                Console.WriteLine("routine Num : " + routineNum);
                Items.Add(new
                {
                    Source = routine.photo,
                    Title = routine.title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me " + " minutes",
                    Text = "Tap to start",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(routineNum,true));
                     })
                });
                routineNum++;
            }

            int goalNum = 0;
            foreach (goal goal in App.User.goals)
            {
                Console.WriteLine("goal Num : " + goalNum);
                Items.Add(new
                {
                    Source = goal.photo,
                    Title = goal.title,
                    Ind = _imageCount++,
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
            }*/

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
            });
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

        public ObservableCollection<object> Items { get; }

    }
}
