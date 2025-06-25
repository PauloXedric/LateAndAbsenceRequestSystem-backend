using DLARS.Helpers;
using DLARS.Models.UserAccountModels;
using DLARS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {

        private readonly IUserAccountService _userAccountService;
        private readonly ITokenService _tokenService;


        public UserAccountController(IUserAccountService userAccountService, ITokenService tokenService)
        {
            _userAccountService = userAccountService;
            _tokenService = tokenService;
        }



        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisterModel userAccount)
        {
            var result = await _userAccountService.RegisterAccountAsync(userAccount);

            if (result.Succeeded) 
            {
                return Ok("Sucessfully register");
            }

            return BadRequest("Error encounter.");
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLogin)
        {
            var user = await _userAccountService.LoginUserAccountAsync(userLogin);

            if(user == null) 
            {
                return Unauthorized(ApiResponse.FailMessage("Incorrect username or password"));
            }

            var tokenString = await _tokenService.GenerateUserTokenAsync(user);

            return Ok(new { tokenString });
        }



    }
}
