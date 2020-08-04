using System;
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
using Xamarin.Forms.Internals;
using Application = Xamarin.Forms.Application;


namespace ProjectCaitlin.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT"})]
    
    class MyFirebaseMessagingService : FirebaseMessagingService
    {
        //FirestoreService firestoreService = new FirestoreService();
        int not_tag = 1;

        public MyFirebaseMessagingService()
        {
            
        }

        /*
         * OnMessageReceived receives the remote message from FCM
         
         */
        public override async void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            Bundle savedInstanceState = new Bundle();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            if (message.Data.Count > 0)
            {
                Console.WriteLine("Data Payload found");
                /*Application a = Xamarin.Forms.Application.Current;
                Console.WriteLine("application initialized");
                
                IDictionary<string, object> properties = a.Properties;
                
                Console.WriteLine("Properties present: " + properties.Count);
                Console.WriteLine("Id present in phone: " + properties["user_id"].ToString());*/
                //string user_id = Xamarin.Forms.Application.Current.Properties["user_id"].ToString();
                //if (user_id == message.Data["id"])
                //{
                    if(App.User.id == "" || App.User.id == null || App.User.id == message.Data["id"])
                    {
                        App.User.id = message.Data["id"];
                        FirestoreService firestoreService = new FirestoreService();
                        await firestoreService.LoadDatabase();
                        //SendLocalNotification(message);
                    }
                    
                //}

                /*if (App.User.id != "" && App.User.id == message.Data["id"])
                {
                    //firestoreService.LoadDatabase();
                    //new AndroidNotificationManager().ScheduleNotification(message.Data["title"], "1234thisisatestnotificationcheck", message.Data["body"], 2.0, "1", 0, "routine");
                    //sendLocalNotification(message.Data["title"], "1234thisisatestnotificationcheck", message.Data["body"], 2.0, "1", 0, "routine");
                    //SendLocalNotification("We are getting notification");
                    await firestoreService.LoadDatabase();
                }*/
                
            }

            
            //firestoreService.LoadDatabase();
            //new AndroidNotificationManager().ScheduleNotification(message.GetNotification().Title, "1234thisisatestnotificationcheck", message.GetNotification().Body, 2.0, "1", 0, "routine");
        }

        void SendLocalNotification(RemoteMessage message)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            //intent.PutExtra("message", body);

            //string title = message.Data["title"] + " in local code";
            //string content = message.Data["content"] + " in local code";
            string title = "Manifest";
            string content = "Your trusted advisor updated your Goals and Routines!";
            //Unique request code to avoid PendingIntent collision.
            var requestCode = new Random().Next();
            var pendingIntent = PendingIntent.GetActivity(this, requestCode, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(this, MainActivity.CHANNEL_ID)
                .SetContentTitle(title)
                .SetSmallIcon(Resource.Drawable.ic_launcher)
                .SetContentText(content)
                .SetAutoCancel(true)
                .SetShowWhen(false)
                .SetContentIntent(pendingIntent);

            /*if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                notificationBuilder.SetChannelId(MainActivity.CHANNEL_ID);
            }*/

            var notificationManager = NotificationManager.FromContext(this);
            //notificationManager.Notify(0, notificationBuilder.Build());
            //notificationManager.Notify((++not_tag).ToString(), 0, notificationBuilder.Build());
            notificationManager.Notify(0, notificationBuilder.Build());
        }
    }
}