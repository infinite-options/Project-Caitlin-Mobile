using System;
using System.Collections.Generic;
using ProjectCaitlin.Methods;
using ProjectCaitlin.Models;

using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class DailyViewPage : ContentPage
    {
        user user;
        FirestoreMethods FSMethods;


        public DailyViewPage()
        {
            InitializeComponent();
            BindingContext = this;
            FSMethods = LoginPage.FSMethods;
            user = App.user;
            SetupUI();
        }

        public void SetupUI()
        {
            Console.WriteLine("user.routines.Count: " + user.routines.Count);

            List<View> timedTaskListElements = new List<View>();
            List<View> routineTaskListElements = new List<View>();
            List<View> goalGridElements = new List<View>();

            foreach (View element in timedTaskList.Children)
            {
                timedTaskListElements.Add(element);
            }
            foreach (View element in routineTaskList.Children)
            {
                routineTaskListElements.Add(element);
            }
            foreach (View element in goalGridElements)
            {
                goalGridElements.Add(element);
            }

            foreach (View element in timedTaskListElements)
            {
                timedTaskList.Children.Remove(element);
            }
            foreach (View element in routineTaskListElements)
            {
                routineTaskList.Children.Remove(element);
            }
            foreach (View element in goalGridElements)
            {
                GoalsGrid.Children.Remove(element);
            }


            foreach (routine routine in user.routines)
            {
                if (routine.isPersistent && routine.title == "Get Ready")
                {
                    routineTitle.Text = routine.title;
                    foreach (task task in routine.tasks)
                    {

                        Button button = new Button
                        {
                            Text = task.title,
                            TextColor = Color.Black,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.Center,
                            ImageSource = task.photo,
                            WidthRequest = 80,
                            HeightRequest = 80,
                            CornerRadius = 5,
                            Margin = new Thickness(5, 20, 5, 10),
                            BackgroundColor = Color.WhiteSmoke,
                        };

                        routineTaskList.Children.Add(button);
                    }
                }

                if (routine.title == "Make Breakfast")
                {
                    timedTitle.Text = routine.title;
                    foreach (task task in routine.tasks)
                    {
                        Button button = new Button
                        {
                            Text = task.title,
                            TextColor = Color.Black,
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            HorizontalOptions = LayoutOptions.Center,
                            ImageSource = task.photo,
                            WidthRequest = 60,
                            HeightRequest = 60,
                            CornerRadius = 5,
                            Margin = new Thickness(3, 20, 3, 10),
                            BackgroundColor = Color.WhiteSmoke,
                        };

                        timedTaskList.Children.Add(button);
                    }
                }
            }

            var goalIdx = 0;
            foreach (goal goal in user.goals)
            {
                StackLayout stack = new StackLayout
                {
                    HeightRequest = 200,
                };

                Label label = new Label
                {
                    Text = goal.title,
                    HorizontalOptions = LayoutOptions.Center
                };
                Image image = new Image
                {
                    Source = goal.photo,

                };
                stack.Children.Add(label);
                stack.Children.Add(image);
                switch (goalIdx)
                {
                    case 0:
                        GoalsGrid.Children.Add(stack, 0, 0);
                        break;
                    case 1:
                        GoalsGrid.Children.Add(stack, 1, 0);
                        break;
                    case 2:
                        GoalsGrid.Children.Add(stack, 0, 1);
                        break;
                    case 3:
                        GoalsGrid.Children.Add(stack, 1, 1);
                        break;
                }

                goalIdx++;
            }
        }

        public async void MonthlyBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonthlyViewPage());
        }

        public async void ListBtnClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ListViewPage());
        }

        public void ReLoginClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        void PrintID(object sender, System.EventArgs e)
        {
            var task = sender as task;

            Console.WriteLine("button clicked, ID: " + task.id);
        }

        public async void RefreshDatabase(object sender, EventArgs e)
        {
            await FSMethods.LoadUser();
            user = App.user;
            SetupUI();
        }

        async void PhotosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhotoDisplayPage());
        }
    }
}
