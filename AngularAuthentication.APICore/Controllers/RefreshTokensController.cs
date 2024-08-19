using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AngularJSAuthentication.API.Controllers
{
    [ApiController]
    [Route("api/RefreshTokens")]
    public class RefreshTokensController : ControllerBase
    {

        private AuthRepository _repo = null;

        public RefreshTokensController()
        {
            _repo = new AuthRepository();
        }

        [Authorize]
        [HttpGet]
        [Route("~/api/RefreshTokens/")]
        public IActionResult Get()
        {
            return Ok(_repo.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [HttpDelete]
        [Route("~/api/RefreshTokens/")]
        public async Task<IActionResult> Delete(string tokenId)
        {
            var result = await _repo.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");

        }
    }
}
