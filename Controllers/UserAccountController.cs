using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.UserAccountModels;
using DLARS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userAccountService.RegisterAccountAsync(userAccount);

            if (result.Succeeded) 
            {
                return Ok(ApiResponse.SuccessMessage("Sucessfully register, Please wait for account activation"));
            }

            return BadRequest("Error encounter.");
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLogin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (result, user) = await _userAccountService.LoginUserAccountAsync(userLogin);

            return result switch
            {
                Result.Success => Ok(new
                {
                    tokenString = await _tokenService.GenerateUserTokenAsync(user!)
                }),

                Result.Failed => StatusCode(403, ApiResponse.FailMessage("Wait for the Director's account activation")),

                Result.DoesNotExist => NotFound(ApiResponse.FailMessage("Incorrect username or password")),

                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected login error"))
            };
        }


        [HttpGet]
        public async Task<ActionResult<List<UserReadModel>>> GetAllUsers()
        {
            var users = await _userAccountService.GetAllUserWithRoleAsync();
            return Ok(users);
        }


        [HttpPut]
        public async Task<IActionResult> UserUpdate([FromBody] UserUpdateModel userUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userAccountService.UpdateUserRoleAndStatusAsync(userUpdate);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("User updated successfully.")),
                Result.DoesNotExist => NotFound(ApiResponse.FailMessage("User does not exist.")),
                Result.Failed => StatusCode(500, ApiResponse.FailMessage("Error in updating user.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };

        }

    }
}
