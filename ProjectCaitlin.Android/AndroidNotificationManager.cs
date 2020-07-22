using System;
using System.Globalization;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Graphics;
using Android.Icu.Util;
using Android.OS;
using Android.Support.V4.App;
using Google.Type;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;
using ProjectCaitlin.Models;
using ProjectCaitlin.Views;
using ProjectCaitlin.Services;
//using Android.


[assembly: Dependency(typeof(ProjectCaitlin.Droid.AndroidNotificationManager))]
namespace ProjectCaitlin.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        public int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        public int messageId = -1;
        string notification_tag;
        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            CreateNotificationChannel();
        }

        public int ScheduleNotification(string title, string subtitle, string message, double duration, string notification_tag, int notification_id, String gOrR)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            messageId = notification_id;

            Intent intent = new Intent(AndroidApp.Context, typeof(AlarmReceiver));
            //intent.PutExtra(TitleKey, title);
            //intent.PutExtra(MessageKey, message);
            //intent.PutExtra("NotificationTag", notification_tag);
            //intent.PutExtra("MessageId", notification_id);
            //intent.PutExtra(channelId, channelId);
            intent.PutExtra("grNum", Int32.Parse(subtitle.Substring(0, 1)));
            intent.PutExtra("grId", subtitle.Substring(1, 20));
            intent.PutExtra("goalOrRoutine", gOrR);

            //string notification_tag = intent.GetStringExtra("NotificationTag");
            int message_Id = notification_id;
            //string title = intent.GetStringExtra("title");
            //string message = intent.GetStringExtra("message");
            //string channelId = intent.GetStringExtra("default");

            int grIdx = intent.GetIntExtra("grNum", 0);
            string grId = intent.GetStringExtra("grId");                                                    //notification.Request.Content.UserInfo["grId"].ToString();
            string goalOrRoutine = intent.GetStringExtra("goalOrRoutine");                                 //notification.Request.Content.UserInfo["goalOrRoutine"].ToString();

            Console.WriteLine("MESSAGE:" + message);
            //Console.WriteLine("DURATION: ");
            Console.WriteLine("NOT_ID:" + notification_tag + messageId.ToString());

            Intent mainIntent = new Intent(AndroidApp.Context, typeof(MainActivity));
            mainIntent.PutExtra(TitleKey, title);
            mainIntent.PutExtra(MessageKey, message);
            //mainIntent.PutExtra("NotificationTag", notification_tag);
            //mainIntent.PutExtra("MessageId", notification_id);
            //mainIntent.PutExtra(channelId, channelId);
            mainIntent.AddFlags(ActivityFlags.ClearTop);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, mainIntent, PendingIntentFlags.OneShot);

            

            DependencyService.Get<INotificationManager>().ReceiveNotification(title, message, true);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_launcher))
                .SetSmallIcon(Resource.Drawable.xamagonBlue)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetAutoCancel(true);

            Android.App.Notification notification = builder.Build();

            //NotificationManager mgr = (NotificationManager)context.GetSystemService(Context.NotificationService);
            //var mgr = NotificationManagerCompat.From(AndroidApp.Context);
            //manager = (NotificationManager)Android.Content.Context.GetSystemService(AndroidApp.NotificationService);

            manager.Notify(notification_tag, message_Id, notification);
            //alarmManager.SetRepeating(AlarmType.RtcWakeup, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + dur, (long)interval, pendingIntent);
            return messageId;
        }

        public void ReceiveNotification(string title, string message, bool isValid)
        {
            if (isValid)
            {
                var args = new NotificationEventArgs()
                {
                    Title = title,
                    Message = message,
                };
                NotificationReceived?.Invoke(null, args);
            }
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        public void PrintPendingNotifications()
        {
            Console.WriteLine("Need to implement: AndroidNotificationManager.cs -> PrintPendingNotifications()");
        }
    }
    
    
}