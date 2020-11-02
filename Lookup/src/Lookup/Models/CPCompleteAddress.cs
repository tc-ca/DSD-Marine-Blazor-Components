namespace DSD.MSS.Blazor.Components.AddressComplete
{
    public class CPCompleteAddress
    {   
        //Canada Post return data structure for Complete Address lookup based on AddressCompleteInteractiveRetrievev211 Key Id lookup
        public string Id { get; set; }
        public string DomesticId { get; set; }
        public string Language { get; set; }
        public string LanguageAlternatives { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string SubBuilding { get; set; }
        public string BuildingNumber { get; set; }
        public string BuildingName { get; set; }
        public string SecondaryStreet { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string Neighbourhood { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string AdminAreaName { get; set; }
        public string AdminAreaCode { get; set; }
        public string Province { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string CountryIso2 { get; set; }
        public string CountryIso3 { get; set; }
        public int CountryIsoNumber { get; set; }
        public string SortingNumber1 { get; set; }
        public string SortingNumber2 { get; set; }
        public string Barcode { get; set; }
        public string POBoxNumber { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string DataLevel { get; set; }
        public string Description { get; set; }
        public string Error { get; set; }
        public string Cause { get; set; }
        public string Resolution { get; set; }
    }
}
