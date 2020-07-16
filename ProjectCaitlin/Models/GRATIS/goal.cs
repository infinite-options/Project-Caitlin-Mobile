using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class goal : grObject, INotifyPropertyChanged
	{
		public Notification Notification { get; set; } = new Notification();
		public List<action> actions { get; set; } = new List<action>();

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
