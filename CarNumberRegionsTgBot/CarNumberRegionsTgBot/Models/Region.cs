using Newtonsoft.Json;

namespace CarNumberRegionsTgBot.Models
{
	public class Region
	{
		public string Name { get; set; }
		[JsonProperty("name_ua")]
		public string NameUA { get; set; }
		public string Slug { get; set; }
		[JsonProperty("old_code")]
		public string OldCode { get; set; }
		[JsonProperty("new_code")]
		public string NewCode { get; set; }
		[JsonProperty("new_code_1")]
		public string NewCode1 { get; set; }
		[JsonProperty("new_code_2")]
		public string NewCode2 { get; set; }
	}
}
