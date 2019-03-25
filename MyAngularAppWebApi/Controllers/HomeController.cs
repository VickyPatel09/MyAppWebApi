using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyAngularAppWebApi.Utility;
using MyAngularAppWebApi.Utility.Datatable;
using Newtonsoft.Json;

namespace MyAngularAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IOptions<ConnectionStrings> connectionStrings;

        HomeController (IOptions<ConnectionStrings> connectionStrings)
        {
            this.connectionStrings = connectionStrings;
        }

        /// <summary>
        /// GET ALL COMMON TABLE LIST 
        /// </summary>
        [HttpPost]
        [Route("GetTableList")]
        public IActionResult GetTableList()
        {
            string requestData;
            using (var reader = new StreamReader(this.Request.Body, Encoding.UTF8))
            {
                requestData = reader.ReadToEnd();
            }

            // DESERIALIZE DATATABLE JSON OBJECT
            var param = JsonConvert.DeserializeObject<DataTableAjaxPostModel>(requestData);

            // SET OBJECT PARAMETER FOR COMMON TABLE LIST
            var objGridParameter = new GridParameter
            {
                ConnectionString = this.connectionStrings.Value.MyApp,
                Where = Convert.ToString(param.Status, CultureInfo.CurrentCulture),
                Draw = param.Draw,
                PageNumber = param.Start,
                RecordPerPage = param.Length,
                SortOrder = param.Order[0].Dir,
                Type = param.Type,
                Columns = param.Columns,
                Orders = param.Order,
                Search = param.Search.Value
            };

            if (objGridParameter.Type == "News")
            {
                objGridParameter.Where = "isNews=1";
            }
            else if (objGridParameter.Type == "Announcement")
            {
                objGridParameter.Where = "isNews=0";
            }
            else if (objGridParameter.Type == "GetOpenOrder")
            {
                objGridParameter.Where = @"OD.[Status] = 1    
    AND OD.IsActive = 1
    AND OD.UserId = CASE WHEN @UserId IS NULL THEN OD.UserId ELSE @UserId END
    AND OD.PairId = CASE WHEN @PairId IS NULL THEN OD.PairId ELSE @PairId END";
            }

            // CALL COMMON METHOD
            var og = new GridData(objGridParameter);

            // RESPONSE
            return this.Ok(og.JsonData.Result);
        }

    }
}