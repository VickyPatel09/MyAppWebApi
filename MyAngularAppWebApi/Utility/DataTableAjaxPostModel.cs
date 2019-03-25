using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility
{
    public class DataTableAjaxPostModel
    {
        [JsonProperty(PropertyName = "columns")]
        public List<Column> Columns { get; set; }

        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        [JsonProperty(PropertyName = "order")]
        public List<Order> Order { get; set; }

        [JsonProperty(PropertyName = "search")]
        public Search Search { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }


        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
      
    }
}
