using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class routine
	{
		public string id { get; set; }

		public string title { get; set; }

		public task[] tasks { get; set; }
	}
}