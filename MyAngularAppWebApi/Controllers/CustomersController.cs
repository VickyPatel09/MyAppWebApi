using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAngularAppCore.Entities;

namespace MyAngularAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        // GET api/values
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "Vicky", "Patel" };
        }

        [HttpGet, Route("GetMembers")]
        public List<Users> GetMembers()
        {

            using (var context = new MyAngularAppContext())
            {
                var Members=context.Users.ToList();
                return Members;
            }
        }
    }
}