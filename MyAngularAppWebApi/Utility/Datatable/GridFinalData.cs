using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility.Datatable
{
    public class GridFinalData
    {
        [JsonProperty(PropertyName = "data")]
        public List<Dictionary<string, object>> Data { get; set; }

        [JsonProperty(PropertyName = "draw")]
        public string Draw { get; set; }

        [JsonProperty(PropertyName = "recordsFiltered")]
        public string RecordsFiltered { get; set; }

        [JsonProperty(PropertyName = "recordsTotal")]
        public string RecordsTotal { get; set; }
    }
}
