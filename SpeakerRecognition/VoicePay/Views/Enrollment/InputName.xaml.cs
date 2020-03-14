using System;
using System.Collections.Generic;
using VoicePay.ViewModels;
using System.Net.Http;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace VoicePay.Views.Enrollment
{
    public partial class InputName : ContentPage
    {
        protected readonly HttpClient _httpClient;

        //public Entry fullNameField = new Entry() { Placeholder = "Full name", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };

        public InputName()
        {
            InitializeComponent();
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //var MyEntry = new Entry { Text = "Enter name" };
            Entry entry = fullname;
            var text = entry.Text;
            System.Diagnostics.Debug.WriteLine(entry.Text);

            var client = new HttpClient();
            var requestUri = "https://tlusrtjjq5.execute-api.us-west-1.amazonaws.com/dev/api/v1/add";
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.Headers.Accept.ParseAdd("application/json");

            var body = new Dictionary<string, string>{
                { "profileID", Helpers.Settings.UserIdentificationId },
                { "profileName", entry.Text },
            };

            var json = JsonConvert.SerializeObject(body, Formatting.Indented);
            var bodycontent = new StringContent(json);
            

            request.Content = bodycontent;
            HttpResponseMessage response = await client.SendAsync(request);
            var jsonResponse = await response.Content.ReadAsStringAsync();


            System.Diagnostics.Debug.WriteLine(body);
            System.Diagnostics.Debug.WriteLine(bodycontent);
            System.Diagnostics.Debug.WriteLine(jsonResponse);



            Application.Current.MainPage = new Views.Enrollment.MainPage();
            //await BaseViewModel.MasterDetail.Detail.Navigation.PopToRootAsync();
            //await BaseViewModel.MasterDetail.Detail.Navigation.PopModalAsync();
        }





    }

}

