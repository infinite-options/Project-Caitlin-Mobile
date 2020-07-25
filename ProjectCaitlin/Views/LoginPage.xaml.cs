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
		public static string access_token;
		FirestoreService firestoreService;
		FirebaseFunctionsService firebaseFunctionsService;
		public static string refreshToken;
        public string clientId;

        public LoginPage()
        {
			InitializeComponent();
        }

		void LoginClicked(object sender, EventArgs e)
        {
			//await Navigation.PushAsync(new GreetingPage());

			//testing for voicepage
			//await Navigation.PushAsync(new VoiceIdentificationPage());

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
				loginButton.Opacity = 0;
				loginButton.Clicked -= LoginClicked;
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
                else
                {
					loginButton.Opacity = 1;
					loginButton.Clicked += LoginClicked;
				}

				if (userJson != null)
				{
					Console.WriteLine("HERE is the TOKEN------------------------------------------------");
					Console.WriteLine(e.Account.Properties["access_token"]);
					Console.WriteLine("HERE is the REFRESH TOKEN----------------------------------------");
					Console.WriteLine(e.Account.Properties["refresh_token"]);
					Console.WriteLine("----------------------------------------------------------------");

					//Reset access_token
					access_token = e.Account.Properties["access_token"];
					refreshToken = e.Account.Properties["refresh_token"];

					App.User = new user();
					firebaseFunctionsService = new FirebaseFunctionsService();

					//Query for email in Users collection
					App.User.email = userJson["email"].ToString();
                    App.User.id = await firebaseFunctionsService.FindUserDocAsync(App.User.email);

					Console.WriteLine("First user id : " + App.User.id);

                    if (App.User.id == "")
                    {
						await DisplayAlert("Oops!", "Looks like your trusted advisor hasn't registered your account yet. Please ask for their assistance!", "OK");
						loginButton.Opacity = 1;
						loginButton.Clicked += LoginClicked;
						return;
                    }

					firestoreService = new FirestoreService();

					//Save to App.User AND Update Firebase with pertitnent info
					var googleService = new GoogleService();
					googleService.Navigation = Navigation;
					Console.WriteLine(refreshToken);

                    //Save Properies inside phone for auto login
					if (Application.Current.Properties.ContainsKey("access_token")) {
						Application.Current.Properties["access_token"] = access_token;
					} else {
						Application.Current.Properties.Add("access_token", access_token);
					}

					if (Application.Current.Properties.ContainsKey("refreshToken")) {
						Application.Current.Properties["refreshToken"] = refreshToken;
					} else {
						Application.Current.Properties.Add("refreshToken", refreshToken);
					}

					if (Application.Current.Properties.ContainsKey("user_id")){
						Application.Current.Properties["user_id"] = App.User.id;
					} else {
						Application.Current.Properties.Add("user_id", App.User.id);
					}

					await Application.Current.SavePropertiesAsync();

					App.LoadApplicationProperties();

					

					//Navigate to the Daily Page after Login
					await Navigation.PushAsync(new LoadingPage());
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
	}
}
