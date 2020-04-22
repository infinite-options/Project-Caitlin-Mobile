using System;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProjectCaitlin.iOS.IOSNotificationManager))]
namespace ProjectCaitlin.iOS
{
    public class IOSNotificationManager : INotificationManager
    {
        int messageId = -1;

        bool hasNotificationsPermission;

        UNNotificationRequest[] pendingNotificationRequests;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            // request the permission to use local notifications
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                hasNotificationsPermission = approved;
            });

            UNUserNotificationCenter.Current.GetPendingNotificationRequests(completionHandler: (obj) => {
                pendingNotificationRequests = obj;
            });
        }

        public void PrintPendingNotifications()
        {
            Console.WriteLine("");
            Console.WriteLine("=============GetPendingNotifications============= ");
            foreach (var notification in pendingNotificationRequests)
            {
                Console.WriteLine("Title: " + notification.Content.Title);
                Console.WriteLine("badge: " + notification.Content.Badge);
            }
            Console.WriteLine("=============GetPendingNotifications=============");
            Console.WriteLine("");

        }

        public void ReceiveNotification(string title, string message, bool isValid)
        {
            if (isValid)
            {
                var args = new NotificationEventArgs()
                {
                    Title = title,
                    Message = message
                };
                NotificationReceived?.Invoke(null, args);
            }
        }

        public int ScheduleNotification(string title, string message, double duration)
        {
            // EARLY OUT: app doesn't have permissions
            if (!hasNotificationsPermission)
            {
                return -1;
            }

            messageId++;

            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                Badge = 1
            };

            Console.WriteLine("here");

            // Local notifications can be time or location based
            // Create a time-based trigger, interval is in seconds and must be greater than 0
            UNTimeIntervalNotificationTrigger trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(duration, false);

            UNNotificationRequest request = UNNotificationRequest.FromIdentifier(messageId.ToString(), content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    throw new Exception($"Failed to schedule notification: {err}");
                }
                else
                {
                    Console.WriteLine("notification: " + request.ToString() + " made to notify in " + duration.ToString() + " seconds.");
                }
            });

            return messageId;
        }
    }
}