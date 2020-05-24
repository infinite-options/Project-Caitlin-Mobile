using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
    [JsonObject]
    public class aboutMe : INotifyPropertyChanged
    {
        public bool have_pic { get; set; }

        public string message_card { get; set; }

        public string message_day { get; set; }

        public string pic { get; set; }

        public List<people> peoples { get; set; } = new List<people>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
