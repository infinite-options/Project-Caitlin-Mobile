﻿using System;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;


[assembly: Dependency(typeof(ProjectCaitlin.Droid.AndroidNotificationManager))]
namespace ProjectCaitlin.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

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

        public int ScheduleNotification(string title, string subtitle, string message, double duration,string notification_tag, int notification_id)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            messageId = notification_id;

            Intent intent = new Intent(AndroidApp.Context, typeof(AlarmReceiver));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.PutExtra("NotificationTag", notification_tag);
            intent.PutExtra("MessageId", notification_id);
            intent.PutExtra(channelId, channelId);


            //PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

            /*
            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamagonBlue))
                .SetSmallIcon(Resource.Drawable.xamagonBlue)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            */

            //AlarmManager alarmMgr = (AlarmManager)AndroidApp.Context.GetSystemService(Context.AlarmService);
            var alarmManager = (AlarmManager)AndroidApp.Context.GetSystemService(Context.AlarmService);
            //AlarmManager mgr = (AlarmManager)AndroidApp.Context.GetSystemService(new Java.Lang.Class(AndroidApp.Context.));
            var interval = AlarmManager.IntervalDay;
            //manager.Notify(notification_tag, messageId, notification);
            Console.WriteLine("INSIDE SCHEDULE NOTIFICATION");
            Console.WriteLine("MESSAGE:" + message + ", DURATION: ");
            Console.WriteLine(duration);
            Console.WriteLine("NOT_ID:" + notification_tag + notification_id.ToString());
            alarmManager.SetRepeating(AlarmType.RtcWakeup, (long)duration, interval, pendingIntent);
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
    
    [BroadcastReceiver (Enabled =true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        
        public override void OnReceive(Context context, Intent intent)
        {
            //Console.WriteLine("ScheduledAlarmHandler", "Starting service @" + SystemClock.ElapsedRealtime());

            // Your App code here - start an Intentservice if needed;

            Console.WriteLine("***************INSIDE RECEIVER:******************************************");
            string notification_tag = intent.GetStringExtra("NotificationTag");
            int messageId = intent.GetIntExtra("MessageId", 0);
            string title = intent.GetStringExtra("title");
            string message = intent.GetStringExtra("message");
            string channelId = intent.GetStringExtra("default");

            Console.WriteLine("MESSAGE:" + message);
            //Console.WriteLine("DURATION: ");
            Console.WriteLine("NOT_ID:" + notification_tag + messageId.ToString());


            PendingIntent pendingIntent = PendingIntent.GetActivity(context, 100, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamagonBlue))
                .SetSmallIcon(Resource.Drawable.xamagonBlue)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetAutoCancel(true);

            Notification notification = builder.Build();

            NotificationManager mgr = (NotificationManager)context.GetSystemService(Context.NotificationService);

            mgr.Notify(notification_tag, messageId, notification);
        }


    }
}