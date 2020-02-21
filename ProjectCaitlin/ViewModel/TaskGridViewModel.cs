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

        public bool IsComplete { get; set; }
        public ICommand NavigateCommand { private set; get; }

        List<bool> complete;

        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public TaskGridViewModel(TaskPage mainPage, int a, bool isRoutine, List<bool> complete)
        {
            this.complete = complete;
            this.mainPage = mainPage;

            if (isRoutine)
            {
                TopImage = App.user.routines[a].photo;
                TopLabel = App.user.routines[a].title;
            }
            else {
                TopImage = App.user.goals[a].photo;
                TopLabel = App.user.goals[a].title;
            }
            

            if (isRoutine)
            {
                if (App.user.routines[a].tasks.Count >= 1)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[0].photo,
                        Text = App.user.routines[a].tasks[0].title,
                        IsComplete = complete[0],
                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 0, isRoutine, complete));
                             })
                    }); ;



                if (App.user.routines[a].tasks.Count >= 2)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[1].photo,
                        Text = App.user.routines[a].tasks[1].title,
                        IsComplete = complete[1],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 1, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 3)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[2].photo,
                        Text = App.user.routines[a].tasks[2].title,
                        IsComplete = complete[2],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 2, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 4)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[3].photo,
                        Text = App.user.routines[a].tasks[3].title,
                        IsComplete = complete[3],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 3, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 5)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[4].photo,
                        Text = App.user.routines[a].tasks[4].title,
                        IsComplete = complete[4],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 4, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 6)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[5].photo,
                        Text = App.user.routines[a].tasks[5].title,
                        IsComplete = complete[5],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 5, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 7)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[6].photo,
                        Text = App.user.routines[a].tasks[6].title,
                        IsComplete = complete[6],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 6, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 8)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[7].photo,
                        Text = App.user.routines[a].tasks[7].title,
                        IsComplete = complete[7],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 7, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 9)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[8].photo,
                        Text = App.user.routines[a].tasks[8].title,
                        IsComplete = complete[8],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 8, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 10)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[9].photo,
                        Text = App.user.routines[a].tasks[9].title,
                        IsComplete = complete[9],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 9, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 11)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[10].photo,
                        Text = App.user.routines[a].tasks[10].title,
                        IsComplete = complete[10],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 10, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 12)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[11].photo,
                        Text = App.user.routines[a].tasks[11].title,
                        IsComplete = complete[11],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 11, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 13)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[12].photo,
                        Text = App.user.routines[a].tasks[12].title,
                        IsComplete = complete[12],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 12, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 14)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[13].photo,
                        Text = App.user.routines[a].tasks[13].title,
                        IsComplete = complete[13],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 13, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 15)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[14].photo,
                        Text = App.user.routines[a].tasks[14].title,
                        IsComplete = complete[14],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 14, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 16)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[15].photo,
                        Text = App.user.routines[a].tasks[15].title,
                        IsComplete = complete[15],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 15, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 17)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[16].photo,
                        Text = App.user.routines[a].tasks[16].title,
                        IsComplete = complete[16],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 16, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 18)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[17].photo,
                        Text = App.user.routines[a].tasks[17].title,
                        IsComplete = complete[17],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 17, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 19)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[18].photo,
                        Text = App.user.routines[a].tasks[18].title,
                        IsComplete = complete[18],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 18, isRoutine, complete));
                             })
                    });
                if (App.user.routines[a].tasks.Count >= 20)
                    _items.Add(new
                    {
                        Source = App.user.routines[a].tasks[19].photo,
                        Text = App.user.routines[a].tasks[19].title,
                        IsComplete = complete[19],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new StepsPage(a, 19, isRoutine, complete));
                             })
                    });
            }
            else {
                if (App.user.goals[a].actions.Count >= 1)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[0].photo,
                        Text = App.user.goals[a].actions[0].title,
                        IsComplete = complete[0],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 0, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 2)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[1].photo,
                        Text = App.user.goals[a].actions[1].title,
                        IsComplete = complete[1],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 1, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 3)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[2].photo,
                        Text = App.user.goals[a].actions[2].title,
                        IsComplete = complete[2],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 2, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 4)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[3].photo,
                        Text = App.user.goals[a].actions[3].title,
                        IsComplete = complete[3],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 3, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 5)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[4].photo,
                        Text = App.user.goals[a].actions[4].title,
                        IsComplete = complete[4],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 4, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 6)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[5].photo,
                        Text = App.user.goals[a].actions[5].title,
                        IsComplete = complete[5],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 5, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 7)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[6].photo,
                        Text = App.user.goals[a].actions[6].title,
                        IsComplete = complete[6],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 6, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 8)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[7].photo,
                        Text = App.user.goals[a].actions[7].title,
                        IsComplete = complete[7],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 7, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 9)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[8].photo,
                        Text = App.user.goals[a].actions[8].title,
                        IsComplete = complete[8],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 8, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 10)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[9].photo,
                        Text = App.user.goals[a].actions[9].title,
                        IsComplete = complete[9],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 9, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 11)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[10].photo,
                        Text = App.user.goals[a].actions[10].title,
                        IsComplete = complete[10],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 10, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 12)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[11].photo,
                        Text = App.user.goals[a].actions[11].title,
                        IsComplete = complete[11],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 11, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 13)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[12].photo,
                        Text = App.user.goals[a].actions[12].title,
                        IsComplete = complete[12],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 12, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 14)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[13].photo,
                        Text = App.user.goals[a].actions[13].title,
                        IsComplete = complete[13],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 13, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 15)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[14].photo,
                        Text = App.user.goals[a].actions[14].title,
                        IsComplete = complete[14],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 14, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 16)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[15].photo,
                        Text = App.user.goals[a].actions[15].title,
                        IsComplete = complete[15],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 15, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 17)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[16].photo,
                        Text = App.user.goals[a].actions[16].title,
                        IsComplete = complete[16],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 16, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 18)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[17].photo,
                        Text = App.user.goals[a].actions[17].title,
                        IsComplete = complete[17],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 17, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 19)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[18].photo,
                        Text = App.user.goals[a].actions[18].title,
                        IsComplete = complete[18],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 18, isRoutine, complete));
                             })
                    });
                if (App.user.goals[a].actions.Count >= 20)
                    _items.Add(new
                    {
                        Source = App.user.goals[a].actions[19].photo,
                        Text = App.user.goals[a].actions[19].title,
                        IsComplete = complete[19],

                        Navigate = new Command(
                             async () =>
                             {
                                 await mainPage.Navigation.PushAsync(new TaskCompletePage(a, 19, isRoutine, complete));
                             })
                    });


            }
            /*int taskNum = 0;
            if(App.user.routines[routineNum].tasks.Count!=0)
            foreach (task task in App.user.routines[routineNum].tasks)
            {
                _items.Add(new { Source = task.photo, Text = task.title ,Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum,taskNum));
                     })
                });
                    taskNum++;
            }*/

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