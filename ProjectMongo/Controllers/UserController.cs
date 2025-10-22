using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectMongo.Application.Business;
using ProjectMongo.Application.VOs;
using ProjectMongo.Domain.Entities;

namespace ProjectMongo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("Bearer")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private IUserBusiness _userBusiness;


        public UserController(ILogger<UserController> logger, IUserBusiness userBusiness)
        {
            _logger = logger;
            _userBusiness = userBusiness;
        }

        #region GET

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userBusiness.FindAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userBusiness.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //[HttpGet("findUserByName")]
        //public IActionResult Get([FromQuery] string? firstName, string? lastName)
        //{
        //    var User = _userBusiness.FindByName(firstName, lastName);

        //    if (User == null)
        //        return NotFound();

        //    return Ok(User);
        //}
        #endregion

        #region POST
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserVO user)
        {

            if (user == null)
                return BadRequest();

            var resultVO = await _userBusiness.CreateAsync(user);

            return Ok(resultVO);
        }
        #endregion

        #region PUT
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserVO user)
        {

            if (user == null)
                return BadRequest();

            var result = await _userBusiness.UpdateAsync(user);

            return Ok(result);
        }
        #endregion

        #region DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userBusiness.DeleteAsync(id);

            return NoContent();
        }
        #endregion
    }
}
