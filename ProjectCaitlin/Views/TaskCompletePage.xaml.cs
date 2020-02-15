using FlowListViewSample;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjectCaitlin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskCompletePage : ContentPage
    {
        private int itemcount;

        readonly TaskCompletePageViewModel pageModel;
        public TaskCompletePage()
        {
            InitializeComponent();
            pageModel = new TaskCompletePageViewModel(this);
            BindingContext = pageModel;
            itemcount = pageModel.count;


        }
        public async void nextpage(object sender, EventArgs args)
        {
            if (CarouselTasks.Position + 1 != itemcount)
            {
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
    }
}