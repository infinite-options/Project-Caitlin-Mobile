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

		public List<Step> steps { get; set; } = new List<Step>();

		public event PropertyChangedEventHandler PropertyChanged;
    }
}