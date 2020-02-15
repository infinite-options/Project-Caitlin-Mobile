using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class step : INotifyPropertyChanged
	{
		public string title { get; set; }

		public string photo { get; set; }

		public bool is_complete { get; set; }

        public DateTime datetime_completed { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
    }
}
