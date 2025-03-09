using CarNumberRegionsTgBot.CheckCarByNumber;
using CarNumberRegionsTgBot.Enums;
using CarNumberRegionsTgBot.Models;
using CarNumberRegionsTgBot.Result;
using CarNumberRegionsTgBot.rus;
using CarNumberRegionsTgBot.Ukraine;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CarNumberRegionsTgBot
{
	public class TgBot
	{
		private static TelegramBotClient _client;
		private static bool _isBazaGaiSession = false;
		private static BazaGai _bazaGai = new BazaGai();
		private static bool _isContactToAdminSession = false;
		private static bool _isFirstMessage = true;
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
							string result = string.Empty;
							string nameofErrorCode = string.Empty;
							// Handle the BazaGai query and get the response
							Result<CarDetails> response = await _bazaGai.GetCar(text);
							nameofErrorCode = Enum.GetName(typeof(ErrorCodes), response.ErrorCode);
							if (response.ErrorCode != (int)ErrorCodes.Success)
							{
								string errorMessage = string.Empty;
								if (response.ErrorCode == (int)ErrorCodes.NotFound)
								{
									errorMessage = $"Car with number {text} not found";
								}
								if (response.ErrorCode == (int)ErrorCodes.InternalServerError)
								{
									errorMessage = $"Server Erorr";
								}
								result = $"Code: {nameofErrorCode}. Message: {errorMessage}";
							}
							else
							{
								string resultClient = string.Empty;
								string resultCar = string.Empty;
								string resultRegion = string.Empty;
								foreach (var prop in response.Data.GetType().GetProperties())
								{
									if (response.Data.ClientDetails != null && string.IsNullOrEmpty(resultCar))
									{
										foreach (var propClientDetails in response.Data.ClientDetails.GetType().GetProperties())
										{
											resultCar = resultCar + $"{propClientDetails.Name}: {propClientDetails.GetValue(response.Data.ClientDetails, null)} \n";
										}
									}
									if (response.Data.Region != null && string.IsNullOrEmpty(resultRegion))
									{
										foreach (var propRegionDetails in response.Data.Region.GetType().GetProperties())
										{
											resultRegion = resultRegion + $"{propRegionDetails.Name}: {propRegionDetails.GetValue(response.Data.Region, null)} \n";
										}
									}
									if (prop.PropertyType == typeof(Region) || prop.PropertyType == typeof(ClientDetails))
									{
										continue;
									}
									resultClient = resultClient + $"{prop.Name}: {prop.GetValue(response.Data, null)} \n";
								}
								result = result + " " + resultCar + " " + resultRegion + " " + resultClient;

							}

							_client.SendTextMessageAsync(update.Message.Chat.Id, result, replyMarkup: BackButtonReply());
						}
					}
					else if (_isContactToAdminSession)
					{
						// If the user types "exit", end the ChatGPT session
						if (text == "exit" || text == "Back")
						{
							_client.SendTextMessageAsync(update.Message.Chat.Id, "You have exited the session.", replyMarkup: GetButtonReply());
							_isContactToAdminSession = false;  // Reset the flag
							_isFirstMessage = true;
						}
						else
						{
							DateTime time = DateTime.Now;
							Console.WriteLine($"User info: {update.Message.Chat.Username}");
							Console.WriteLine($"{time} - User: {text}");

							// Check if it's the first message in the chat (you need a way to track this)
							// Replace this with actual tracking logic

							if (!_isFirstMessage)
							{
								Console.Write("Admin enter your text: ");
								string adminText = Console.ReadLine();
								time = DateTime.Now;
								Console.WriteLine($"{time} - Admin: {adminText}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, adminText, replyMarkup: BackButtonReply());
							}
							else
							{
								Console.WriteLine("Waiting for admin response...");
								_isFirstMessage = false;
								_client.SendTextMessageAsync(update.Message.Chat.Id, "Welcome", replyMarkup: BackButtonReply());
							}
							//// Handle the ChatGPT query and get the response
							//DateTime time = new DateTime();
							//time = DateTime.Now;
							//Console.WriteLine($"User info: {update.Message.Chat.Username}");
							//Console.WriteLine($"{time} - User: {text}");
							//Console.Write("Admin enter your text: ");
							//string adminText = Console.ReadLine();
							//time = DateTime.Now;
							//Console.WriteLine($"{time} - Admin: {adminText}");
							//_client.SendTextMessageAsync(update.Message.Chat.Id, adminText, replyMarkup: BackButtonReply());

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
								Console.WriteLine($"UA car numbers 2013-present: {DateTime.Now} - {codes}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, codes, replyMarkup: BackButtonReply());
								break;
							case "UA car numbers 2004-2013":
								codes = "";
								foreach (KeyValuePair<string, string> pair in NewRegionsCode.UkrainianNewCarNumbersRegionsBefore2013)
								{
									codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
								}
								Console.WriteLine($"UA car numbers 2004-2013: {DateTime.Now} - {codes}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, codes, replyMarkup: BackButtonReply());
								break;
							case "UA car numbers 1995-2004":
								codes = "";
								foreach (KeyValuePair<string, int> pair in OldRegionsCode.UkrainianOldCarNumbersRegions)
								{
									codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
								}
								Console.WriteLine($"UA car numbers 1995-2004: {DateTime.Now} - {codes}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, codes, replyMarkup: BackButtonReply());
								break;
							case "UA car numbers (diplomatic)":
								codes = "";
								foreach (KeyValuePair<int, string> pair in DiplomaticCodes.DiplomaticRepresentations)
								{
									codes = $"{codes} \n Code: {pair.Key}. Name: {pair.Value}";
								}
								Console.WriteLine($"UA car numbers (diplomatic): {DateTime.Now} - {codes}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, codes, replyMarkup: BackButtonReply());
								break;
							case "Ukrainian occupied territories":
								codes = "";
								foreach (KeyValuePair<string, string> pair in OccupiedByrus.OccupiedUkrainianTerritories)
								{
									codes = $"{codes} \n Region: {pair.Key}. Code: {pair.Value}";
								}
								string codesOfOccupiedRegion = $"Codes of occupied regions \n {codes}";
								Console.WriteLine($"Ukrainian occupied territories: {DateTime.Now} - {codesOfOccupiedRegion}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, codesOfOccupiedRegion, replyMarkup: BackButtonReply());
								break;
							case "Moldavian occupied territories":
								codes = "";
								foreach (KeyValuePair<string, string> pair in OccupiedByrus.OccupiedMoldovianTerritories)
								{
									codes = $"{codes} \n Code: {pair.Key}. Region: {pair.Value}";
								}
								codesOfOccupiedRegion = $"Codes of occupied regions \n {codes}";
								Console.WriteLine($"Moldavian occupied territories: {DateTime.Now} - {codesOfOccupiedRegion}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, codesOfOccupiedRegion, replyMarkup: BackButtonReply());
								break;
							case "Chechen occupied territories":
								codes = "";
								foreach (KeyValuePair<string, int> pair in OccupiedByrus.OccupiedChechenTerritories)
								{
									codes = $"{codes} \n Region: {pair.Key}. Code: {pair.Value}";
								}
								codesOfOccupiedRegion = $"Codes of occupied regions \n {codes}";
								Console.WriteLine($"Chechen occupied territories: {DateTime.Now} - {codesOfOccupiedRegion}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, codesOfOccupiedRegion, replyMarkup: BackButtonReply());
								break;
							case "ru military car numbers according to Wiki":
								codes = "";
								foreach (KeyValuePair<int, string> pair in MilitaryCodesrus.MilitaryCodesrusAccordingToWiki)
								{
									codes = $"{codes} \n Code: {pair.Key}. Name: {pair.Value}";
								}
								string militaryCodes = $"Military codes \n {codes}";
								Console.WriteLine($"Militari according to wiki: {DateTime.Now} - {militaryCodes}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, militaryCodes, replyMarkup: BackButtonReply());
								break;
							case "ru military car numbers according to lazlegend": //responce too long
								codes = "";
								foreach (KeyValuePair<string, string> pair in MilitaryCodesrus.MilitaryCodesrusAccordingTolazlegend)
								{
									codes = $"{codes} \n Codes: {pair.Key}. Names: {pair.Value}";
								}
								militaryCodes = $"Military codes \n {codes}";
								Console.WriteLine($"Militari according to lazlegend: {DateTime.Now} - {militaryCodes}");
								_client.SendTextMessageAsync(update.Message.Chat.Id, militaryCodes, replyMarkup: BackButtonReply());
								break;
							case "Baza Gai":
								_client.SendTextMessageAsync(update.Message.Chat.Id, "Baza Gai:", replyMarkup: BackButtonReply());
								Console.WriteLine($"Baza Gai");
								_isBazaGaiSession = true;
								break;
							case "Contact to Admin":
								_client.SendTextMessageAsync(update.Message.Chat.Id, "Contact to admin:", replyMarkup: BackButtonReply());
								_isContactToAdminSession = true;
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

		private static ReplyMarkup GetButtonReply()
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
						new KeyboardButton("ru military car numbers according to lazlegend"),
					},
					new []
					{
						new KeyboardButton("Baza Gai")
					},
					new []
					{
						new KeyboardButton("Contact to Admin")
					}
			});
		}
		private static ReplyMarkup BackButtonReply()
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
