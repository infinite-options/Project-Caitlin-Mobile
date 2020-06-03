using ProjectCaitlin.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace ProjectCaitlin.ViewModel
{

    class VoiceIdentificationViewModel : BindableObject
    {
        public ObservableCollection<object> Items { get; set; }

        public VoiceIdentificationViewModel()
        {
            Items = new ObservableCollection<object>();

            App.User.people.Clear();
            person a = new person();
            a.name = "Bob";
            a.pic = "";
            a.relationship = "Father";
            a.phoneNumber = "123456";
            App.User.people.Add(a);

            person b = new person();
            b.name = "John";
            b.pic = "";
            b.relationship = "Brother";
            b.phoneNumber = "123456";
            App.User.people.Add(b);

            person c = new person();
            c.name = "Jenny";
            c.pic = "";
            c.relationship = "Mother";
            c.phoneNumber = "123456";
            App.User.people.Add(c);

            foreach (person people in App.User.people)
            {
                String pic;
                if (people.pic != "")
                    pic = people.pic;
                else
                    pic = "aboutmeiconnotext.png";

                string phoneNumber = people.phoneNumber;
                Items.Add(new
                {
                    Source = pic,
                    Name = people.name,
                    Relationship = people.relationship,
                    PhoneNumber = people.phoneNumber
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
