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

				if (string.IsNullOrWhiteSpace(token))
				{
					throw new Exception("BOT_TOKEN is missing");
				}

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
