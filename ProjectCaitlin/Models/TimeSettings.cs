using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectCaitlin.Models
{
    public class TimeSettings: INotifyPropertyChanged
    {
        public string afternoon { get; set; }
        public string dayEnd { get; set; }
        public string dayStart { get; set; }
        public string evening { get; set; }
        public string morning { get; set; }
        public string night { get; set; }
        public string timeZone { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
