﻿using ProjectCaitlin.ViewModel;
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
        public TaskCompletePage(int a,int b,bool isRoutine)
        {
            InitializeComponent();
            pageModel = new TaskCompletePageViewModel(this, a, b,isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;


        }
        public async void nextpage(object sender, EventArgs args)
        {
           
           if (CarouselTasks.Position + 1 != itemcount)
            {   
                next.Text = "Next";
                previous.Text = "Previous";
                CarouselTasks.Position = CarouselTasks.Position + 1;
            }
            else
                next.Text = "Done";

        }
        public async void prepage(object sender, EventArgs args)
        {
            if (CarouselTasks.Position != 0)
            {
                previous.Text = "Previous";
                CarouselTasks.Position = CarouselTasks.Position - 1;
            }
        }
    }
}