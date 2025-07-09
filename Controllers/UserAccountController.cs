using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.UserAccountModels;
using DLARS.Services;
using Microsoft.AspNetCore.Mvc;


namespace DLARS.Controllers
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



        [HttpPost("register-user")]
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



        [HttpPost("login")]
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

                Result.Failed => StatusCode(403, ApiResponse.FailMessage("Wait for the Director's account activation.")),

                Result.DoesNotExist => NotFound(ApiResponse.FailMessage("Account does not exist.")),

                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected login error."))
            };
        }


        [HttpPost("request-reset-password")]
        public async Task<IActionResult> RequestResetPassword([FromBody] ResetPasswordRequestModel resetPassword)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var token = await _userAccountService.GenerateResetPasswordTokenAsync(resetPassword.Username);
            if (token == null)
                return NotFound(ApiResponse.FailMessage("No user found with that email."));

            return Ok(new { token, username = resetPassword.Username });
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userAccountService.ResetPasswordAsync(resetPassword.Email, resetPassword.Token, resetPassword.NewPassword);
            if (result.Succeeded)
                return Ok(ApiResponse.SuccessMessage("Password has been reset successfully."));

            return BadRequest(result.Errors);
        }


        [HttpGet("all")]
        public async Task<ActionResult<List<UserReadModel>>> GetAllUsers()
        {
            var users = await _userAccountService.GetAllUserWithRoleAsync();
            return Ok(users);
        }


        [HttpGet("check-user/{username}")]
        public async Task<ActionResult<bool>> CheckUserAsync([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username cannot be empty.");

            var userExist = await _userAccountService.CheckUserExistenceAsync(username);
            return Ok(userExist);   
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
