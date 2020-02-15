using System;

namespace ProjectCaitlin.Authentication
{
    public interface IGoogleAuthenticationDelegate
    {
        void OnAuthenticationCompleted();
        void OnAuthenticationFailed(string message, Exception exception);
        void OnAuthenticationCanceled();
        //void OnGetCalendarsCompleted(GoogleOAuthToken token);
    }
}
