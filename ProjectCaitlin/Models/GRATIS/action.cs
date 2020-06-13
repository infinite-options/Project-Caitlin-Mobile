using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class action : atObject, INotifyPropertyChanged
	{
		public List<instruction> instructions { get; set; } = new List<instruction>();

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
