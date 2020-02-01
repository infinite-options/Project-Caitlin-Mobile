using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using ProjectCaitlin.Services;
using ProjectCaitlin.ViewModel;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace ProjectCaitlin
{
    public partial class DailyViewPage : ContentPage
    {
        public static List<string> eventNameList;
        public int oldDate;
        public DailyViewModel dailyViewModel;

        public DailyViewPage()
        {
            InitializeComponent();
            BindingContext = DailyViewModel.Instance;
            PrepareRefreshEvents();

            dailyViewModel = (DailyViewModel)BindingContext;           
        }

        public async void PrepareRefreshEvents()
        {
            await Task.Delay(1000);
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
                await DisplayAlert("Oops!", "There was an error listing your events", "Re-Login");
            }

            //Parse the json using EventsList Method

            try
            {

                var parsedResult = JsonConvert.DeserializeObject<Methods.GetEventsListMethod>(jsonResult);

                //Create Item List 
                var eventList = new List<string>();
                var dateList = new List<string>();
                var startTimeList = new List<string>();
                var endTimeList = new List<string>();


                //Separate out just the EventName 
                foreach (var events in parsedResult.Items)
                 {
                    eventList.Add(events.EventName);
                    dateList.Add(events.Start.DateTime.ToString());
                    startTimeList.Add(events.Start.DateTime.ToString());
                    endTimeList.Add(events.End.DateTime.ToString());
                }


                //Add EventName's in to a string separated by commas
                //eventNameString = String.Join(", ", itemList);
                eventNameList = eventList;

                //---------- Configure UI

                navDate.IsVisible = true;
                //navEvents.IsVisible = true;
                dividerLine.IsVisible = true;

                //---------- ROW 1
                try
                {
                    if (eventNameList[0] == null)
                    {

                    }
                    else
                    {
                        string dateString = dateList[0];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl1.Text = paddedNum;
                        }
                        else
                        {
                            numLbl1.Text = date[1];
                        }

                        oldDate = dateInt;

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0,4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl1.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl1.Text = dayOfWeek.ToUpper();
                        }

                        button1.Text = eventNameList[0];
                        button1.BackgroundColor = Color.FromHex("#56b880");
                        button1.TextColor = Color.White;
                        numLbl1.TextColor = Color.Default;
                        dayLbl1.TextColor = Color.Default;

                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    await DisplayAlert("Oops!", "No events during scheduled", "Go Back");
                    await Navigation.PopAsync();
                }

                //--------- ROW 2
                try
                {
                    if (eventNameList[1] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button2.IsVisible = true;
                        numLbl2.IsVisible = true;
                        dayLbl2.IsVisible = true;

                        //Update date label
                        string dateString = dateList[1];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl2.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl2.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl2.Text = paddedNum;
                        }
                        else
                        {
                            numLbl2.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl2.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl2.Text = dayOfWeek.ToUpper();
                        }

                        //Update Button Text
                        button2.Text = eventNameList[1];
                        button2.BackgroundColor = Color.FromHex("#56b880");
                        button2.TextColor = Color.White;
                    }
                }
                catch(ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 3
                try
                {
                    if (eventNameList[2] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button3.IsVisible = true;
                        numLbl3.IsVisible = true;
                        dayLbl3.IsVisible = true;

                        string dateString = dateList[2];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl3.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl3.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl3.Text = paddedNum;
                        }
                        else
                        {
                            numLbl3.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl3.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl3.Text = dayOfWeek.ToUpper();
                        }

                        button3.Text = eventNameList[2];
                        button3.BackgroundColor = Color.FromHex("#56b880");
                        button3.TextColor = Color.White;
                    }
                }
                catch(ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 4
                try
                {
                    if (eventNameList[3] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button4.IsVisible = true;
                        numLbl4.IsVisible = true;
                        dayLbl4.IsVisible = true;

                        string dateString = dateList[3];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl4.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl4.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl4.Text = paddedNum;
                        }
                        else
                        {
                            numLbl4.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl4.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl4.Text = dayOfWeek.ToUpper();
                        }

                        button4.Text = eventNameList[3];
                        button4.BackgroundColor = Color.FromHex("#56b880");
                        button4.TextColor = Color.White;
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 5
                try
                {
                    if (eventNameList[4] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button5.IsVisible = true;
                        numLbl5.IsVisible = true;
                        dayLbl5.IsVisible = true;

                        string dateString = dateList[4];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl5.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl5.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl5.Text = paddedNum;
                        }
                        else
                        {
                            numLbl5.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl5.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl5.Text = dayOfWeek.ToUpper();
                        }

                        button5.Text = eventNameList[4];
                        button5.BackgroundColor = Color.FromHex("#e6e6e6");
                        button5.TextColor = Color.White;
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 6
                try
                {
                    if (eventNameList[5] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button6.IsVisible = true;
                        numLbl6.IsVisible = true;
                        dayLbl6.IsVisible = true;

                        string dateString = dateList[5];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl6.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl6.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl6.Text = paddedNum;
                        }
                        else
                        {
                            numLbl6.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl6.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl6.Text = dayOfWeek.ToUpper();
                        }

                        button6.Text = eventNameList[5];
                        button6.BackgroundColor = Color.FromHex("a6567b");
                        button6.TextColor = Color.White;
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 7
                try
                {
                    if (eventNameList[6] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button7.IsVisible = true;
                        numLbl7.IsVisible = true;
                        dayLbl7.IsVisible = true;

                        string dateString = dateList[6];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl7.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl7.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl7.Text = paddedNum;
                        }
                        else
                        {
                            numLbl7.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl7.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl7.Text = dayOfWeek.ToUpper();
                        }

                        button7.Text = eventNameList[6];
                        button7.BackgroundColor = Color.FromHex("a6567b");
                        button7.TextColor = Color.White;
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 8
                try
                {
                    if (eventNameList[7] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button8.IsVisible = true;
                        numLbl8.IsVisible = true;
                        dayLbl8.IsVisible = true;

                        string dateString = dateList[7];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl8.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl8.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl8.Text = paddedNum;
                        }
                        else
                        {
                            numLbl8.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl8.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl8.Text = dayOfWeek.ToUpper();
                        }

                        button8.Text = eventNameList[7];
                        button8.BackgroundColor = Color.FromHex("a6567b");
                        button8.TextColor = Color.White;
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 9
                try
                {
                    if (eventNameList[8] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button9.IsVisible = true;
                        numLbl9.IsVisible = true;
                        dayLbl9.IsVisible = true;

                        string dateString = dateList[8];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl9.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl9.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl9.Text = paddedNum;
                        }
                        else
                        {
                            numLbl9.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl9.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl9.Text = dayOfWeek.ToUpper();
                        }

                        button9.Text = eventNameList[8];
                        button9.BackgroundColor = Color.FromHex("a6567b");
                        button9.TextColor = Color.White;
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }

                //----------- ROW 10
                try
                {
                    if (eventNameList[9] == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        //Enable Them First
                        button10.IsVisible = true;
                        numLbl10.IsVisible = true;
                        dayLbl10.IsVisible = true;

                        string dateString = dateList[9];
                        string[] date = dateString.Split('/');
                        int dateInt = Int32.Parse(date[1]);
                        string paddedNum = "";

                        if (oldDate == dateInt)
                        {
                            numLbl10.TextColor = Color.FromHex("#FAFAFA");
                            dayLbl10.TextColor = Color.FromHex("#FAFAFA");
                        }

                        oldDate = dateInt;

                        if (dateInt < 10)
                        {
                            paddedNum = dateInt.ToString().PadLeft(2, '0');
                            numLbl10.Text = paddedNum;
                        }
                        else
                        {
                            numLbl10.Text = date[1];
                        }

                        var month = Int32.Parse(date[0]);
                        var day = Int32.Parse(date[1]);
                        var year = Int32.Parse(date[2].Substring(0, 4));

                        DateTime dt = new DateTime(year, month, day);

                        var dayOfWeek = dt.DayOfWeek.ToString();

                        if (dayOfWeek == "Thursday")
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl10.Text = dayOfWeek.ToUpper();
                        }
                        else
                        {
                            dayOfWeek = dt.DayOfWeek.ToString().Substring(0, 3);
                            dayLbl10.Text = dayOfWeek.ToUpper();
                        }

                        button10.Text = eventNameList[9];
                        button10.BackgroundColor = Color.FromHex("a6567b");
                        button10.TextColor = Color.White;

                    }
                }
                catch (ArgumentOutOfRangeException e)
                {

                }

                //------- ANIMATION FADE-IN

                await Task.WhenAny
                    (
                       button1.FadeTo(1, 185),
                       numLbl1.FadeTo(1, 500),
                       dayLbl1.FadeTo(1, 700),
                       button1.TranslateTo(0, 0, 20)
                    );

                await Task.WhenAny
                    (
                       button2.FadeTo(1, 200),
                       numLbl2.FadeTo(1, 500),
                       dayLbl2.FadeTo(1, 700),
                       button2.TranslateTo(0, 0, 35)
                    );

                await Task.WhenAny
                    (
                       button3.FadeTo(1, 215),
                       numLbl3.FadeTo(1, 500),
                       dayLbl3.FadeTo(1, 700),
                       button3.TranslateTo(0, 0, 50)
                    );

                await Task.WhenAny
                    (
                       button4.FadeTo(1, 230),
                       numLbl4.FadeTo(1, 500),
                       dayLbl4.FadeTo(1, 700),
                       button4.TranslateTo(0, 0, 65)
                    );

                await Task.WhenAny
                    (
                       button5.FadeTo(1, 245),
                       numLbl5.FadeTo(1, 500),
                       dayLbl5.FadeTo(1, 700),
                       button5.TranslateTo(0, 0, 80)
                    );

                await Task.WhenAny
                    (
                       button6.FadeTo(1, 260),
                       numLbl6.FadeTo(1, 500),
                       dayLbl6.FadeTo(1, 700),
                       button6.TranslateTo(0, 0, 95)
                    );

                await Task.WhenAny
                    (
                       button7.FadeTo(1, 275),
                       numLbl7.FadeTo(1, 500),
                       dayLbl7.FadeTo(1, 700),
                       button7.TranslateTo(0, 0, 110)
                    );

                await Task.WhenAny
                    (
                       button8.FadeTo(1, 290),
                       numLbl8.FadeTo(1, 500),
                       dayLbl8.FadeTo(1, 700),
                       button8.TranslateTo(0, 0, 125)
                    );

                await Task.WhenAny
                    (
                       button9.FadeTo(1, 305),
                       numLbl9.FadeTo(1, 500),
                       dayLbl9.FadeTo(1, 700),
                       button9.TranslateTo(0, 0, 140)
                    );

                await Task.WhenAny
                    (
                       button10.FadeTo(1, 320),
                       numLbl10.FadeTo(1, 500),
                       dayLbl10.FadeTo(1, 700),
                       button10.TranslateTo(0, 0, 155)
                    );

                //dailyViewModel.UpdateBtn1.Execute(null);
                //dailyViewModel.UpdateBtn2.Execute(null);
                //dailyViewModel.UpdateColorNumLbl1Trans.Execute(null);
                //dailyViewModel.UpdateColorDayLbl1Trans.Execute(null);

                //if (eventNameList[0] != null)
                //{
                //   dailyViewModel.UpdateColorLbl1Trans.Execute(null);
                //}

                //Console.WriteLine(eventNameList[0]);

                return null;

            }

            catch(ArgumentNullException e)
            {
                //LoginPage.accessToken = LoginPage.refreshToken;
                await Navigation.PopAsync();
                //Console.WriteLine(LoginPage.accessToken);
                return null;
            }

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
