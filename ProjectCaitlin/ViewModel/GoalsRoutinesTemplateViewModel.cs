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


namespace ProjectCaitlin.ViewModel
{
    public class GoalsRoutinesTemplateViewModel : BindableObject
    {
        private GoalsRoutinesTemplate mainPage;

        //public event PropertyChangedEventHandler PropertyChanged;

        private int _currentIndex;
        private int _imageCount = 1078;

        private ObservableCollection<object> _items = new ObservableCollection<object>() { };
        public ICommand AboutMeCommand { private set; get; }
        public ICommand RoutineCommand { private set; get; }


        public GoalsRoutinesTemplateViewModel(GoalsRoutinesTemplate mainPage)
        {
            this.mainPage = mainPage;

            Items = new ObservableCollection<object>();

            /*Items = new ObservableCollection<object>
            {
                new { Source = "toothbrushCircle.png",
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Title = "Good Morning, Caitlin,",
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me"
                     },

                new { Source = "toothbrushCircle",
                    Ind = _imageCount++,
                    Color = Color.FromHex("FFBBBB"),
                    Title = "Meeting with Not Impossible Labs",
                    Length = "Should take about 1 hour",
                    ButtonText = "Click for More About Me"},

                new { Source = CreateSource(),
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Title = "Make myself some lunch",
                    Length = "Takes from 10 to 30 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me"},

                new { Source = CreateSource(), Ind = _imageCount++, Color = Color.FromHex("BBD8FF"), Title = "Browse Pinterest projects", Length = "" },
                new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Default, Title = "Eat dinner with mom", Length = "Takes about 30 minutes" },
                new { Source = CreateSource(), Ind = _imageCount++, Color = Color.Default, Title = "Finish my chemistry homework", Length = "Could take up to 1 hour" }
            };*/

            AboutMeCommand = new Command(
            async () =>
            {
                await mainPage.Navigation.PushAsync(new GreetingPage());
            });
            Items.Add(new
            {
                Source = "Martha.png",
                Title = "About Me",
                Ind = _imageCount++,
                Color = Color.Default,
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = AboutMeCommand
            });

            if (App.user.routines.Count >= 1)

                Items.Add(new
            {
                Source = App.user.routines[0].photo,
                Title = App.user.routines[0].title,
                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(0));
                     })
            });

            if (App.user.routines.Count >= 2)
                Items.Add(new
            {
                Source = App.user.routines[1].photo,
                Title = App.user.routines[1].title,
                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(1));
                     })
            });

            if (App.user.routines.Count >= 3)
                Items.Add(new
            {
                Source = App.user.routines[2].photo,
                Title = App.user.routines[2].title,
                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(2));
                     })
            });

            if (App.user.routines.Count >= 4)
                Items.Add(new
            {
                Source = App.user.routines[3].photo,
                Title = App.user.routines[3].title,
                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(3));
                     })
            });

            if (App.user.routines.Count >= 5)
                Items.Add(new
            {
                Source = App.user.routines[4].photo,
                Title = App.user.routines[4].title,
                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(4));
                     })
            });

            if (App.user.routines.Count >= 6)
                Items.Add(new
            {
                Source = App.user.routines[5].photo,
                Title = App.user.routines[5].title,

                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(5));
                     })
            });

            if (App.user.routines.Count >= 7)
                Items.Add(new
            {
                Source = App.user.routines[6].photo,
                Title = App.user.routines[6].title,

                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(6));
                     })
            });

            if (App.user.routines.Count >= 8)
                Items.Add(new
            {
                Source = App.user.routines[7].photo,
                Title = App.user.routines[7].title,

                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(7));
                     })
            });

            if(App.user.routines.Count>=9)
            Items.Add(new
            {
                Source = App.user.routines[8].photo,
                Title = App.user.routines[8].title,

                Ind = _imageCount++,
                Color = Color.Default,
                Length = "Takes me 25 minutes",
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(8));
                     })
            });

            if (App.user.routines.Count >= 10)
                Items.Add(new
                {
                    Source = App.user.routines[9].photo,
                    Title = App.user.routines[9].title,

                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(9));
                         })
                });

            if (App.user.routines.Count >= 11)

                Items.Add(new
                {
                    Source = App.user.routines[10].photo,
                    Title = App.user.routines[10].title,
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(10));
                     })
                });

            if (App.user.routines.Count >= 12)
                Items.Add(new
                {
                    Source = App.user.routines[11].photo,
                    Title = App.user.routines[11].title,
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(11));
                     })
                });

            if (App.user.routines.Count >= 13)
                Items.Add(new
                {
                    Source = App.user.routines[12].photo,
                    Title = App.user.routines[12].title,
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(12));
                     })
                });

            if (App.user.routines.Count >= 14)
                Items.Add(new
                {
                    Source = App.user.routines[13].photo,
                    Title = App.user.routines[13].title,
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(13));
                     })
                });

            if (App.user.routines.Count >= 15)
                Items.Add(new
                {
                    Source = App.user.routines[14].photo,
                    Title = App.user.routines[14].title,
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(14));
                     })
                });

            if (App.user.routines.Count >= 16)
                Items.Add(new
                {
                    Source = App.user.routines[15].photo,
                    Title = App.user.routines[15].title,

                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(15));
                     })
                });

            if (App.user.routines.Count >= 17)
                Items.Add(new
                {
                    Source = App.user.routines[16].photo,
                    Title = App.user.routines[16].title,

                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(16));
                     })
                });

            if (App.user.routines.Count >= 18)
                Items.Add(new
                {
                    Source = App.user.routines[17].photo,
                    Title = App.user.routines[17].title,

                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(17));
                     })
                });

            if (App.user.routines.Count >= 19)
                Items.Add(new
                {
                    Source = App.user.routines[18].photo,
                    Title = App.user.routines[18].title,

                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(18));
                         })
                });

            if (App.user.routines.Count >= 20)
                Items.Add(new
                {
                    Source = App.user.routines[19].photo,
                    Title = App.user.routines[19].title,

                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(19));
                         })
                });
            /*int routineNum = 0;
            foreach (routine routine in App.user.routines)
            {
                Console.WriteLine("routine Num : " + routineNum);
                Items.Add(new
                {
                    Source = routine.photo,
                    Title = routine.title,
                    Ind = _imageCount++,
                    Color = Color.Default,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(routineNum));
                     })
                });
                routineNum++;
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


        public int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                _currentIndex = value;
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentIndex)));
            }
        }

        public bool IsAutoAnimationRunning { get; set; }

        public bool IsUserInteractionRunning { get; set; }

        public ObservableCollection<object> Items { get; }

        private string CreateSource()
        {
            var source = $"toothbrushCircle.png";
            return source;
        }
    }
}
