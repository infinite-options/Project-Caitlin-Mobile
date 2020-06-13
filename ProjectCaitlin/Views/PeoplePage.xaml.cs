using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using VoiceRecognition.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VoiceRecognition.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PeoplePage : ContentPage
    {
        public ObservableCollection<PeopleDto> Contacts { get; set; }

        public PeoplePage()
        {
            InitializeComponent();

            Contacts = new ObservableCollection<PeopleDto>
            {
                new PeopleDto(new People{
                        FirstName = "Papa",
                        LastName = "Brown",
                        Relation = "Father",
                        PhoneNumber = "1234567890",
                    }){ 
                    AzureId = "1",
                    EnrollmentStatus="Enrolled"
                },
                new PeopleDto(new People
                    {
                        FirstName = "Mama",
                        LastName = "Brown",
                        Relation = "Mother",
                        PhoneNumber = "0987654321",
                        
                    }){
                        AzureId = "2",
                    EnrollmentStatus = "Enrolling"
                 },
                new PeopleDto(    new People
                    {
                        FirstName = "Bro",
                        LastName = "Brown",
                        Relation = "Brother",
                        PhoneNumber = "0192837465"
                    }){
                    AzureId = "3",
                    EnrollmentStatus = "NotEnrolled"
                },
                new PeopleDto(new People
                    {
                        FirstName = "Sis",
                        LastName = "Brown",
                        Relation = "Sister",
                        PhoneNumber = "1234567890"
                })
                {
                    AzureId = "4",
                    EnrollmentStatus = "Enrolled"
                }
            };

            Contacts_ListView.ItemsSource = Contacts;
            //Label heading = new Label
            //{
            //    Text = "RelativeLayout Example",
            //    TextColor = Color.Red,
            //};
            //PeopleBlocks.Children.Add(heading, 
            //    Constraint.RelativeToParent((parent) => {
            //    return parent.Width / 3;
            //}),
            //Constraint.RelativeToParent((parent) => {
            //    return parent.Height / 2;
            //}));
        }

        //private void peopleBlock()
        //{

        //}

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
