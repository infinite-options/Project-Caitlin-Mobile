using System;
using UserNotifications;
using Xamarin.Forms;
using ProjectCaitlin.Models;
using UIKit;
using Foundation;

namespace ProjectCaitlin.iOS
{
    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
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
                completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}