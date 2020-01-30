using Newtonsoft.Json;

namespace ProjectCaitlin.Models
{
	[JsonObject]
	public class step
	{
		public string title { get; set; }

		public string status { get; set; }

	}
}