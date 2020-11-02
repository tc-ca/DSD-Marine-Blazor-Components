using DSD.MSS.Blazor.Components.AddressComplete.Models;
using DSD.MSS.Blazor.Components.AddressComplete.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DSD.MSS.Blazor.Components.AddressComplete
{
    internal static class CPAddressCompleteService
    {
        internal async static Task<IEnumerable<CPCountry>> GetCountries()
        {
            var json = Index.countries;
            return await Task.FromResult(JsonConvert.DeserializeObject<List<CPCountry>>(json));
        }

        internal static async Task<List<CPQuickLookup>> FindSuggestions(IHttpClientFactory clientFactory, string refererURL, string key, string searchterm, string lastid, string searchfor, string country, string languagepreference, int maxsuggestions, int maxresults)
        {
            //Build the url
            var url = "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&SearchTerm=" + System.Web.HttpUtility.UrlEncode(searchterm);
            url += "&LastId=" + System.Web.HttpUtility.UrlEncode(lastid);
            url += "&SearchFor=" + System.Web.HttpUtility.UrlEncode(searchfor);
            url += "&Country=" + System.Web.HttpUtility.UrlEncode(country);
            url += "&LanguagePreference=" + System.Web.HttpUtility.UrlEncode(languagepreference);
            url += "&MaxSuggestions=" + System.Web.HttpUtility.UrlEncode(maxsuggestions.ToString(CultureInfo.InvariantCulture));
            url += "&MaxResults=" + System.Web.HttpUtility.UrlEncode(maxresults.ToString(CultureInfo.InvariantCulture));

            //SearchTerm , String, The search term to find.If the LastId is provided, the SearchTerm searches within the results from the LastId.
            //LastId, String, The Id from a previous Find or FindByPosition. 
            //SearchFor, String, default: Everything, Filters the search results. 
            //Country String, default: CAN, The name or ISO 2 or 3 character code for the country to search in. Most country names will be recognised but the use of the ISO country code is recommended for clarity.
            //LanguagePreference, String, default: en, The 2 or 4 character language preference identifier e.g. (en, en - gb, en - us etc).
            //MaxSuggestions, Integer, default: 7, The maximum number of autocomplete suggestions to return.
            //MaxResults, Integer, default: 100, The maximum number of retrievable address results to return.

            var result = await AddressFindAPI(clientFactory, refererURL, url);
            return result;
        }

        internal static async Task<List<CPCompleteAddress>> Retrieve(IHttpClientFactory clientFactory, string refererURL, string key, string id)
        {
            //Build the url
            var url = "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&Id=" + System.Web.HttpUtility.UrlEncode(id);

            //Key, String, The key to use to authenticate to the service., AA11 - AA11 - AA11 - AA11
            //Id, String, The Id from a Find method to retrieve the details for., CAN | 1520704

            var result = await AddressRetrieveAPI(clientFactory, refererURL, url);
            return result;
        }

        private static async Task<List<CPQuickLookup>> AddressFindAPI(IHttpClientFactory clientFactory, string refererURL, string parameters)
        {
            var _httpClient = clientFactory.CreateClient();
            var baseAddress = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/Find/v2.10/json3ex.ws?";
            var request = new HttpRequestMessage(HttpMethod.Get, baseAddress + parameters);
            _httpClient.DefaultRequestHeaders.Add("Referer", refererURL);
            var responseMsg = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var jsonContent = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (responseMsg.IsSuccessStatusCode)
            {
                var returnData = JsonConvert.DeserializeObject<CPQuickLookupResponse>(jsonContent);
                return returnData == null ? null : returnData.Items;
            }
            else
            {
                return default;
            }
        }

        private static async Task<List<CPCompleteAddress>> AddressRetrieveAPI(IHttpClientFactory clientFactory, string refererURL, string parameters)
        {
            var _httpClient = clientFactory.CreateClient();
            var baseAddress = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/Retrieve/v2.11/json3ex.ws?";
            var request = new HttpRequestMessage(HttpMethod.Get, baseAddress + parameters);
            _httpClient.DefaultRequestHeaders.Add("Referer", refererURL);
            var responseMsg = await _httpClient.SendAsync(request).ConfigureAwait(false);
            var jsonContent = await responseMsg.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (responseMsg.IsSuccessStatusCode)
            {
                var returnData = JsonConvert.DeserializeObject<CPCompleteAddressResponse>(jsonContent);
                return returnData == null ? null : returnData.Items;
            }
            else
            {
                return default;
            }
        }
    }

}
