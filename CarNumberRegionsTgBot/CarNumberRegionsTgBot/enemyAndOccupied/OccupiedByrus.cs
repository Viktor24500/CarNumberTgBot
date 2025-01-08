namespace CarNumberRegionsTgBot.rus
{
    public static class OccupiedByrus
    {
        //Ukraine
        public static Dictionary<string, string> OccupiedUkrainianTerritories = new Dictionary<string, string>
        {
            { "Donetsk Region", "80,180" },
            { "Luhansk Region", "81,181" },
            { "Autonomous Republic of Crimea", "82,182,777" },
            { "Kherson Region", "84,184" },
            { "Zaporizhzhia Region", "85,185" }
        };

        //Georgia
        public static Dictionary<string, List<int>> OccupiedGeorgianTerritories = new Dictionary<string, List<int>>
        {

        };

        //Moldova
        public static Dictionary<string, string> OccupiedMoldovianTerritories = new Dictionary<string, string>
        {
            { "A", "Bendery" },
            { "B", "Grigoriopol" },
            { "E", "Dubossary" },
            { "H", "Tiraspol (only mopeds) "},
            { "K", "Kamenka" },
            { "P", "Rybnitsa" },
            { "C", "Slobodzeya" },
            { "T", "Tiraspol" }
        };

        //Chechnya 
        public static Dictionary<string, int> OccupiedChechenTerritories = new Dictionary<string, int>
        {
            { "Chechen Republic", 95 }
        };
    }
}
