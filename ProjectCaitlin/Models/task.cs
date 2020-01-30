using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class task
	{
		public string id { get; set; }

		public string title { get; set; }

		public step[] steps { get; set; }
	}
}