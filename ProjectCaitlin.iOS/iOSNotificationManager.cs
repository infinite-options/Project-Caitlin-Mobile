using System;
using Foundation;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(ProjectCaitlin.iOS.IOSNotificationManager))]
namespace ProjectCaitlin.iOS
{
    public class IOSNotificationManager : INotificationManager
    {
        String messageId;

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

        public int ScheduleNotification(string title, string subtitle, string message, double duration, string notification_tag, int notification_id)
        {
            // EARLY OUT: app doesn't have permissions
            if (!hasNotificationsPermission)
            {
                return -1;
            }

            messageId = notification_tag + notification_id.ToString();

            //
            // Using C# objects, strings and ints, produces
            // a dictionary with 2 NSString keys, "key1" and "key2"
            // and two NSNumbers with the values 1 and 2
            //
            var key1 = new NSString("routineNum");
            var value1 = new NSNumber(Int32.Parse(subtitle.Substring(0, 1)));
            var key2 = new NSString("routineId");
            var value2 = new NSString(subtitle.Substring(1, 20));

            var userInfo = new NSDictionary(key1, value1, key2, value2);

            var content = new UNMutableNotificationContent()
            {
                Title = title,
                Subtitle = "",
                Body = message,
                UserInfo = userInfo,
                Badge = 1
            };

            Console.WriteLine("here");

            // Local notifications can be time or location based
            // Create a time-based trigger, interval is in seconds and must be greater than 0
            UNTimeIntervalNotificationTrigger trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(duration, false);
            //UNCalendarNotificationTrigger trigger1 = UNCalendarNotificationTrigger.CreateTrigger(NSD)

            UNNotificationRequest request = UNNotificationRequest.FromIdentifier(messageId, content, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                    Console.WriteLine($"Failed to schedule notification: {err}");
                }
                else
                {
                    Console.WriteLine($"notification: {request} made to notify in {duration} seconds.");
                }
            });

            return notification_id;
        }
    }
}