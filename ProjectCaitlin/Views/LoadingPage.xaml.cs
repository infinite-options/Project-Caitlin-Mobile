using System;
using System.Collections.Generic;
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
using Xamarin.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProjectCaitlin
{
    public partial class LoadingPage : ContentPage
    {
        private FirestoreService firestoreService;
        private FirebaseFunctionsService firebaseFunctionsService;
        private GoogleService googleService;

        public LoadingPage()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            if (Application.Current.Properties.ContainsKey("access_token")
                && Application.Current.Properties.ContainsKey("refreshToken")
                && Application.Current.Properties.ContainsKey("user_id"))
            {
                App.LoadApplicationProperties();

                firestoreService = new FirestoreService();
                firebaseFunctionsService = new FirebaseFunctionsService();
                googleService = new GoogleService();

                bool loading = false;

                if (!await googleService.UseAccessToken())
                {
                    if (await googleService.RefreshToken())
                    {
                        loading = true;
                        Console.WriteLine("Access Token Expired / Using Refresh Token");
                        
                    }
                }

                else
                {
                    loading = true;
                    Console.WriteLine("Access token valid");
                }

                if(loading)
                {
                    Console.WriteLine("Calling LoadDatabase");
                    await firestoreService.LoadDatabase();
                    await googleService.LoadTodaysEvents();
                    await Navigation.PushAsync(new ListViewPage());
                }
                
            }
        }

        
    }
}
