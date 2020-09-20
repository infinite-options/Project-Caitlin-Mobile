using System;
using UserNotifications;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using UIKit;
using Foundation;
using ProjectCaitlin.Views;
using ProjectCaitlin.Services;

namespace ProjectCaitlin.iOS
{
    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        public override async void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            try
            {
                //int routineIdx = int.Parse(notification.Request.Content.UserInfo["routineNum"].ToString());
                //string routineId = notification.Request.Content.UserInfo["routineId"].ToString();
                int grIdx = int.Parse(notification.Request.Content.UserInfo["grNum"].ToString());
                string grId = notification.Request.Content.UserInfo["grId"].ToString();
                string goalOrRoutine = notification.Request.Content.UserInfo["goalOrRoutine"].ToString();

                //bool isRoutineComplete = false;
                bool isGRComplete = false;

                if(goalOrRoutine.Equals("routine"))
                {
                    if (grIdx < App.User.routines.Count && App.User.routines[grIdx].id == grId)
                    {
                        isGRComplete = App.User.routines[grIdx].isComplete;
                    }
                    else
                    {
                        foreach (routine routine in App.User.routines)
                        {
                            if (routine.id == grId)
                            {
                                isGRComplete = App.User.routines[grIdx].isComplete;
                            }
                        }
                    }
                }

                else
                {
                    if (grIdx < App.User.goals.Count && App.User.goals[grIdx].id == grId)
                    {
                        isGRComplete = App.User.goals[grIdx].isComplete;
                    }
                    else
                    {
                        foreach (goal goal in App.User.goals)
                        {
                            if (goal.id == grId)
                            {
                                isGRComplete = App.User.goals[grIdx].isComplete;
                            }
                        }
                    }
                }


                DependencyService.Get<INotificationManager>().ReceiveNotification(notification.Request.Content.Title, notification.Request.Content.Body, !isGRComplete);

                // alerts are always shown for demonstration but this can be set to "None"
                // to avoid showing alerts if the app is in the foreground

                if (isGRComplete)
                    completionHandler(UNNotificationPresentationOptions.None);
                else
                {
                    if (App.IsInForeground)
                    {
                        if (!await App.Current.MainPage.DisplayAlert(notification.Request.Content.Title, "Would you like to start it now?", "No", "Yes"))
                        {
                            App.ParentPage = "ListView";
                            if(goalOrRoutine.Equals("routine"))
                            {
                                App.User.routines[grIdx].isInProgress = true;
                                App.User.routines[grIdx].isComplete = false;
                                firebaseFunctionsService.updateGratisStatus(App.User.routines[grIdx], "goals&routines", false);
                                if (App.User.routines[grIdx].isSublistAvailable)
                                {
                                    await App.Current.MainPage.Navigation.PushAsync(new TaskPage(grIdx, true, null));
                                }
                                else
                                {
                                    await App.Current.MainPage.Navigation.PushAsync(new TodaysList());
                                }

                            }
                            else
                            {
                                App.User.goals[grIdx].isInProgress = true;
                                App.User.goals[grIdx].isComplete = false;
                                firebaseFunctionsService.updateGratisStatus(App.User.goals[grIdx], "goals&routines", false);
                                if (App.User.goals[grIdx].isSublistAvailable)
                                {
                                    await App.Current.MainPage.Navigation.PushAsync(new TaskPage(grIdx, false, null));
                                }
                                else
                                {
                                    await App.Current.MainPage.Navigation.PushAsync(new TodaysList());
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        completionHandler(UNNotificationPresentationOptions.Alert);
                    }
                }


            }
            catch (Exception e)
            {
                Console.Error.WriteLine($"Notification Error: {e}");
            }
        }
    }
}
