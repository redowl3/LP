using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaunchPad.Mobile.Models
{
    public class CountryCodeModel
    {

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("country_name")]
        public string CountryName { get; set; }

        [JsonProperty("dialling_code")]
        public string DiallingCode { get; set; }
    }

    public class Country
    {

        [JsonProperty("countryCodes")]
        public List<CountryCodeModel> CountryCodes { get; set; }
    }

}
