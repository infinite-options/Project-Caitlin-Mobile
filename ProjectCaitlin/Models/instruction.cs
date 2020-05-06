using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class instruction : GratisObject, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
	}
}
