using ProjectCaitlin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

using ProjectCaitlin.Models;

namespace ProjectCaitlin.ViewModel
{
    public class GreetingViewModel : BindableObject
    {
        private GreetingPage mainPage;
        public ObservableCollection<PeopleItemModel> Items { get; set; }

        public GreetingViewModel(GreetingPage mainPage)
        {
            this.mainPage = mainPage;
            Items = new ObservableCollection<PeopleItemModel>();

            foreach (person people in App.User.people)
            {
                String pic;
                if (people.pic != "")
                    pic = people.pic;
                else
                    pic = "aboutmeiconnotext.png";

                int peopleIdx = 0;
                string phoneNumber = people.phoneNumber;
                Items.Add(new PeopleItemModel(people.pic,
                    people.name,
                    new Command<int>(
                            async (int _actionIdx) =>
                            {
                                if (phoneNumber == "")
                                    await Application.Current.MainPage.DisplayAlert("Sorry!", $"Looks a phone number hasn't been registered with {people.name}.", "OK");
                                else
                                    Device.OpenUri(new Uri("tel:" + phoneNumber));
                            }
                    ),
                    peopleIdx));
                peopleIdx++;

                Console.WriteLine("People : " + people.pic);
            }
        }
    }
    public class PeopleItemModel : INotifyPropertyChanged
    {

        private string source;
        public string Source
        {
            get => source;
        }

        private string text;
        public string Text
        {
            get => text;
        }

        private Command<int> navigate;
        public Command<int> Navigate
        {
            get => navigate;
            set
            {
                if (navigate != value)
                {
                    navigate = value;
                    OnPropertyChanged(nameof(Navigate));
                }
            }
        }

        private int navigateIdx;
        public int NavigateIdx
        {
            get => navigateIdx;
            set
            {
                if (navigateIdx != value)
                {
                    navigateIdx = value;
                    OnPropertyChanged(nameof(NavigateIdx));
                }
            }
        }

        public PeopleItemModel(string _source, string _text, Command<int> _navigate, int _navigateIdx)
        {
            source = _source;
            text = _text;
            navigate = _navigate;
            navigateIdx = _navigateIdx;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}


