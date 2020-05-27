using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VoiceRecognition.Model
{
    public class User : INotifyPropertyChanged
    {
        public string id { get; set; }

        public string email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public string old_refresh_token { get; set; }

        //public List<Routine> routines { get; set; } = new List<Routine>();

        //public List<goal> goals { get; set; } = new List<Goal>();

        //public List<EventsItems> CalendarEvents { get; set; } = new List<EventsItems>();


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
