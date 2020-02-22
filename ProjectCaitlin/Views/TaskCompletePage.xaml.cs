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

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskCompletePage : ContentPage
    {
        private int itemcount;
        int a;
        int b;
        bool isRoutine;
        List<bool> complete;
        readonly TaskCompletePageViewModel pageModel;
        public TaskCompletePage(int a,int b,bool isRoutine, List<bool> complete)
        {
            InitializeComponent();
            this.complete = complete;
            

            this.a = a;
            this.b = b;
            this.isRoutine = isRoutine;
            pageModel = new TaskCompletePageViewModel(this, a, b,isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;


        }
        public async void nextpage(object sender, EventArgs args)
        {
            if (CarouselTasks.Position == App.user.routines[a].tasks[b].steps.Count - 1)
            {
                next.Text = "Done";
            }

            else if (CarouselTasks.Position + 1 != itemcount)
            {
                next.Text = "Next";
                CarouselTasks.Position = CarouselTasks.Position + 1;
            }

            if (next.Text == "Done")
            {
                complete[b] = true;
                await Navigation.PushAsync(new TaskPage(a, isRoutine, complete));
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
            await Navigation.PushAsync(new TaskPage(a,isRoutine, complete));
            //await Navigation.PushAsync(new GoalsRoutinesTemplate());
        }
    }
}