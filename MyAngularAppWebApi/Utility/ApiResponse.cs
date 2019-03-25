using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyAngularAppWebApi.Utility
{
    public static class ApiResponse
    {
        public static object SetResponse(ApiResponseStatus responseStatus, string message, object data)
        {
            var response = new { ResponseStatus = responseStatus, Message = message, Data = data };
            return response;
        }
    }

    public enum ApiResponseStatus
    {
        Ok,

        Error,

        Warning
    }
}
