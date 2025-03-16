namespace CarNumberRegionsTgBot
{
	public class Program
	{
		private const string _tokenBot = "7491430607:AAEontZSaKJ1p_tvH_TulTQVtqHOUZEkXeM"; //token for bot
		static void Main(string[] args)
		{
			try
			{
				TgBot.getUpdate(_tokenBot);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}
