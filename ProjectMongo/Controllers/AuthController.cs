using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectMongo.Application.Business;
using ProjectMongo.Application.VOs;

namespace ProjectMongo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private ILoginBusiness _loginBusiness;

        public AuthController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> Signin([FromBody] LoginVO user)
        {
            if (user == null)
                return BadRequest("Invalid client request");

            var token =  await _loginBusiness.ValidateCredentials(user);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenVO tokenVo)
        {
            if (tokenVo == null)
                return BadRequest("Invalid client request");

            var token = await _loginBusiness.ValidateCredentials(tokenVo);

            if (token == null)
                return BadRequest("Invalid client request");

            return Ok(token);
        }

        [HttpGet]
        [Route("revoke")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Revoke([FromBody] TokenVO tokenVo)
        {
            var username = User.Identity.Name;
            var result = await _loginBusiness.RevokeToken(username);

            if (result == false)
                return BadRequest("Invalid client request");
            

            return NoContent();
        }
    }
}
