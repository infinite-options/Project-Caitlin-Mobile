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
using ProjectCaitlin.Services;
using ProjectCaitlin.Views;
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
		FirestoreService firestoreService;
        public static string refreshToken;
        public string clientId;

        public LoginPage()
        {
			InitializeComponent();
        }

		protected override async void OnAppearing()
		{
			loginButton.IsVisible = true;
			//var firestoreService = new FirestoreService("7R6hAVmDrNutRkG3sVRy");
			//await firestoreService.LoadUser();

			//Console.WriteLine("----------------------------------------------------------");
			//Console.WriteLine("Made it to 1");
			//Console.WriteLine(App.User.access_token);
			//Console.WriteLine(App.User.refresh_token);
			//Console.WriteLine("----------------------------------------------------------");

			//if (App.User.old_refresh_token != App.User.refresh_token)
			//{
			//	Console.WriteLine("----------------------------------------------------------");
			//	Console.WriteLine("Made it to 2: First catch");
			//	Console.WriteLine("----------------------------------------------------------");

			//	if (App.User.access_token != null)
			//	{
			//		Console.WriteLine("----------------------------------------------------------");
			//		Console.WriteLine("Made it to 2: Second catch");
			//		Console.WriteLine("----------------------------------------------------------");
			//		if (await GoogleService.LoadTodaysEvents())
			//		{
			//			await Navigation.PushAsync(new GoalsRoutinesTemplate());
			//		}
			//		else
			//		{
			//			loginButton.IsVisible = true;
			//		}
			//	}
			//}
			//else
			//{
			//	// await Application.Current.MainPage.DisplayAlert("Alert", "Please re-login to continue", "OK");
			//
			//}
		}

		async void LoginClicked(object sender, EventArgs e)
        {
			clientId = null;
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



		async void CardViewPageClicked(object sender, EventArgs e)
		{

			await Navigation.PushAsync(new GoalsRoutinesTemplate());

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
				var request = new OAuth2Request("GET", new Uri(Constants.UserInfoUrl), null, e.Account.Properties["access_token"]);
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

                //Display Successful Login Alert
				//await DisplayAlert("Login Successful", "", "OK");

                //Write the Toekn to console, in case it changes
                Console.WriteLine("HERE is the TOKEN------------------------------------------------");
                Console.WriteLine(e.Account.Properties["access_token"]);
                Console.WriteLine("HERE is the REFRESH TOKEN----------------------------------------");
                Console.WriteLine(e.Account.Properties["refresh_token"]);
                Console.WriteLine("----------------------------------------------------------------");

                //Reset accessToken
                accessToken = e.Account.Properties["access_token"];
                refreshToken = e.Account.Properties["refresh_token"];

                //Save to App.User AND Update Firebase with pertitnent info
                var googleService = new GoogleService();
                await googleService.SaveAccessTokenToFireBase(accessToken);
                Console.WriteLine(refreshToken);
                await googleService.SaveRefreshTokenToFireBase(refreshToken);

                //Navigate to the Daily Page after Login
                await Navigation.PushAsync(new GoalsRoutinesTemplate());
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

		public async void ListViewClicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new ListViewPage());
		}
	}
}
