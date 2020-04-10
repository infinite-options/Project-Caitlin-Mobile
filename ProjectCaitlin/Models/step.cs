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

		public bool isInProgress { get; set; }

		public bool isComplete { get; set; }

        public int dbIdx { get; set; }

		public int expected_completion_time { get; set; }

		public DateTime dateTimeCompleted { get; set; }

		public DateTime availableStartTime { get; set; }

		public DateTime availableEndTime { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
    }
}
