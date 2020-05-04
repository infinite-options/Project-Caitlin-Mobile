using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;
using ProjectCaitlin.Methods;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class user : INotifyPropertyChanged
	{
		[Id]
		public string id { get; set; }

		[MapTo("email_id")]
		public string email { get; set; }

		[MapTo("first_name")]
		public string firstName { get; set; }

		[MapTo("last_name")]
		public string lastName { get; set; }

		[Ignored]
		public aboutMe aboutMe { get; set; } = new aboutMe();

		[Ignored]
		public List<person> people { get; set; } = new List<person>();

		[Ignored]
		public List<photo> photos { get; set; } = new List<photo>();

		[Ignored]
		public List<routine> routines { get; set; } = new List<routine>();

		[Ignored]
		public List<goal> goals { get; set; } = new List<goal>();

		[Ignored]
		public List<EventsItems> CalendarEvents { get; set; } = new List<EventsItems>();

		[Ignored]
		public List<List<string>> photoURIs { get; set; } = new List<List<string>>();

		public event PropertyChangedEventHandler PropertyChanged;
    }
}
