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


		FirestoreMethods FSMethods;

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

			foreach (routine goal in App.user.goals)
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
				await DisplayAlert("Login Successful", "", "OK");

                accessToken = e.Account.Properties["access_token"];
                //await LoginGoogleAsync();

				await Navigation.PushAsync(new DailyPage());
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

			Debug.WriteLine("Authentication error: " + e.Message);
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
