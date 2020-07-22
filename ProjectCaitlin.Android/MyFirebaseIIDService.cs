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
using Firebase.Iid;
using ProjectCaitlin.Services;

namespace ProjectCaitlin.Droid
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT"})]
    class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        FirebaseFunctionsService firebaseFunctionsService = new FirebaseFunctionsService();

        //[Obsolete]
        public override void OnTokenRefresh()
        {
            var refreshedToken = FirebaseInstanceId.Instance.Token;

            //SendTokenToServer(refreshedToken);
            App.deviceToken = refreshedToken;
            Console.WriteLine(App.User.email + "....: " + refreshedToken);
            //base.OnTokenRefresh();
        }

        private async void SendTokenToServer(string token)
        {
            
            
            Console.WriteLine("In SendTokenToServer: " + token);

            if(App.User.id != "")
                firebaseFunctionsService.sendDeviceToken(App.User.id, token);
            
        }
    }
}