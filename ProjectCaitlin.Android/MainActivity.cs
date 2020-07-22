using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Platform;
using PanCardView.Droid;
using Acr.UserDialogs;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android;
using Firebase;
using Android.Content;
using Xamarin.Forms;
using Firebase.Messaging;
using Firebase.Iid;
using Android.Util;
using Android.Gms.Common;

namespace ProjectCaitlin.Droid
{
    [Activity(Label = "ManifestMy Space", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public const string TAG = "MainActivity";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            /*var options = new FirebaseOptions.Builder()
              .SetApplicationId("<AppID>")
              .SetApiKey("<ApiKey>")
              .SetDatabaseUrl("<DBURl>")
              .SetStorageBucket("<StorageBucket>")
              .SetGcmSenderId("<SenderID>").Build();
            var fapp = FirebaseApp.InitializeApp(this, options);*/

            //FirebaseApp.InitializeApp(this);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            UserDialogs.Init(this);

            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.RecordAudio }, 1);
            }

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            CardsViewRenderer.Preserve();

            CachedImageRenderer.Init(true);
            CardsViewRenderer.Preserve();

            CachedImageRenderer.InitImageViewHandler();

            LoadApplication(new App());
            
            
            CreateNotificationFromIntent(base.Intent);

            //Console.WriteLine("The Device Token in OnCreate: " + FirebaseInstanceId.Instance.Token);
            if (FirebaseInstanceId.Instance.Token != null)
                App.deviceToken = FirebaseInstanceId.Instance.Token;

            IsPlayServicesAvailable();
        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        private void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.Extras.GetString(AndroidNotificationManager.TitleKey);
                string message = intent.Extras.GetString(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message, true);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    //Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                    Console.WriteLine("ERROR:" + TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Console.WriteLine(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            Console.WriteLine(TAG, "Google Play Services is available.");
            return true;
        }
    }
}
