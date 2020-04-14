using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
    [JsonObject]
    public class people : INotifyPropertyChanged
    {
        public bool have_pic { get; set; }

        public string name { get; set; }

        public string phone_number { get; set; }

        public string pic { get; set; }

        public string relationship { get; set; }

        public string unique_id { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}