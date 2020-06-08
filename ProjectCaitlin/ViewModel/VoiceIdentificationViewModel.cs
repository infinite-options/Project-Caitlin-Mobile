using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using ProjectCaitlin.Services;
using VoiceRecognition.Model;
using VoiceRecognition.Services.Firebase;

namespace ProjectCaitlin.ViewModel
{

    class VoiceIdentificationViewModel : BindableObject
    {
        public ObservableCollection<object> Items { get; set; }
        List<People> list;
        public VoiceIdentificationViewModel(People people)
        {
            Items = new ObservableCollection<object>();
            List<People> list = new List<People>();

            String pic;
            if (people.HavePic)
                pic = people.picUrl;
            else
                pic = "aboutmeiconnotext.png";

            Items.Add(new {
                Source = pic,
                Name = people.FirstName + " " + people.LastName,
                Relationship = people.Relation,
                PhoneNumber = people.PhoneNumber
            });
        }
        public VoiceIdentificationViewModel() {
            Items = new ObservableCollection<object>();
            List<People> list = new List<People>();
            loadPeople();
        }


        public async void loadPeople() {
            PeopleClient peopleClient = new PeopleClient("Ph2u3nRSZeYsWHitLSnv");
            //PeopleClient peopleClient = new PeopleClient(App.User.id);
            list = await peopleClient.GetAllPeopleAsync();
            Console.WriteLine("list count : " + list.Count);

            foreach (People people in list)
            {
                String pic;
                if (people.HavePic)
                    pic = people.picUrl;
                else
                    pic = "aboutmeiconnotext.png";

                string phoneNumber = people.PhoneNumber;
                Items.Add(new
                {
                    Source = pic,
                    Name = people.FirstName + " " + people.LastName,
                    Relationship = people.Relation,
                    PhoneNumber = people.PhoneNumber
                });
            }
        }

        /*public VoiceIdentificationViewModel(string name)
        {
            Items = new ObservableCollection<object>();

            *//*Items.Add(new
            {
                Source = pic,
                Name = people.name,
                Relationship = people.relationship,
                PhoneNumber = people.phoneNumber
            });*//*

        }*/
    }
}
