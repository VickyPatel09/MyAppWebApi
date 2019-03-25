using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility
{
    public class GridParameter
    {
        public List<Column> Columns { get; set; }

        public string ConnectionString { get; set; }

        public int Draw { get; set; }

        public bool IsExport { get; set; }

        public List<Order> Orders { get; set; }

        public int PageNumber { get; set; }

        public int RecordPerPage { get; set; }

        public string Search { get; set; }

        public string SortOrder { get; set; }

        public string Type { get; set; }

        public string Where { get; set; }
    }
}
