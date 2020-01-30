using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectCaitlin.Services;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace ProjectCaitlin
{
    public partial class DailyViewPage : ContentPage
    {
        public DailyViewPage()
        {
            InitializeComponent();

            PrepareRefreshEvents();
            
        }

        public async void PrepareRefreshEvents()
        {
            await Task.Delay(3000);
            await RefreshEvents();
        }

        public async Task<string> RefreshEvents()
        {

            //Call Google API
            var googleService = new GoogleService();
            var jsonResult = await googleService.GetEventsList();

            //Return error if result is empty
            if (jsonResult == null)
            {
                await DisplayAlert("Oops!", "There was an error listing your events", "OK");
            }

            //Parse the json using EventsList Method
            var parsedResult = JsonConvert.DeserializeObject<Methods.GetEventsListMethod>(jsonResult);

            //Create Item List 
            var itemList = new List<string>();

            //Separate out just the EventName 
            try
            {
                foreach (var events in parsedResult.Items)
                {
                    itemList.Add(events.EventName);
                    //itemList.Add(events.Start.DateTime.ToString());
                    //itemList.Add(events.End.DateTime.ToString());
                }
            }

            //Catch if there is an error
            catch (NullReferenceException e)
            {
                return (null);
            }

            //Add EventName's in to a string separated by commas
            string eventNameString = String.Join(", ", itemList);

            Console.WriteLine(eventNameString);

            var buttonDisplay = itemList[0];

            return null;
        }

        //Re-Login as a new user and return to Login Screen
        public void ReLoginClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        //Disable Android's back button
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
