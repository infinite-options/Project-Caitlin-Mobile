using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class routine : grObject, INotifyPropertyChanged
	{
		public Notification Notification { get; set; } = new Notification();

		public List<task> tasks { get; set; } = new List<task>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
