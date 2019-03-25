using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility
{
    public class Search
    {
        [JsonProperty(PropertyName = "regex")]
        public string Regex { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
