using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class task : INotifyPropertyChanged
	{
		public string id { get; set; }

		public string title { get; set; }

		public string photo { get; set; }

		public bool isInProgress { get; set; }

		public bool isComplete { get; set; }

		public int dbIdx { get; set; }

		public TimeSpan expectedCompletionTime { get; set; }

		public bool isSublistAvailable { get; set; }

		public DateTime dateTimeCompleted { get; set; }

		public TimeSpan availableStartTime { get; set; }

		public TimeSpan availableEndTime { get; set; }

		public List<step> steps { get; set; } = new List<step>();

		public event PropertyChangedEventHandler PropertyChanged;
    }
}
