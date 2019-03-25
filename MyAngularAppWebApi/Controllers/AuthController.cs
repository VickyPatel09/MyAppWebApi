using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyAngularAppWebApi.Model;
using MyAngularAppWebApi.Utility;

namespace MyAngularAppWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // GET api/values
        [HttpPost, Route("login")]
        public IActionResult Login([FromBody]LoginModel user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }

            if (user.UserName == "vicky" && user.Password == "1234")
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                issuer: "http://localhost:44301",
                audience: "http://localhost:44301",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Ok, "Login Sucessfully", new { Token = tokenString }));
               
            }
            else
            {
                return this.Ok(ApiResponse.SetResponse(ApiResponseStatus.Error, "You have entered wrong username or password", null));
            }
        }
    }
}