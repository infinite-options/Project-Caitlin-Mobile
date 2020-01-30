using System.Collections.Generic;
using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class user
	{
		public string id { get; set; }

		public string email { get; set; }

		public string firstName { get; set; }

		public string lastName { get; set; }

		public List<routine> routines = new List<routine>();

		public List<routine> goals = new List<routine>();
	}
}