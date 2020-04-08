using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]

	public class Notification : INotifyPropertyChanged
	{
		public DateTime ta_before { get; set; }//???
		public DateTime ta_after { get; set; }
		public DateTime ta_during { get; set; }
		public DateTime user_before { get; set; }
		public DateTime user_after { get; set; }
		public DateTime user_during { get; set; }

		public string ta_before_message { get; set; }
		public string ta_after_message { get; set; }
		public string ta_during_message { get; set; }
		public string user_before_message { get; set; }
		public string user_after_message { get; set; }
		public string user_during_message { get; set; }//???

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
