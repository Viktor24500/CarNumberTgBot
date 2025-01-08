using CarNumberRegionsTgBot.Result;
using CarNumberRegionsTgBot.Models;
using CarNumberRegionsTgBot.CheckCarByNumber;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using CarNumberRegionsTgBot.Ukraine;
using CarNumberRegionsTgBot.rus;

namespace CarNumberRegionsTgBot
{
    public class TgBot
    {
        private static TelegramBotClient _client;
        private static bool _isBazaGaiSession = false;
        private static BazaGai _bazaGai = new BazaGai();
        public static void getUpdate(string _token)
        {
            _client = new TelegramBotClient(_token);
            var me = _client.GetMeAsync().Result;
            if (me != null && !string.IsNullOrEmpty(me.Username))
            {
                int offset = 0;
                while (true)
                {
                    try
                    {
                        var updates = _client.GetUpdatesAsync(offset).Result;
                        if (updates != null && updates.Count() > 0)
                        {
                            foreach (var update in updates)
                            {
                                processUpdate(update);
                                offset = update.Id + 1;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    Thread.Sleep(1000);
                }
            }
        }
        private static async void processUpdate(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                    string text = update.Message.Text;
                    if (_isBazaGaiSession)
                    {
                        // If the user types "exit", end the ChatGPT session
                        if (text == "exit" || text == "Back")
                        {
                            _client.SendTextMessageAsync(update.Message.Chat.Id, "You have exited the BazaGai session.", replyMarkup: GetButtonReply());
                            _isBazaGaiSession = false;  // Reset the flag
                        }
                        else
                        {
                            // Handle the BazaGai query and get the response
                            Result<CarDetails> response = await _bazaGai.GetCar(text);
                            _client.SendTextMessageAsync(update.Message.Chat.Id, response.Data.ToString(), replyMarkup: BackButtonReply());
                        }
                    }
                    else
                    {
                        switch (text)
                        {
                            case "/start":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "Hello", replyMarkup: GetButtonReply());
                                break;
                            case "UA car numbers 2013-present":
                                string codes = "";
                                foreach (KeyValuePair<string, string> pair in NewRegionsCode.UkrainianCarNumbersRegionsAfter2013)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "UA car numbers 2004-2013":
                                codes = "";
                                foreach (KeyValuePair<string, string> pair in NewRegionsCode.UkrainianNewCarNumbersRegionsBefore2013)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "UA car numbers 1995-2004":
                                codes = "";
                                foreach (KeyValuePair<string, int> pair in OldRegionsCode.UkrainianOldCarNumbersRegions)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "UA car numbers (diplomatic)":
                                codes = "";
                                foreach (KeyValuePair<int, string> pair in DiplomaticCodes.DiplomaticRepresentations)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "Ukrainian occupied territories":
                                codes = "";
                                foreach (KeyValuePair<string, string> pair in OccupiedByrus.OccupiedUkrainianTerritories)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
                                }
                                string codesOfOccupiedRegion = $"Codes of occupied regions \n {codes}";
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codesOfOccupiedRegion}", replyMarkup: BackButtonReply());
                                break;
                            case "Moldavian occupied territories":
                                codes = "";
                                foreach (KeyValuePair<string, string> pair in OccupiedByrus.OccupiedMoldovianTerritories)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "Chechen occupied territories":
                                codes = "";
                                foreach (KeyValuePair<string, int> pair in OccupiedByrus.OccupiedChechenTerritories)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "ru car numbers military according to Wiki":
                                codes = "";
                                foreach (KeyValuePair<int, string> pair in MilitaryCodesrus.MilitaryCodesrusAccordingToWiki)
                                {
                                    codes = $"{codes} \n Code: {pair.Key}. Name: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "ru car numbers military according to lazlegend":
                                codes = "";
                                foreach (KeyValuePair<string, string> pair in MilitaryCodesrus.MilitaryCodesrusAccordingTolazlegend)
                                {
                                    codes = $"{codes} \n Codes: {pair.Key}. Names: {pair.Value}";
                                }
                                _client.SendTextMessageAsync(update.Message.Chat.Id, $"{codes}", replyMarkup: BackButtonReply());
                                break;
                            case "Baza Gai":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "Baza Gai:", replyMarkup: BackButtonReply());
                                _isBazaGaiSession = true;
                                break;
                            case "Back":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "Main", replyMarkup: GetButtonReply());
                                break;
                        }
                        break;
                    }
                    break;
            }
        }

        private static IReplyMarkup GetButtonReply()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                    new []
                    {
                        new KeyboardButton("UA car numbers 2013-present"),
                    },
                    new []
                    {
                        new KeyboardButton("UA car numbers 2004-2013"),
                    },
                    new []
                    {
                        new KeyboardButton("UA car numbers 1995-2004"),
                    },
                    new []
                    {
                        new KeyboardButton("UA car numbers (diplomatic)"),
                    },
                   new []
                    {
                        new KeyboardButton("Ukrainian occupied territories"),
                    },
                    new []
                    {
                        new KeyboardButton("Moldavian occupied territories"),
                    },
                    new []
                    {
                        new KeyboardButton("Chechen occupied territories"),
                    },
                    new []
                    {
                        new KeyboardButton("ru military car numbers according to Wiki"),
                    },
                    new []
                    {
                        new KeyboardButton("ru military car numbers according to LazLegend"),
                    },
                    new []
                    {
                        new KeyboardButton("Baza Gai")
                    }
            });
        }
        private static IReplyMarkup BackButtonReply()
        {
            return new ReplyKeyboardMarkup(new[]
            {
                new []
                {
                    new KeyboardButton("Back"),
                }
            });
        }

    }
}
