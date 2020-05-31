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
            int routineIdx = int.Parse(notification.Request.Content.UserInfo["routineNum"].ToString());
            string routineId = notification.Request.Content.UserInfo["routineId"].ToString();

            bool isRoutineComplete = false;
            if (routineIdx < App.User.routines.Count && App.User.routines[routineIdx].id == routineId)
            {
                isRoutineComplete = App.User.routines[routineIdx].isComplete;
            }
            else
            {
                foreach (routine routine in App.User.routines)
                {
                    if (routine.id == routineId)
                    {
                        isRoutineComplete = App.User.routines[routineIdx].isComplete;
                    }
                }
            }

            DependencyService.Get<INotificationManager>().ReceiveNotification(notification.Request.Content.Title, notification.Request.Content.Body, !isRoutineComplete);

            // alerts are always shown for demonstration but this can be set to "None"
            // to avoid showing alerts if the app is in the foreground
            if (isRoutineComplete)
                completionHandler(UNNotificationPresentationOptions.None);
            else
            {
                if (App.IsInForeground)
                {
                    if (!await App.Current.MainPage.DisplayAlert(notification.Request.Content.Title, "Would you like to start it now?", "No", "Yes"))
                    {
                        App.ParentPage = "ListView";
                        App.User.routines[routineIdx].isInProgress = true;
                        App.User.routines[routineIdx].isComplete = false;
                        firebaseFunctionsService.updateGratisStatus(App.User.routines[routineIdx], "goals&routines", false);
                        if (App.User.routines[routineIdx].isSublistAvailable)
                        {
                            await App.Current.MainPage.Navigation.PushAsync(new TaskPage(routineIdx, false, null));
                        }
                        else
                        {
                            await App.Current.MainPage.Navigation.PushAsync(new ListViewPage());
                        }
                    }
                }
                else
                {
                    completionHandler(UNNotificationPresentationOptions.Alert);
                }
            }
        }
    }
}
