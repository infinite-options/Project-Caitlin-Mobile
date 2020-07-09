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
using ProjectCaitlin.Models;
using ProjectCaitlin.Views;
using ProjectCaitlin.Services;
//using UserNotifications;
using Xamarin.Forms;

using AndroidApp = Android.App.Application;

namespace ProjectCaitlin.Droid
{

    [BroadcastReceiver(Enabled = true)]
    public class AlarmReceiver : BroadcastReceiver
    {
        const string channelId = "default";
        public static string title = "";
        public static string message = "";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        int messageId = -1;
        NotificationManager manager;
        FirebaseFunctionsService firebaseFunctionsService;
        public override async void OnReceive(Context context, Intent intent)
        {
            //Toast.MakeText(context, "Received intent!", ToastLength.Short).Show();
            Console.WriteLine("***************INSIDE RECEIVER:******************************************");
            string notification_tag = intent.GetStringExtra("NotificationTag");
            int messageId = intent.GetIntExtra("MessageId", 0);
            string title = intent.GetStringExtra("title");
            string message = intent.GetStringExtra("message");
            string channelId = intent.GetStringExtra("default");

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


            bool isGRComplete = false;

            if (goalOrRoutine.Equals("routine"))
            {
                if (grIdx < App.User.routines.Count && App.User.routines[grIdx].id == grId)
                {
                    isGRComplete = App.User.routines[grIdx].isComplete;
                }
                else
                {
                    foreach (routine routine in App.User.routines)
                    {
                        if (routine.id == grId)
                        {
                            isGRComplete = App.User.routines[grIdx].isComplete;
                        }
                    }
                }
            }

            else
            {
                if (grIdx < App.User.goals.Count && App.User.goals[grIdx].id == grId)
                {
                    isGRComplete = App.User.goals[grIdx].isComplete;
                }
                else
                {
                    foreach (goal goal in App.User.goals)
                    {
                        if (goal.id == grId)
                        {
                            isGRComplete = App.User.goals[grIdx].isComplete;
                        }
                    }
                }
            }

            DependencyService.Get<INotificationManager>().ReceiveNotification(title, message, !isGRComplete);

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
            manager = (NotificationManager)context.GetSystemService(AndroidApp.NotificationService);

            if (!isGRComplete)
            {
                if (App.IsInForeground)
                {
                    bool answer = await App.Current.MainPage.DisplayAlert(title, "Would you like to start it now?", "No", "Yes");
                    if (!answer)
                    {
                        App.ParentPage = "ListView";
                        if (goalOrRoutine.Equals("routine"))
                        {
                            App.User.routines[grIdx].isInProgress = true;
                            App.User.routines[grIdx].isComplete = false;
                            firebaseFunctionsService.updateGratisStatus(App.User.routines[grIdx], "goals&routines", false);
                            if (App.User.routines[grIdx].isSublistAvailable)
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new TaskPage(grIdx, true, null));
                            }
                            else
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new ListViewPage());
                            }

                        }
                        else
                        {
                            App.User.goals[grIdx].isInProgress = true;
                            App.User.goals[grIdx].isComplete = false;
                            firebaseFunctionsService.updateGratisStatus(App.User.goals[grIdx], "goals&routines", false);
                            if (App.User.goals[grIdx].isSublistAvailable)
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new TaskPage(grIdx, false, null));
                            }
                            else
                            {
                                await App.Current.MainPage.Navigation.PushAsync(new ListViewPage());
                            }
                        }

                    }
                }
                else
                    manager.Notify(notification_tag, messageId, notification);
            }


        }
    }
}