using System;
namespace ProjectCaitlin.Models
{
    public class GratisObject
    {
		public string title { get; set; }

		public string status { get; set; }

		public string photo { get; set; }

		public bool isInProgress { get; set; }

		public bool isComplete { get; set; }

		public int dbIdx { get; set; }

		public TimeSpan expectedCompletionTime { get; set; }

		public DateTime dateTimeCompleted { get; set; }

		public TimeSpan availableStartTime { get; set; }

		public TimeSpan availableEndTime { get; set; }
	}
}
