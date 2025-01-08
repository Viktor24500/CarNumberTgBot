namespace CarNumberRegionsTgBot
{
    public class Program
    {
        private const string _tokenBot = "7624223679:AAGncx3WROwQ03dPbb3MeYEKZHLlFTVex1A"; //token for bot
        static void Main(string[] args)
        {
            TgBot bot = new TgBot();
            try
            {
                bot.getUpdate(_tokenBot);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
