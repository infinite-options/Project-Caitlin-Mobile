using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using ProjectCaitlin.Methods;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskCompletePage : ContentPage
    {
        private int itemcount;
        int a;
        int b;
        bool isRoutine;
        readonly TaskCompletePageViewModel pageModel;
        public TaskCompletePage(int a,int b,bool isRoutine)
        {
            InitializeComponent();

            this.a = a;
            this.b = b;
            this.isRoutine = isRoutine;
            pageModel = new TaskCompletePageViewModel(this, a, b,isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;


        }
        public async void nextpage(object sender, EventArgs args)
        {
            if (next.Text == "Done")
            {
                await Navigation.PushAsync(new TaskPage(a, isRoutine));
            }

            if (CarouselTasks.Position == App.user.goals[a].actions[b].instructions.Count - 1)
            {
                next.Text = "Done";
            }

            else if (CarouselTasks.Position + 1 != itemcount)
            {
                next.Text = "Next";
                CarouselTasks.Position = CarouselTasks.Position + 1;
            }

        }
        public async void prepage(object sender, EventArgs args)
        {
            if (CarouselTasks.Position != 0)
            {
                CarouselTasks.Position = CarouselTasks.Position - 1;
            }
        }
        public async void close(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new GoalsRoutinesTemplate());
        }
        public async void back(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new TaskPage(a, isRoutine));
        }
    }
}
