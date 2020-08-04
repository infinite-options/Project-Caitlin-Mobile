﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using ProjectCaitlin.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Android.Support.V4.App;

namespace ProjectCaitlin.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT"})]
    class MyFirebaseMessagingService : FirebaseMessagingService
    {
        FirestoreService firestoreService = new FirestoreService();
        public MyFirebaseMessagingService()
        {

        }

        /*
         * OnMessageReceived receives the remote message from FCM
         
         */
        public override async void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            if(message.Data.Count > 0)
            {
                Console.WriteLine("Data Payload found");
                if (App.User.id != "" && App.User.id == message.Data["id"])
                {
                    //firestoreService.LoadDatabase();
                    //new AndroidNotificationManager().ScheduleNotification(message.Data["title"], "1234thisisatestnotificationcheck", message.Data["body"], 2.0, "1", 0, "routine");
                    //sendLocalNotification(message.Data["title"], "1234thisisatestnotificationcheck", message.Data["body"], 2.0, "1", 0, "routine");
                    //SendLocalNotification("We are getting notification");
                    await firestoreService.LoadDatabase();
                }
            }

            
            //firestoreService.LoadDatabase();
            //new AndroidNotificationManager().ScheduleNotification(message.GetNotification().Title, "1234thisisatestnotificationcheck", message.GetNotification().Body, 2.0, "1", 0, "routine");
        }

        /*void SendLocalNotification(string body)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.PutExtra("message", body);

            //Unique request code to avoid PendingIntent collision.
            var requestCode = new Random().Next();
            var pendingIntent = PendingIntent.GetActivity(this, requestCode, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this)
                .SetContentTitle("Manifest")
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentText(body)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId("my_notification_channel");
            }

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.Notify(0, notificationBuilder.Build());
        }*/
    }
}