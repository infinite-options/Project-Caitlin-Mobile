using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class instruction : INotifyPropertyChanged
	{
		public string title { get; set; }

		public string status { get; set; }
		public string photo { get; set; }


		public event PropertyChangedEventHandler PropertyChanged;
	}
}