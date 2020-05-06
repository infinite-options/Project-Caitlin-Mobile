using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class routine : GratisObject, INotifyPropertyChanged
	{
		public Notification Notification { get; set; } = new Notification();

		public string id { get; set; }

		public bool isSublistAvailable { get; set; }

		public List<task> tasks { get; set; } = new List<task>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
