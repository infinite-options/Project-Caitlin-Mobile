using System;

namespace ProjectCaitlin
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        void ReceiveNotification(string title, string message, bool isValid);
        int ScheduleNotification(string title, string subtitle, string message, double duration, string notification_tag, int notification_id);
        void PrintPendingNotifications();
    }
}