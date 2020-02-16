using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ProjectCaitlin.ViewModel
{
    public class TaskGridViewModel : BindableObject
    {
        private TaskPage mainPage;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string RoutineImage { get; set; }
        public string RoutineLabel { get; set; }

        public ICommand NavigateCommand { private set; get; }


        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public TaskGridViewModel(TaskPage mainPage, int routineNum )
        {

            this.mainPage = mainPage;


            RoutineImage = App.user.routines[routineNum].photo;
            RoutineLabel = App.user.routines[routineNum].title;

            if (App.user.routines[routineNum].tasks.Count>=1)
            _items.Add(new
            {
                Source = App.user.routines[routineNum].tasks[0].photo,
                Text = App.user.routines[routineNum].tasks[0].title,
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 0));
                     })
            });
            if (App.user.routines[routineNum].tasks.Count >= 2)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[1].photo,
                    Text = App.user.routines[routineNum].tasks[1].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 1));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 3)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[2].photo,
                    Text = App.user.routines[routineNum].tasks[2].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 2));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 4)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[3].photo,
                    Text = App.user.routines[routineNum].tasks[3].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 3));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 5)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[4].photo,
                    Text = App.user.routines[routineNum].tasks[4].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 4));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 6)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[5].photo,
                    Text = App.user.routines[routineNum].tasks[5].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 5));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 7)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[6].photo,
                    Text = App.user.routines[routineNum].tasks[6].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 6));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 8)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[7].photo,
                    Text = App.user.routines[routineNum].tasks[7].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 7));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 9)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[8].photo,
                    Text = App.user.routines[routineNum].tasks[8].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 8));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 10)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[9].photo,
                    Text = App.user.routines[routineNum].tasks[9].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 9));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 11)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[10].photo,
                    Text = App.user.routines[routineNum].tasks[10].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 10));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 12)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[11].photo,
                    Text = App.user.routines[routineNum].tasks[11].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 11));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 13)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[12].photo,
                    Text = App.user.routines[routineNum].tasks[12].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 12));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 14)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[13].photo,
                    Text = App.user.routines[routineNum].tasks[13].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 13));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 15)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[14].photo,
                    Text = App.user.routines[routineNum].tasks[14].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 14));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 16)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[15].photo,
                    Text = App.user.routines[routineNum].tasks[15].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 15));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 17)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[16].photo,
                    Text = App.user.routines[routineNum].tasks[16].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 16));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 18)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[17].photo,
                    Text = App.user.routines[routineNum].tasks[17].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 17));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 19)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[18].photo,
                    Text = App.user.routines[routineNum].tasks[18].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 18));
                         })
                });
            if (App.user.routines[routineNum].tasks.Count >= 20)
                _items.Add(new
                {
                    Source = App.user.routines[routineNum].tasks[19].photo,
                    Text = App.user.routines[routineNum].tasks[19].title,
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskCompletePage(routineNum, 19));
                         })
                });

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