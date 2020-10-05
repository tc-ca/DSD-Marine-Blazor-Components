using Lookup.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Lookup
{
    internal static class CPAddressCompleteService
    {
        internal async static Task<IEnumerable<CPCountry>> GetCountries()
        {
            var json = Index.countries;
            return await Task.FromResult(JsonConvert.DeserializeObject<List<CPCountry>>(json));
        }

        internal static DataSet FindSuggestions(string key, string searchterm, string lastid, string searchfor, string country, string languagepreference, int maxsuggestions, int maxresults)
        {            
            //Build the url
            var url = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/Find/v2.10/dataset.ws?";
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&SearchTerm=" + System.Web.HttpUtility.UrlEncode(searchterm);
            url += "&LastId=" + System.Web.HttpUtility.UrlEncode(lastid);
            url += "&SearchFor=" + System.Web.HttpUtility.UrlEncode(searchfor);
            url += "&Country=" + System.Web.HttpUtility.UrlEncode(country);
            url += "&LanguagePreference=" + System.Web.HttpUtility.UrlEncode(languagepreference);
            url += "&MaxSuggestions=" + System.Web.HttpUtility.UrlEncode(maxsuggestions.ToString(CultureInfo.InvariantCulture));
            url += "&MaxResults=" + System.Web.HttpUtility.UrlEncode(maxresults.ToString(CultureInfo.InvariantCulture));

            //Key Temp 14 days from June 9th 2020: DM64-YB56-WY57-JB29
            //SearchTerm , String, The search term to find.If the LastId is provided, the SearchTerm searches within the results from the LastId.
            //LastId, String, The Id from a previous Find or FindByPosition. 
            //SearchFor, String, default: Everything, Filters the search results. 
            //Country String, default: CAN, The name or ISO 2 or 3 character code for the country to search in. Most country names will be recognised but the use of the ISO country code is recommended for clarity.
            //LanguagePreference, String, default: en, The 2 or 4 character language preference identifier e.g. (en, en - gb, en - us etc).
            //MaxSuggestions, Integer, default: 7, The maximum number of autocomplete suggestions to return.
            //MaxResults, Integer, default: 100, The maximum number of retrievable address results to return.

            //Create the dataset
            var dataSet = new DataSet();
            dataSet.ReadXml(url);

            //Check for an error
            if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 && dataSet.Tables[0].Columns[0].ColumnName == "Error")
                throw new Exception(dataSet.Tables[0].Rows[0].ItemArray[1].ToString());

            //Return the dataset
            return dataSet;

            //FYI: The dataset contains the following columns:
            //Name, Type, Description, Values(optional), Example

            //Id, String, The Id to be used as the LastId with the Find method., CAN | PR | X247361852 | E | 0 | 0
            //Text, String, The found item., 2701 Riverside Dr, Ottawa, ON
            //Highlight, String, A list of number ranges identifying the characters to highlight in the Text response(zero - based start position and end). , 0 - 2,6 - 4
            //Cursor, Integer,  A zero-based position in the Text response indicating the suggested position of the cursor if this item is selected.A - 1 response indicates no suggestion is available., 0
            //Description, String, Descriptive information about the found item, typically if it's a container., 102 Streets 
            //Next, String, The next step of the search process., Values(Find Retrieve), Retrieve

        }

        internal static DataSet Retrieve(string key, string id)
        {
            //Build the url
            var url = "http://ws1.postescanada-canadapost.ca/AddressComplete/Interactive/Retrieve/v2.11/dataset.ws?";
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&Id=" + System.Web.HttpUtility.UrlEncode(id);

            //Key, String, The key to use to authenticate to the service., AA11 - AA11 - AA11 - AA11
            //Id, String, The Id from a Find method to retrieve the details for., CAN | 1520704


            //Create the dataset
            var dataSet = new System.Data.DataSet();
            dataSet.ReadXml(url);

            //Check for an error
            if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 && dataSet.Tables[0].Columns[0].ColumnName == "Error")
                throw new Exception(dataSet.Tables[0].Rows[0].ItemArray[1].ToString());

            //Return the dataset
            return dataSet;

            //FYI: The dataset contains the following columns:
            //Id
            //DomesticId
            //Language
            //LanguageAlternatives
            //Department
            //Company
            //SubBuilding
            //BuildingNumber
            //BuildingName
            //SecondaryStreet
            //Street
            //Block
            //Neighbourhood
            //District
            //City
            //Line1
            //Line2
            //Line3
            //Line4
            //Line5
            //AdminAreaName
            //AdminAreaCode
            //Province
            //ProvinceName
            //ProvinceCode
            //PostalCode
            //CountryName
            //CountryIso2
            //CountryIso3
            //CountryIsoNumber
            //SortingNumber1
            //SortingNumber2
            //Barcode
            //POBoxNumber
            //Label
            //Type
            //DataLevel
        }

        internal static IList<CPQuickLookup> GetLookupListFromTable(DataTable table)
        {
            IList<CPQuickLookup> list = new List<CPQuickLookup>();

            EnumerableRowCollection<DataRow> rows = table.AsEnumerable();
            list = (from DataRow dr in rows
                    select new CPQuickLookup
                    {
                               Id = dr["Id"].ToString(),
                               Cursor = int.Parse(dr["Cursor"].ToString()),
                               Description = dr["Description"].ToString(),
                               Highlight = dr["Highlight"].ToString(),
                               Next = dr["Next"].ToString(),
                               Text = dr["Text"].ToString()
                           }).ToList();

            return list;
        }
    }

}
