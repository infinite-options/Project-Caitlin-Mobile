using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class action : INotifyPropertyChanged
	{
		public string id { get; set; }

		public string title { get; set; }

		public string photo { get; set; }

		public bool isInProgress { get; set; }

		public bool isComplete { get; set; }

		public int dbIdx { get; set; }

		public bool isSublistAvailable { get; set; }

		public DateTime dateTimeCompleted { get; set; }

		public DateTime availableStartTime { get; set; }

		public DateTime availableEndTime { get; set; }

		public List<instruction> instructions { get; set; } = new List<instruction>();

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
