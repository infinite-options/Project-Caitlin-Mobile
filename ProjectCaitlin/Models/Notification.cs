using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]

	public class Notification : INotifyPropertyChanged
	{
		public NotificationTime user { get; set; } = new NotificationTime();

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class NotificationTime : INotifyPropertyChanged
	{
		public NotificationAttributes before { get; set; } = new NotificationAttributes();

		public NotificationAttributes during { get; set; } = new NotificationAttributes();

		public NotificationAttributes after { get; set; } = new NotificationAttributes();

		public event PropertyChangedEventHandler PropertyChanged;
	}

	public class NotificationAttributes : INotifyPropertyChanged
	{
		public TimeSpan time { get; set; }

		public string message { get; set; }

		public bool is_enabled { get; set; }

		public bool is_set { get; set; }

		public bool date_set { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
