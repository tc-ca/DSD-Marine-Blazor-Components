namespace AddressComplete.Demo.Models
{
    public class AddressApi
    {   
        public string BaseUrlFind { get; set; }
        public string BaseUrlRetrieve { get; set; }
        public string APIKey { get; set; }
        public string RefererURL { get; set; }
        public string VerificationCode { get; set; }
        public int MaxSuggestions { get; set; }
        public int MaxResults { get; set; }
        public bool PrefixBuildingNumberWithSub { get; set; }
        public int MaxGroupSuggestions { get; set; }
    }
}
