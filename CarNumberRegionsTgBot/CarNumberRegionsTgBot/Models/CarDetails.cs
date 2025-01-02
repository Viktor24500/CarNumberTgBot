using Newtonsoft.Json;

namespace CarNumberRegionsTgBot.Models
{
	public class CarDetails
	{
		public string Digits { get; set; }
		//public string Vin { get; set; }
		public string Vendor { get; set; }
		public string Model { get; set; }
		[JsonProperty("model_year")]
		public int ModelYear { get; set; }
		[JsonProperty("photo_url")]
		public string PhotoUrl { get; set; }
		[JsonProperty("is_stolen")]
		public bool IsStolen { get; set; }
		public int CarPrice { get; set; }
		public Region Region { get; set; }
		public ClientDetails ClientDetails { get; set; }
	}
}
