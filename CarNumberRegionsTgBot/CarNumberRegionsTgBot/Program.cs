using Microsoft.Extensions.Configuration;

namespace CarNumberRegionsTgBot
{
	public class Program
	{
		private const string _tokenBot = "7491430607:AAEontZSaKJ1p_tvH_TulTQVtqHOUZEkXeM"; //token for bot
		static void Main(string[] args)
		{
			try
			{
				var configuration = new ConfigurationBuilder()
					//.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../../Properties"))
					.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Properties"))
					.AddJsonFile("launchSettings.json")
					.Build();
				TgBot.CreateFilesInstnce(configuration);
				TgBot.getUpdate(_tokenBot);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
