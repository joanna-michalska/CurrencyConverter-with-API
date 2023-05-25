using CurrencyConverter_with_API.Models;
using Newtonsoft.Json;

namespace CurrencyConverter_with_API
{
    class Program
    {
        // Field for storing the personal API key. This should remain private, thus not commiting the real key to the public repository.
        private static string Apikey = "---PLACEHOLDER---";

        static async Task Main(string[] args)
        {
            // Collect input from user
            Console.Write("Enter the amount to convert: ");
            var amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the currency to convert from: ");
            var fromCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the currency to convert to: ");
            var toCurrency = Console.ReadLine().ToUpper();

            // Use try-catch clause in case of any unpredicted errors (like API server was down, wrong/expired token, etc.)
            try
            {
                // API call for current exchange rates
                var exchangeRate = await GetExchangeRate(fromCurrency, toCurrency);

                // Using the result of previous method, calculate the currency
                var convertedAmount = ConvertCurrency(amount, exchangeRate);

                // Print the result of conversion to console
                Console.WriteLine($"{amount} {fromCurrency} = {convertedAmount} {toCurrency}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task<double> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            // Create API url to fetch the exchange rate data for specified currencies
            var apiUrl = $"https://api.currencyapi.com/v3/latest?apikey={Apikey}&base_currency={fromCurrency}&currencies={toCurrency}";

            // Use http client to make a HTTP call
            using (var httpClient = new HttpClient())
            {
                // Make a HTTP GET call to get the data in JSON format
                var response = await httpClient.GetAsync(apiUrl);

                // Read JSON from the API response
                var json = await response.Content.ReadAsStringAsync();

                // Deserialize JSON format data to actual object/model
                var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponseModel>(json);

                // Check if data was fetched
                if (currencyResponse == null || currencyResponse.Data == null)
                {
                    throw new Exception("Failed to fetch exchange rate");
                }

                // Return exchange rate for the currencies
                return currencyResponse.Data.ToList().First().Value.Value;
            }
        }

        static double ConvertCurrency(double amount, double exchangeRate)
        {
            // Calculate the currency
            return amount * exchangeRate;
        }
    }
}