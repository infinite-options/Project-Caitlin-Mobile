﻿using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class goal : INotifyPropertyChanged
	{
		public string id { get; set; }

		public string title { get; set; }

		public string photo { get; set; }

		public List<action> actions { get; set; } = new List<action>();

		public event PropertyChangedEventHandler PropertyChanged;
	}
}