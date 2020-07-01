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
using Android.Support.V4.App;
using Android.Graphics;
using AndroidApp = Android.App.Application;

namespace ProjectCaitlin.Droid
{
    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
            Console.WriteLine("***************INSIDE RECEIVER:******************************************");
            string notification_tag = intent.GetStringExtra("NotificationTag");
            int messageId = intent.GetIntExtra("MessageId", 0);
            string title = intent.GetStringExtra("title");
            string message = intent.GetStringExtra("message");
            string channelId = intent.GetStringExtra("default");

            Console.WriteLine("MESSAGE:" + message);
            //Console.WriteLine("DURATION: ");
            Console.WriteLine("NOT_ID:" + notification_tag + messageId.ToString());


            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, 100, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamagonBlue))
                .SetSmallIcon(Resource.Drawable.xamagonBlue)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetAutoCancel(true);

            Notification notification = builder.Build();

            //NotificationManager mgr = (NotificationManager)context.GetSystemService(Context.NotificationService);
            var mgr = NotificationManagerCompat.From(AndroidApp.Context);

            mgr.Notify(notification_tag, messageId, notification);

        }
    }
}