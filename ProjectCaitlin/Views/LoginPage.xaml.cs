﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectCaitlin.Authentication;
using ProjectCaitlin.Methods;
using ProjectCaitlin.Models;
using Xamarin.Auth;
using Xamarin.Forms;

namespace ProjectCaitlin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {

		Account account;
		public static string accessToken;


		public static FirestoreMethods FSMethods;
        public static string refreshToken = "1//06wtEbpEnf3VBCgYIARAAGAYSNwF-L9IrTcpRa4IsqetNoVK3RQsX_FJHiPXso5sDweGSLW-N_7oB78Nu68vqFcAhacV9ZcbUAKY";

        public LoginPage()
        {
			InitializeComponent();
			FSMethods = new FirestoreMethods("7R6hAVmDrNutRkG3sVRy");
			LoadFirebaseUser();
        }

        async Task LoadFirebaseUser()
        {
			await FSMethods.LoadUser();
			OnPropertyChanged(nameof(App.user));
			Console.WriteLine("user first name: " + App.user.firstName);
			Console.WriteLine("user last name: " + App.user.lastName);

            foreach (routine routine in App.user.routines)
            {
				OnPropertyChanged(nameof(routine));
				Console.WriteLine("user routine title: " + routine.title);
				Console.WriteLine("user routine id: " + routine.id);
			}

			foreach (goal goal in App.user.goals)
			{
				OnPropertyChanged(nameof(goal));
				Console.WriteLine("user goal title: " + goal.title);
				Console.WriteLine("user goal id: " + goal.id);
			}
		}

		async void LoginClicked(object sender, EventArgs e)
        {
			string clientId = null;
			string redirectUri = null;

			switch (Device.RuntimePlatform)
			{
				case Device.iOS:
					clientId = Constants.iOSClientId;
					redirectUri = Constants.iOSRedirectUrl;
					break;

				case Device.Android:
					clientId = Constants.AndroidClientId;
					redirectUri = Constants.AndroidRedirectUrl;
					break;
			}

			var authenticator = new OAuth2Authenticator(
				clientId,
				null,
				Constants.Scope,
				new Uri(Constants.AuthorizeUrl),
				new Uri(redirectUri),
				new Uri(Constants.AccessTokenUrl),
				null,
				true);

			authenticator.Completed += OnAuthCompleted;
			authenticator.Error += OnAuthError;

			AuthenticationState.Authenticator = authenticator;

			var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
			presenter.Login(authenticator);

		}

		async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;
			if (authenticator != null)
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}

			if (e.IsAuthenticated)
			{
				// If the user is authenticated, request their basic user data from Google
				// UserInfoUrl = https://www.googleapis.com/oauth2/v2/userinfo
				var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account);
				var response = await request.GetResponseAsync();
				if (response != null)
				{
					// Deserialize the data and store it in the account store
					// The users email address will be used to identify data in SimpleDB
					string userJson = await response.GetResponseTextAsync();
					//user = JsonConvert.DeserializeObject<user>(userJson);
				}

				if (account != null)
				{
					//store.Delete(account, Constants.AppName);
				}

				//await store.SaveAsync(account = e.Account, Constants.AppName);
				//await DisplayAlert("Login Successful", "", "OK");

                accessToken = e.Account.Properties["access_token"];
                //await LoginGoogleAsync();

				await Navigation.PushAsync(new DailyViewPage());

                //Display Successful Login Alert
				//await DisplayAlert("Login Successful", "", "OK");

                //Reset accessToken
                accessToken = e.Account.Properties["access_token"];

                //Write the Toekn to console, in case it changes
                Console.WriteLine("HERE is the key------------------------------------------------");
                Console.WriteLine(e.Account.Properties["access_token"]);
                //Console.WriteLine(e.Account.Properties["refresh_token"]);
                Console.WriteLine("----------------------------------------------------------------");


                //Navigate to the Daily Page after Login
                // await Navigation.PushAsync(new DailyViewPage());
			}
		}

		void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
		{
			var authenticator = sender as OAuth2Authenticator;
			if (authenticator != null)
			{
				authenticator.Completed -= OnAuthCompleted;
				authenticator.Error -= OnAuthError;
			}

			DisplayAlert("Authentication error: " , e.Message, "OK");
		}

        public void SkipLoginClicked(object sender, EventArgs e)
        {
            accessToken = "ya29.Il-8B9wQJLqM5-t04SGNlv4j-7RMP_VCZx-U4ZK0YKK125ydbJB0prX9hCSOOfmQ-9X8dUT-K40RJq_jA8lcr_T3L4SoN0JX4rhTDfQEe0i2s_JGCbEtbqFRjA9mCbN2-Q";
            Navigation.PushAsync(new DailyViewPage());
        }

		async Task LoginGoogleAsync()
		{
			//try
			//{
			//	if (!string.IsNullOrEmpty(_googleService.ActiveToken))
			//	{
			//		//Always require user authentication
			//		_googleService.Logout();
			//	}

			//	EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
			//	userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
			//	{
			//		switch (e.Status)
			//		{
			//			case GoogleActionStatus.Completed:
   //                         #if DEBUG
			//				var googleUserString = JsonConvert.SerializeObject(e.Data);
			//				Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
   //                         #endif
			//				//await App.Current.MainPage.Navigation.PushModalAsync(new HomePage(socialLoginData));
			//				break;
			//			case GoogleActionStatus.Canceled:
			//				await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
			//				break;
			//			case GoogleActionStatus.Error:
			//				await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
			//				break;
			//			case GoogleActionStatus.Unauthorized:
			//				await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
			//				break;
			//		}

			//		_googleService.OnLogin -= userLoginDelegate;
			//	};

			//	_googleService.OnLogin += userLoginDelegate;

			//	await _googleService.LoginAsync();
			//}
			//catch (Exception ex)
			//{
			//	Debug.WriteLine(ex.ToString());
			//}
		}
	}
}