using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
    [JsonObject]
    public class person : INotifyPropertyChanged
    {
        public string speakerId { get; set; }

        public string name { get; set; }

        public string phoneNumber { get; set; }

        public string pic { get; set; }

        public string relationship { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
