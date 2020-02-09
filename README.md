# Project-Caitlin-Mobile

Xamarin.Forms cross-platform application for Project Caitlin.

Before running the application, open
ProjectCaitlin -> Constants.cs
to make sure each of the lines contain the correct ClientID's.

If these variables are empty, please copy and paste the ClientID's listed in the
project-caitlin Slack channel before building and running.

If you desire to test new functionality, please start your own branch and include your name within the title.
Once tested for stability, new functionality can be merged with the master branch.

KNOWN ISSUES:
1. Android Authentication does not close automatically after login, click the "X" in the upper left after logging in.

CURRENT TO-DO's:
1. Add additional scopes for Google Photo API
2. Add page navigation.
3. Encorporate "Daily View," accessing & displaying recipes from Firebase.
4. Encorporate "List View", displaying events from Calendar API
5. Encorporate "Monthly View," displaying image thumbnails from Firebase.
