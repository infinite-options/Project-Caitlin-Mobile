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
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            if(message.Data.Count > 0)
            {
                Console.WriteLine("Data Payload found");
                if (App.User.id != "" && App.User.id == message.Data["id"])
                {
                    //firestoreService.LoadDatabase();
                    new AndroidNotificationManager().ScheduleNotification(message.Data["title"], "1234thisisatestnotificationcheck", message.Data["body"], 2.0, "1", 0, "routine");

                }
            }

            
            //firestoreService.LoadDatabase();
            //new AndroidNotificationManager().ScheduleNotification(message.GetNotification().Title, "1234thisisatestnotificationcheck", message.GetNotification().Body, 2.0, "1", 0, "routine");
        }
    }
}