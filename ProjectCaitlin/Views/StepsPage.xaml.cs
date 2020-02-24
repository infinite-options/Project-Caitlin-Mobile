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



        }

        private void OnLabelClicked()
        {
            throw new NotImplementedException();
        }

        public async void close(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new TaskPage(a, isRoutine));
        }

        public async void DoneClicked(object sender, EventArgs args)
        {
            var completeCounter = 0;

            foreach(step step in App.user.routines[a].tasks[b].steps)
            {
                if (step.isComplete)
                {
                    completeCounter++;
                }
            }

            if(completeCounter == App.user.routines[a].tasks[b].steps.Count)
            {
                var routineId = App.user.routines[a].id;
                var taskId = App.user.routines[a].tasks[b].id;

                var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

                var okToCheckmark = await firestoreService.UpdateTask(routineId, taskId, b.ToString());
                if (okToCheckmark) { App.user.routines[a].tasks[b].isComplete = true; }

                await Navigation.PushAsync(new TaskPage(a, isRoutine));
            }
            else
            {
                await DisplayAlert("Oops!", "Please complete all steps before marking this task as done", "OK");
            }


        }
    }
}
