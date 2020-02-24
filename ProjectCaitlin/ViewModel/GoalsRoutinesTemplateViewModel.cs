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
                Text = "Click this card to start!",
                ButtonText = "Click for More About Me",
                Navigate = AboutMeCommand
            });

            if (App.user.routines.Count >= 1) {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[0].photo,
                    Title = App.user.routines[0].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(0, true));
                     })
                });
            }
            if (App.user.routines.Count >= 2) {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[1].photo,
                    Title = App.user.routines[1].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(1, true));
                     })
                });
            }
            if (App.user.routines.Count >= 3)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[2].photo,
                    Title = App.user.routines[2].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(2, true));
                     })
                });
            }
            if (App.user.routines.Count >= 4)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[3].photo,
                    Title = App.user.routines[3].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(3, true));
                     })
                });
            }
            if (App.user.routines.Count >= 5)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[4].photo,
                    Title = App.user.routines[4].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(4, true));
                     })
                });
            }
            if (App.user.routines.Count >= 6)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[5].photo,
                    Title = App.user.routines[5].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(5, true));
                     })
                });
            }
            if (App.user.routines.Count >= 7)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[6].photo,
                    Title = App.user.routines[6].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(6, true));
                     })
                });
            }
            if (App.user.routines.Count >= 8)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[7].photo,
                    Title = App.user.routines[7].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(7, true));
                     })
                }); 
            }

            if (App.user.routines.Count >= 9)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[8].photo,
                    Title = App.user.routines[8].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(8, true));
                     })
                });
            }

            if (App.user.routines.Count >= 10)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[9].photo,
                    Title = App.user.routines[9].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(9, true));
                         })
                });
            }

            if (App.user.routines.Count >= 11)
            {
                complete.Add(false);

                Items.Add(new
                {
                    Source = App.user.routines[10].photo,
                    Title = App.user.routines[10].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(10, true));
                     })
                });
            }
            if (App.user.routines.Count >= 12)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[11].photo,
                    Title = App.user.routines[11].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(11, true));
                     })
                });
            }
            if (App.user.routines.Count >= 13)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[12].photo,
                    Title = App.user.routines[12].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(12, true));
                     })
                });
            }
            if (App.user.routines.Count >= 14)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[13].photo,
                    Title = App.user.routines[13].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(13, true));
                     })
                });
            }
            if (App.user.routines.Count >= 15)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[14].photo,
                    Title = App.user.routines[14].title,
                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(14, true));
                     })
                });
            }
            if (App.user.routines.Count >= 16)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[15].photo,
                    Title = App.user.routines[15].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(15, true));
                     })
                });
            }
            if (App.user.routines.Count >= 17)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[16].photo,
                    Title = App.user.routines[16].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(16, true));
                     })
                });
            }
            if (App.user.routines.Count >= 18)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[17].photo,
                    Title = App.user.routines[17].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                     async () =>
                     {
                         await mainPage.Navigation.PushAsync(new TaskPage(17, true));
                     })
                });
            }
            if (App.user.routines.Count >= 19)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[18].photo,
                    Title = App.user.routines[18].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(18, true));
                         })
                });
            }
            if (App.user.routines.Count >= 20)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.routines[19].photo,
                    Title = App.user.routines[19].title,

                    Ind = _imageCount++,
                    BackgroundColor = Color.Default,
                    TextColor = Color.Black,
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(19, true));
                         })
                });
            }
            if (App.user.goals.Count >= 1)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[0].photo,
                    Title = App.user.goals[0].title,

                    Ind = _imageCount++,
                    TextColor = Color.White,
                    BackgroundColor = "#272E32",
                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(0, false));
                         })
                });
            }

            if (App.user.goals.Count >= 2)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[1].photo,
                    Title = App.user.goals[1].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(1, false));
                         })
                });
            }
            if (App.user.goals.Count >= 3)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[2].photo,
                    Title = App.user.goals[2].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(2, false));
                         })
                });
            }
            if (App.user.goals.Count >= 4)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[3].photo,
                    Title = App.user.goals[3].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(3, false));
                         })
                });
            }
            if (App.user.goals.Count >= 5)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[4].photo,
                    Title = App.user.goals[4].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(4, false));
                         })
                });
            }
            if (App.user.goals.Count >= 6)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[5].photo,
                    Title = App.user.goals[5].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(5, false));
                         })
                });
            }
            if (App.user.goals.Count >= 7)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[6].photo,
                    Title = App.user.goals[6].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(6, false));
                         })
                });
            }
            if (App.user.goals.Count >= 8)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[7].photo,
                    Title = App.user.goals[7].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(7, false));
                         })
                });
            }
            if (App.user.goals.Count >= 9)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[8].photo,
                    Title = App.user.goals[8].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(8, false));
                         })
                });
            }
            if (App.user.goals.Count >= 10)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[9].photo,
                    Title = App.user.goals[9].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(9, false));
                         })
                });
            }
            if (App.user.goals.Count >= 11)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[10].photo,
                    Title = App.user.goals[10].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(10, false));
                         })
                });
            }
            if (App.user.goals.Count >= 12)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[11].photo,
                    Title = App.user.goals[11].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(11, false));
                         })
                });
            }
            if (App.user.goals.Count >= 13)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[12].photo,
                    Title = App.user.goals[12].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(12, false));
                         })
                });
            }
            if (App.user.goals.Count >= 14)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[13].photo,
                    Title = App.user.goals[13].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(13, false));
                         })
                });
            }
            if (App.user.goals.Count >= 15)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[14].photo,
                    Title = App.user.goals[14].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(14, false));
                         })
                });
            }
            if (App.user.goals.Count >= 16)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[15].photo,
                    Title = App.user.goals[15].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(15, false));
                         })
                });
            }
            if (App.user.goals.Count >= 17)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[16].photo,
                    Title = App.user.goals[16].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(16, false));
                         })
                });
            }
            if (App.user.goals.Count >= 18)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[17].photo,
                    Title = App.user.goals[17].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(17, false));
                         })
                });
            }
            if (App.user.goals.Count >= 19)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[18].photo,
                    Title = App.user.goals[18].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(18, false));
                         })
                });
            }
            if (App.user.goals.Count >= 20)
            {
                complete.Add(false);
                Items.Add(new
                {
                    Source = App.user.goals[19].photo,
                    Title = App.user.goals[19].title,

                    Ind = _imageCount++,
                    BackgroundColor = "#272E32",
                    TextColor = Color.White,

                    Length = "Takes me 25 minutes",
                    Text = "Click this card to start!",
                    ButtonText = "Click for More About Me",
                    Navigate = new Command(
                         async () =>
                         {
                             await mainPage.Navigation.PushAsync(new TaskPage(19, false));
                         })
                });
            }

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
        public void setUpTime()
        {
            DateTime localDate = DateTime.Now;
            // Uses the default calendar of the InvariantCulture.
            Calendar myCal = CultureInfo.InvariantCulture.Calendar;
            DayLabel = myCal.GetDayOfWeek(localDate) + "";
            int time = localDate.Hour;

            if (time >= 6 && time < 12) { TimeLabel = "Good Morning"; DayImage = "sunrisemid.png"; }
            else if (time >= 12 && time < 18) { TimeLabel = "Good Afternoon"; DayImage = "fullsun.png"; }
            else { TimeLabel = "Good Evening"; DayImage = "moon.png"; }
        }
        public bool IsAutoAnimationRunning { get; set; }

        public bool IsUserInteractionRunning { get; set; }

        public ObservableCollection<object> Items { get; }

    }
}
