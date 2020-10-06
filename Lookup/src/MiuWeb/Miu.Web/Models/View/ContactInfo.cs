using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AddressComplete.Demo.Models.View
{
    public class ContactInfo
    {

        public string CompanyName { get; set; }

        public string BusinessNumber { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string ExtNumber { get; set; }

        public string Email { get; set; }

        public bool ShowEmail { get; set; }

        public UserInfoIndex Index { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressCity { get; set; }

        public string AddressCountry { get; set; }
        public string AddressCountryCode { get; set; }

        public string AddressProvince { get; set; }

        public string AddressPostalCode { get; set; }

    }
}
