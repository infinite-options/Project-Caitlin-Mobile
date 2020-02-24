﻿using ProjectCaitlin;
using ProjectCaitlin.Views;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using System;
using System.ComponentModel;
using ProjectCaitlin.Methods;

namespace ProjectCaitlin.ViewModel
{
    public class StepsPageViewModel : BindableObject
    {
        private StepsPage mainPage;
        public int count = 0;
        public string Text { get; set; }
        public Command ItemTappedCommand { get; set; }
        public string photo { get; set; }
        public string TopImage { get; set; }
        public string TopLabel { get; set; }
        public string TaskName { get; set; }

        private ObservableCollection<object> _items = new ObservableCollection<object>() { };

        public ObservableCollection<object> Items
        {
            get
            {
                return _items;
            }
            set
            {
                if (_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        public StepsPageViewModel(StepsPage mainPage, int a, int b, bool isRoutine)
        {
            this.mainPage = mainPage;
            var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");

            if (isRoutine)
            {
                TopImage = App.user.routines[a].tasks[b].photo;
                TopLabel = App.user.routines[a].tasks[b].title;
                TaskName = App.user.routines[a].title;
                int stepIdx = 0;
                int stepNum = 1;

                foreach (step step in App.user.routines[a].tasks[b].steps)
                {
                    var routineId = App.user.routines[a].id;
                    var taskId = App.user.routines[a].tasks[b].id;

                    if (App.user.routines[a].tasks[b].steps[stepIdx].isComplete == false)
                    {
                        _items.Add(new
                        { Text = stepNum + ". " + App.user.routines[a].tasks[b].steps[stepIdx].title,
                            CheckmarkIcon = "graycheckmarkicon.png",
                            CompleteStep = new Command(
                             async () =>
                             { var okToCheckmark = await firestoreService.UpdateStep(routineId, taskId, (stepIdx.ToString()));
                                 if (okToCheckmark)
                                 {
                                     _items[stepIdx] = "greencheckmarkicon.png";
                                 }
                        )});
                    }
                    else
                    {
                        _items.Add(new { Text = stepNum + ". " + App.user.routines[a].tasks[b].steps[stepIdx].title, CheckmarkIcon = "greencheckmarkicon.png" });
                    }

                    stepIdx++;
                    stepNum++;
                }

                //if (App.user.routines[a].tasks[b].steps.Count >= 1)
                //    _items.Add(new { Text = "1. " + App.user.routines[a].tasks[b].steps[0].title, CheckmarkIcon = "greencheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 2)
                //    _items.Add(new { Text = "2. " + App.user.routines[a].tasks[b].steps[1].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 3)
                //    _items.Add(new { Text = "3. " + App.user.routines[a].tasks[b].steps[2].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 4)
                //    _items.Add(new { Text = "4. " + App.user.routines[a].tasks[b].steps[3].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 5)
                //    _items.Add(new { Text = "5. " + App.user.routines[a].tasks[b].steps[4].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 6)
                //    _items.Add(new { Text = "6. " + App.user.routines[a].tasks[b].steps[5].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 7)
                //    _items.Add(new { Text = "7. " + App.user.routines[a].tasks[b].steps[6].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 8)
                //    _items.Add(new { Text = "8. " + App.user.routines[a].tasks[b].steps[7].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 9)
                //    _items.Add(new { Text = "9. " + App.user.routines[a].tasks[b].steps[8].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 10)
                //    _items.Add(new { Text = "10. " + App.user.routines[a].tasks[b].steps[9].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 11)
                //    _items.Add(new { Text = "11. " + App.user.routines[a].tasks[b].steps[10].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 12)
                //    _items.Add(new { Text = "12. " + App.user.routines[a].tasks[b].steps[11].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 13)
                //    _items.Add(new { Text = "13. " + App.user.routines[a].tasks[b].steps[12].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 14)
                //    _items.Add(new { Text = "14. " + App.user.routines[a].tasks[b].steps[13].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 15)
                //    _items.Add(new { Text = "15. " + App.user.routines[a].tasks[b].steps[14].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 16)
                //    _items.Add(new { Text = "16. " + App.user.routines[a].tasks[b].steps[15].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 17)
                //    _items.Add(new { Text = "17. " + App.user.routines[a].tasks[b].steps[16].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 18)
                //    _items.Add(new { Text = "18. " + App.user.routines[a].tasks[b].steps[17].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 19)
                //    _items.Add(new { Text = "19. " + App.user.routines[a].tasks[b].steps[18].title, CheckmarkIcon = "graycheckmarkicon.png" });

                //if (App.user.routines[a].tasks[b].steps.Count >= 20)
                //    _items.Add(new { Text = "20. " + App.user.routines[a].tasks[b].steps[19].title, CheckmarkIcon = "graycheckmarkicon.png" });
            }
        }

    }
    
}
