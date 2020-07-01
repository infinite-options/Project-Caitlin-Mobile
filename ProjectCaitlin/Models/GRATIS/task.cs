using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class task : atObject, INotifyPropertyChanged
	{
		public List<step> steps { get; set; } = new List<step>();

		public event PropertyChangedEventHandler PropertyChanged;
    }
}
