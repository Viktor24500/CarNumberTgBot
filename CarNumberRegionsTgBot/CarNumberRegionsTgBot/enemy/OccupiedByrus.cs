namespace CarNumberRegionsTgBot.rus
{
	public static class OccupiedByrus
	{
		//Ukraine
		public static Dictionary<string, List<int>> OccupiedUkrainianTerritories = new Dictionary<string, List<int>>
		{
			{ "Donetsk Region", new List<int> { 80, 180 } },
			{ "Luhansk Region", new List<int> { 81, 181 } },
			{ "Autonomous Republic of Crimea", new List<int> { 82, 182, 777 } },
			{ "Kherson Region", new List<int> { 84, 184 } },
			{ "Zaporizhzhia Region", new List<int> { 85, 185 } }
		};

		//Georgia
		public static Dictionary<string, List<int>> OccupiedGeorgianTerritories = new Dictionary<string, List<int>>
		{

		};

		//Moldova 
		public static Dictionary<string, string> OccupiedMoldovianTerritories = new Dictionary<string, string>
		{

		};

		//Chechnya 
		public static Dictionary<string, int> OccupiedChechenTerritories = new Dictionary<string, int>
		{
			{ "Chechen Republic", 95 }
		};
	}
}
