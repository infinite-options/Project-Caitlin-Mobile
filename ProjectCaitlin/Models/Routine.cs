using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class routine : INotifyPropertyChanged
	{
		public string id { get; set; }

		public string title { get; set; }

		public string photo { get; set; }

		public bool is_complete { get; set; }

		public DateTime datetime_completed { get; set; }

		public List<task> tasks { get; set; } = new List<task>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
