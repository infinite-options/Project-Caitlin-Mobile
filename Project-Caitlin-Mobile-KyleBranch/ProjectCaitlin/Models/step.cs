using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class step : INotifyPropertyChanged
	{
		public string title { get; set; }

		public string status { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}