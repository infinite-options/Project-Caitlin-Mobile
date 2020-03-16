using System;

namespace ProjectCaitlin
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;

        void Initialize();

        void ReceiveNotification(string title, string message);
        int ScheduleNotification(string title, string message, double duration);
        void PrintPendingNotifications();
    }
}