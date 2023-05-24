using Newtonsoft.Json;

namespace CurrencyConverter_with_API
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Enter the amount to convert: ");
            var amount = Convert.ToDouble(Console.ReadLine());

            Console.Write("Enter the currency to convert from: ");
            var fromCurrency = Console.ReadLine().ToUpper();

            Console.Write("Enter the currency to convert to: ");
            var toCurrency = Console.ReadLine().ToUpper();

            try
            {
                var exchangeRate = await GetExchangeRate(fromCurrency, toCurrency);
                var convertedAmount = ConvertCurrency(amount, exchangeRate);

                Console.WriteLine($"{amount} {fromCurrency} = {convertedAmount} {toCurrency}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        static async Task<double> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var apiKey = "---PLACEHOLDER---";
            var apiUrl = $"https://api.currencyapi.com/v3/latest?apikey={apiKey}&base_currency={fromCurrency}&currencies={toCurrency}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(apiUrl);
                var json = await response.Content.ReadAsStringAsync();
                var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponseModel>(json);

                if (currencyResponse == null)
                {
                    throw new Exception("Failed to fetch exchange rate");
                }

                return currencyResponse.Data.ToList().First().Value.Value;
            }
        }

        static double ConvertCurrency(double amount, double exchangeRate)
        {
            return amount * exchangeRate;
        }
    }
}