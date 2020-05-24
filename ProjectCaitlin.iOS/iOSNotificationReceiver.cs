using System;
using UserNotifications;
using Xamarin.Forms;
using ProjectCaitlin.Models;

namespace ProjectCaitlin.iOS
{
    public class iOSNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
        {
            int routineIdx = int.Parse(notification.Request.Content.Title.Substring(0,1));
            string title = notification.Request.Content.Title.Substring(1);
            bool isRoutineComplete = App.User.routines[routineIdx].isComplete;

            DependencyService.Get<INotificationManager>().ReceiveNotification(title, notification.Request.Content.Body, isRoutineComplete);

            // alerts are always shown for demonstration but this can be set to "None"
            // to avoid showing alerts if the app is in the foreground
            if (isRoutineComplete)
                completionHandler(UNNotificationPresentationOptions.None);
            else
                completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}