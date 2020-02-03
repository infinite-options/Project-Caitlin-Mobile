using System;
using System.Collections.Generic;
using ProjectCaitlin.Models;

using Xamarin.Forms;

namespace ProjectCaitlin
{
    public partial class DailyViewPage : ContentPage
    {
        user user;


        public DailyViewPage()
        {
            InitializeComponent();
            BindingContext = this;
            user = App.user;
            setupUI();
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

        async void PrintID(object sender, System.EventArgs e)
        {
            var task = sender as task;

            Console.WriteLine("button clicked, ID: " + task.id);
        }

        async void PhotosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PhotoDisplayPage());
        }

        public void setupUI()
        {
            Console.WriteLine("user.routines.Count: " + user.routines.Count);
            if (user.routines.Count > 0)
            {
                routineTitle.Text = user.routines[0].title;
            }

            foreach (routine routine in user.routines)
            {
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
                        HeightRequest = 90,
                        CornerRadius = 5,
                        Margin = new Thickness(5, 20, 5, 10),
                        BackgroundColor = Color.WhiteSmoke,
                    };

                    taskList.Children.Add(button);

                    Console.WriteLine("task name: " + task.title);
                    Console.WriteLine("task id: " + task.id);
                    Console.WriteLine("task id: " + task.photo);

                }
            }

        }
    }
}
