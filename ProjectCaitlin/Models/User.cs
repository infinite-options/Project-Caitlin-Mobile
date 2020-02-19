using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class user : INotifyPropertyChanged
	{
		public string id { get; set; }

		public string email { get; set; }

		public string firstName { get; set; }

		public string lastName { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public string old_refresh_token { get; set; }

        public List<routine> routines { get; set; } = new List<routine>();

		public List<goal> goals { get; set; } = new List<goal>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
