using CarNumberRegionsTgBot.Enums;
using CarNumberRegionsTgBot.Models;
using CarNumberRegionsTgBot.Result;
using CarNumberRegionsTgBot.Validators;
using System.Net.Http.Json;

namespace CarNumberRegionsTgBot.CheckCarByNumber
{
    public class BazaGai
    {
        private int _counter = 1000; //reqests for month
        private const string _apiKey = "da6ba151f025d9ff0e3a33af6dc3d571";
        private const string _urlBazaGai = $"https://baza-gai.com.ua/nomer/";
        private int _counterForAllTime = 0;
        public async Task<Result<CarDetails>> GetCar(string carNumber)
        {
            DateTime dateTime = new DateTime(2025, 01, 02);
            DateTime today = DateTime.Now;
            if (dateTime.AddMonths(1) == today)
            {
                _counter = 1000;
            }
            Result<CarDetails> result = new Result<CarDetails>();
            if (string.IsNullOrEmpty(carNumber))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = $"carNumber can't be empty";
                Console.WriteLine($"Error: {today} - {result.ErrorCode} - carNumber can't be empty");
                return result;
            }
            if (!Validations.isCarNumberPatternValid(carNumber))
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = $"carNumber don't match pattern. Example AA0000AA. Please write on english";
                Console.WriteLine($"Error: {today} - {result.ErrorCode} - carNumber don't match pattern. Example AA0000AA. Please write on english");
                return result;
            }
            if (_counter < 0)
            {
                result.ErrorCode = (int)ErrorCodes.BadRequest;
                result.ErrorMessage = "Your free requests is expired";
                Console.WriteLine($"Error: {today} - {result.ErrorCode} - Your free requests is expired");
                return result;
            }
            string url = _urlBazaGai + carNumber;
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);

                HttpResponseMessage responseMessage = await client.GetAsync(url);
                _counter--;
                _counterForAllTime++;

                //logger
                Console.WriteLine($"You have {_counter} free requests");
                Console.WriteLine();
                Console.WriteLine($"{_counterForAllTime} requests from {dateTime} to {today}");
                if (responseMessage.IsSuccessStatusCode)
                {
                    result.Data = await responseMessage.Content.ReadFromJsonAsync<CarDetails>();
                }
                else
                {
                    result.ErrorCode = (int)responseMessage.StatusCode;
                    result.ErrorMessage = responseMessage.RequestMessage.ToString();
                    Console.WriteLine($"Error: {today} - {(int)responseMessage.StatusCode} - {responseMessage.RequestMessage}");
                    //Console.WriteLine($"Error: {today} - {responseMessage.RequestMessage}");
                    return result;
                }
                result.ErrorCode = (int)ErrorCodes.Success;
                result.ErrorMessage = $"You have {_counter} free requests";
            }
            return result;
        }
    }
}
