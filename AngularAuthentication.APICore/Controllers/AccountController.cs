using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using AngularAuthentication.APICore.Models;
using AngularJSAuthentication.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Text;

namespace AngularJSAuthentication.API.Controllers
{
    [ApiController]
    [Route("api/Account")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Health")]
        public string Health()
        {
            return "I'm good, how are you?";
        }

        [HttpPost("login")]
        public ActionResult<object> Authenticate([FromBody] UserModel user)
        {
            var loginResponse = new LoginResponse { };
            UserModel loginrequest = new()
            {
                UserName = user.UserName.ToLower(),
                Password = user.Password
            };

            bool isUsernamePasswordValid = false;

            if (user != null)
            {
                isUsernamePasswordValid = loginrequest.Password == "admin" ? true : false;
            }
            if (isUsernamePasswordValid)
            {
                string token = CreateToken(loginrequest.UserName);

                loginResponse.Token = token;
                loginResponse.responseMsg = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK
                };

                return Ok(new { loginResponse });
            }
            else
            {
                return BadRequest("Username or Password Invalid!");
            }
        }

        private string CreateToken(string username)
        {

            List<Claim> claims = new()
            {                    
                //list of Claims - we only checking username - more claims can be added.
                new Claim("username", Convert.ToString(username)),
            };
              
            var cred = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: cred
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        // POST api/Account/Register
        [AllowAnonymous]
        [HttpPost]
        [Route("~/api/Account/Register")]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userModel.Password =  userModel.Password;

            // when db connection added
            //_dbContext.Users.Add(userModel);
            //await _dbContext.SaveChangesAsync();

            return Ok(userModel);

        }
    }
}
