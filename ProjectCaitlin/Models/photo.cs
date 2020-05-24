using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
    [JsonObject]
    public class photo : INotifyPropertyChanged{
        public string id { get; set; }

        public string description { get; set; }

        public string note { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }


}
