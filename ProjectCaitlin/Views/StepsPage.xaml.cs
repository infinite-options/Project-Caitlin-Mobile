﻿using ProjectCaitlin.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using Xamarin.Forms.Xaml;
using ProjectCaitlin.Methods;

namespace ProjectCaitlin.Views
{
    public partial class StepsPage : ContentPage
    {
        private int itemcount;
        int a;
        public int b;
        bool isRoutine;
        List<bool> complete;
        readonly StepsPageViewModel pageModel;

        public StepsPage(int a, int b, bool isRoutine)
        {
            InitializeComponent();


            this.a = a;
            this.b = b;
            this.isRoutine = isRoutine;
            pageModel = new StepsPageViewModel(this, a, b, isRoutine);
            BindingContext = pageModel;
            itemcount = pageModel.count;
            StepListView.HeightRequest = 50 * App.User.routines[a].tasks[b].steps.Count;
        }

        public async void close(object sender, EventArgs args)
        {
            Navigation.PopAsync();
            Navigation.PopAsync();
        }

        public async void back(object sender, EventArgs args)
        {
            await Navigation.PopAsync();
        }

        public async void DoneClicked(object sender, EventArgs args)
        {
            var completeCounter = 0;

            var completeTasksCounter = 0;

            foreach (step step in App.User.routines[a].tasks[b].steps)
            {
                if (step.isComplete)
                {
                    completeCounter++;
                }
            }

            if (completeCounter == App.User.routines[a].tasks[b].steps.Count)
            {
                var routineId = App.User.routines[a].id;
                var taskId = App.User.routines[a].tasks[b].id;

                var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

                var okToCheckmark = await firestoreService.UpdateTask(routineId, taskId, App.User.routines[a].tasks[b].dbIdx.ToString());
                if (okToCheckmark)
                {
                    App.User.routines[a].tasks[b].isComplete = true;
                    App.User.routines[a].tasks[b].dateTimeCompleted = DateTime.Now;
                }

                await Navigation.PushAsync(new TaskPage(a, isRoutine));
            }
            else
            {
                await DisplayAlert("Oops!", "Please complete all steps before marking this task as done", "OK");
            }

            foreach (task task in App.User.routines[a].tasks)
            {
                if (task.isComplete)
                {
                    completeTasksCounter++;
                }
            }

            if (completeTasksCounter == App.User.routines[a].tasks.Count)
            {
                var routineId = App.User.routines[a].id;

                var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

                var okToCheckmark = await firestoreService.CompleteRoutine(routineId, App.User.routines[a].dbIdx.ToString());
                if (okToCheckmark)
                {
                    App.User.routines[a].isComplete = true;
                    App.User.routines[a].dateTimeCompleted = DateTime.Now;
                }
            }
        }
    }
}
