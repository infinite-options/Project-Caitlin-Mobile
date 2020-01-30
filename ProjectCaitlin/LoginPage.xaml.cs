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

        public static string accessToken = "ya29.Il-7B8r3xI0Tq6RAMn4qh5pNha875-JgmKEda7rEGYWDaRtNCrNXet7JwnwdQT_F1Sc1w5cPsYYyz5hTDnT4mt3c6seae3uDP8mnkytSLY7O6y5V0YGGkNU6EEtj9DF5Gg";
        public static string refreshToken = "1//06wtEbpEnf3VBCgYIARAAGAYSNwF-L9IrTcpRa4IsqetNoVK3RQsX_FJHiPXso5sDweGSLW-N_7oB78Nu68vqFcAhacV9ZcbUAKY";

        public LoginPage()
        {
            InitializeComponent();
        }

        public void LoginClicked(object sender, EventArgs e)
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
                if (account != null)
				{
					//store.Delete(account, Constants.AppName);
				}

				//await store.SaveAsync(account = e.Account, Constants.AppName);

                //Display Successful Login Alert
				await DisplayAlert("Login Successful", "", "OK");

                //Reset accessToken
                accessToken = e.Account.Properties["access_token"];

                //Write the Toekn to console, in case it changes
                Console.WriteLine("HERE is the key");
                Console.WriteLine(e.Account.Properties["access_token"]);
                Console.WriteLine(e.Account.Properties["refresh_token"]);
                Console.WriteLine("----------------");


                //Navigate to the Daily Page after Login
                await Navigation.PushAsync(new DailyViewPage());
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
            Navigation.PushAsync(new DailyViewPage());
        }
    }
}
