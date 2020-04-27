using System;
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
		FirebaseFunctionsService firebaseFunctionsService;
		public static string refreshToken;
        public string clientId;

        public LoginPage()
        {
			InitializeComponent();
        }

		protected override async void OnAppearing()
		{
			if (Application.Current.Properties.ContainsKey("accessToken")
                && Application.Current.Properties.ContainsKey("refreshToken")
				&& Application.Current.Properties.ContainsKey("user_id"))
            {
				LoadApplicationProperties();

				firestoreService = new FirestoreService();
				firebaseFunctionsService = new FirebaseFunctionsService();

				await firestoreService.LoadUser();
				await GoogleService.LoadTodaysEvents();

				await Navigation.PushAsync(new GoalsRoutinesTemplate());
			}
            else
            {
                if (!App.isAuthenticating)
                    loginButton.IsVisible = true;
				firebaseFunctionsService = new FirebaseFunctionsService();
			}
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
			App.isAuthenticating = true;
			loginButton.IsVisible = false;
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
				JObject userJson = null;
				if (response != null)
				{
					// Deserialize the data and store it in the account store
					// The users email address will be used to identify data in SimpleDB
					string userJsonString = await response.GetResponseTextAsync();
					userJson = JObject.Parse(userJsonString);
				}

				if (userJson != null)
				{
					//store.Delete(account, Constants.AppName);
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

					App.User = new user();

					//Query for email in Users collection
					App.User.email = userJson["email"].ToString();
                    App.User.id = firebaseFunctionsService.FindUserDoc(App.User.email).Result;

                    if (App.User.id == "")
                    {
						DisplayAlert("Oops!", "Looks like your trusted advisor hasn't registered your account yet. Please ask for their assistance!", "OK");
						loginButton.IsVisible = true;
						return;
                    }

					firestoreService = new FirestoreService();

					//Save to App.User AND Update Firebase with pertitnent info
					var googleService = new GoogleService();
					await googleService.SaveAccessTokenToFireBase(accessToken);
					Console.WriteLine(refreshToken);
					await googleService.SaveRefreshTokenToFireBase(refreshToken);

                    //Save Properies inside phone for auto login
					Application.Current.Properties["accessToken"] = accessToken;
					Application.Current.Properties["refreshToken"] = refreshToken;
					Application.Current.Properties["user_id"] = App.User.id;

					LoadApplicationProperties();

					await firestoreService.LoadUser();
					await GoogleService.LoadTodaysEvents();

					App.isAuthenticating = false;
					//Navigate to the Daily Page after Login
					await Navigation.PushAsync(new GoalsRoutinesTemplate());
				}
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

        private void LoadApplicationProperties()
        {
			App.User.access_token = Application.Current.Properties["accessToken"].ToString();
			App.User.refresh_token = Application.Current.Properties["refreshToken"].ToString();
			App.User.id = Application.Current.Properties["user_id"].ToString();
		}
	}
}
