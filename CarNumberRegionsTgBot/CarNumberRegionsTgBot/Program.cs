using Microsoft.Extensions.Configuration;

namespace CarNumberRegionsTgBot
{
	public class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var configuration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: true)
					.AddEnvironmentVariables()
					.Build();

				var token = configuration["BOT_TOKEN"];

				TgBot.CreateFilesInstnce(configuration);
				TgBot.getUpdate(token);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}
	}
}
