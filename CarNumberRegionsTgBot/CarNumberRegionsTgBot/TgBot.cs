using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;

namespace CarNumberRegionsTgBot
{
    public class TgBot
    {
        private static TelegramBotClient _client;
        private bool _isBazaGaiSession = false;
        private BazaGai _bazaGai = new BazaGai();
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
        private async void processUpdate(Update update)
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
                            Result<CarDetails> response = await _bazaGai(text);
                            _client.SendTextMessageAsync(update.Message.Chat.Id, response.Data, replyMarkup: BackButtonReply());
                        }
                    }
                    else
                    {
                        switch (text)
                        {
                            case "/start":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "Hello", replyMarkup: GetButtonReply());
                                break;
                            case "UA car numbers (new)":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "Підгорний Віктор гр ІК-13", replyMarkup: BackButtonReply());
                                break;
                            case "UA car numbers (old)":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "Backend", replyMarkup: BackButtonReply());
                                break;
                            case "UA car numbers (diplomatic)":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "phone: +380542240450 \n email: xxxx@gmail.com", replyMarkup: BackButtonReply());
                                break;
                            case "ru car numbers (occupied territories)":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "ru car numbers (occupied territories):", replyMarkup: BackButtonReply());
                                _isChatGPTSession = true;
                                break;
                            case "ru car numbers (military)":
                                _client.SendTextMessageAsync(update.Message.Chat.Id, "ru car numbers (military):", replyMarkup: BackButtonReply());
                                _isChatGPTSession = true;
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
                        new KeyboardButton("UA car numbers (new)"),
                        new KeyboardButton("UA car numbers (old)"),
                        new KeyboardButton("UA car numbers (diplomatic)"),
                    },
                    new []
                    {
                        new KeyboardButton("ru car numbers (occupied territories)"),
                        new KeyboardButton("ru car numbers (military)"),
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
