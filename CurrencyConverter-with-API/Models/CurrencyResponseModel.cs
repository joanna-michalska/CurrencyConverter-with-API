namespace CurrencyConverter_with_API.Models
{
    public class CurrencyResponseModel
    {
        public MetaDataModel Meta { get; set; }
        public Dictionary<string, CurrencyDataModel> Data { get; set; }
    }
}