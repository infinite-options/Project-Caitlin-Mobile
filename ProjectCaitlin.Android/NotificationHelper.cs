using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
//using NotificationSample.Droid;
using Xamarin.Forms;
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

[assembly: Dependency(typeof(ProjectCaitlin.Droid.NotificationHelper))]
namespace ProjectCaitlin.Droid
{
    class NotificationHelper
    {
        /*const string channelId = "default";
        public static string title = "";
        public static string message = "";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;
        private Context mContext;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        int messageId = -1;
        NotificationManager manager;
        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        public NotificationHelper()
        {
            mContext = AndroidApp.Context;
        }

        public void Initialize()
        {
            CreateNotificationChannel();
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
        public void CreateNotification(String title, String message)
        {
            try
            {
                var intent = new Intent(mContext, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                intent.PutExtra(title, message);
                var pendingIntent = PendingIntent.GetActivity(mContext, 0, intent, PendingIntentFlags.OneShot);

                //DependencyService.Get<INotificationManager>().ReceiveNotification(title, message, !isGRComplete);

                NotificationCompat.Builder builder = new NotificationCompat.Builder(mContext, channelId)
                    .SetContentIntent(pendingIntent)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ic_launcher))
                    .SetSmallIcon(Resource.Drawable.xamagonBlue)
                    .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                    .SetAutoCancel(true);

                Android.App.Notification notification = builder.Build();



                NotificationManager notificationManager = mContext.GetSystemService(Context.NotificationService) as NotificationManager;

                if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.O)
                {
                    NotificationImportance importance = global::Android.App.NotificationImportance.High;

                    NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, title, importance);
                    notificationChannel.EnableLights(true);
                    notificationChannel.EnableVibration(true);
                    notificationChannel.SetSound(sound, alarmAttributes);
                    notificationChannel.SetShowBadge(true);
                    notificationChannel.Importance = NotificationImportance.High;
                    notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                    if (notificationManager != null)
                    {
                        mBuilder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                        notificationManager.CreateNotificationChannel(notificationChannel);
                    }
                }

                notificationManager.Notify(0, mBuilder.Build());
            }
            catch (Exception ex)
            {
                //
            }
        }*/
    }
}