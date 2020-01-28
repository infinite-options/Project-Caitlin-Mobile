using System;
using System.Collections.Generic;
using Xamarin.Auth;
using ProjectCaitlin.Authentication;

namespace ProjectCaitlin.Authentication
{
    public class GoogleAuthenticator
    {
        //private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        private const string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        //private const string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        private const string AccessTokenUrl = "https://oauth2.googleapis.com/token";
        private const bool IsUsingNativeUI = true;

        public static string superToken;

        private OAuth2Authenticator _auth;
        private IGoogleAuthenticationDelegate _authenticationDelegate;

        public GoogleAuthenticator(string clientId, string scope, string redirectUrl, IGoogleAuthenticationDelegate authenticationDelegate)
        {
            _authenticationDelegate = authenticationDelegate;

            _auth = new OAuth2Authenticator(clientId, string.Empty, scope,
                                            new Uri(AuthorizeUrl),
                                            new Uri(redirectUrl),
                                            new Uri(AccessTokenUrl),
                                            null, IsUsingNativeUI);

            _auth.Completed += OnAuthenticationCompleted;
            _auth.Error += OnAuthenticationFailed;
        }

        public OAuth2Authenticator GetAuthenticator()
        {
            return _auth;
        }

        public void OnPageLoading(Uri uri)
        {
            _auth.OnPageLoading(uri);
        }

        public void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            if (e.IsAuthenticated)
            {
                var token = new GoogleOAuthToken
                {
                    TokenType = e.Account.Properties["token_type"],
                    AccessToken = e.Account.Properties["access_token"]
                };
                _authenticationDelegate.OnAuthenticationCompleted();

                superToken = token.AccessToken;

                //System.Diagnostics.Debug.WriteLine(token);
                //System.Diagnostics.Debug.WriteLine(superToken);
                //System.Diagnostics.Debug.WriteLine(token.TokenType);
                //System.Diagnostics.Debug.WriteLine(token.AccessToken);
               

            }
            else
            {
                _authenticationDelegate.OnAuthenticationCanceled();
            }
        }

        private void OnAuthenticationFailed(object sender, AuthenticatorErrorEventArgs e)
        {
            _authenticationDelegate.OnAuthenticationFailed(e.Message, e.Exception);
        }
    }
}
