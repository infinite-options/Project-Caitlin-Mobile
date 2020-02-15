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

		public List<instruction> instructions { get; set; } = new List<instruction>();

		public event PropertyChangedEventHandler PropertyChanged;
	}
}