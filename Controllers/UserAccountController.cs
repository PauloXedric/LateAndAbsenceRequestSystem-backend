using DLARS.Constants;
using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.UserAccountModels;
using DLARS.Services;
using Microsoft.AspNetCore.Authorization;
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


        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <remarks>
        /// Used for registering users through a director-sent invitation link send via email.
        /// The account will remain inactive until activate by the Director.
        /// </remarks>
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterModel userAccount)
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


        /// <summary>
        /// Authenticates a user and generates a JWT token.
        /// </summary>
        /// <remarks>
        /// Only admins / invited through the director-sent invitation link can log-in.
        /// </remarks>
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


        /// <summary>
        /// Generates a password reset token for the admins.
        /// </summary>
        /// <remarks>
        /// Returns a secure token that can be used to reset the password. The token is time-limited.
        /// </remarks>
        [HttpPost("request-reset-password")]
        public async Task<IActionResult> RequestResetPassword([FromBody] ResetPasswordRequestModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var token = await _userAccountService.GenerateResetPasswordTokenAsync(resetPassword.Username);
            if (token == null)
                return NotFound(ApiResponse.FailMessage("No user found with that email."));

            return Ok(new { token, username = resetPassword.Username });
        }


        /// <summary>
        /// Validates the provided password reset token by the identity core.
        /// </summary>
        /// <remarks>
        /// Ensures the token is still valid and associated with the provided username.
        /// </remarks>
        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ResetTokenValidationModel resetToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool valid = await _userAccountService.ValidateResetTokenAsync(resetToken);

            return Ok(valid);
        }


        /// <summary>
        /// Resets the user's password using a valid reset token.
        /// </summary>
        /// <remarks>
        /// Updates the user's password if the token is valid and not expired.
        /// </remarks>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool success = await _userAccountService.ResetPasswordAsync(resetPassword.Email, resetPassword.Token, resetPassword.NewPassword);
            if (success)
                return Ok(ApiResponse.SuccessMessage("Password has been reset successfully."));

            return BadRequest(ApiResponse.FailMessage("Password reset failed."));
        }


        /// <summary>
        /// Retrieves all registered users and their roles.
        /// </summary>
        /// <remarks>
        /// Used by the director to manage admins in the system.
        /// </remarks>
        [Authorize(Roles = UserRoleConstant.Director)]
        [HttpGet("all")]
        public async Task<ActionResult<List<UserReadModel>>> GetAllUsers()
        {
            var users = await _userAccountService.GetAllUserWithRoleAsync();
            return Ok(users);
        }


        /// <summary>
        /// Checks if a user exists by their username or email.
        /// </summary>
        /// <remarks>
        /// Used in this system to check whether the account already exist or not.
        /// </remarks>
        [HttpGet("check-user/{username}")]
        public async Task<ActionResult<bool>> CheckUserAsync([FromRoute] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username cannot be empty.");

            var userExist = await _userAccountService.CheckUserExistenceAsync(username);
            return Ok(userExist);
        }


        /// <summary>
        /// Updates a user's role and account status.
        /// </summary>
        /// <remarks>
        /// Can be used to activate or deactivate a user, and assign roles like Secretary, Chairperson, or Director.
        /// </remarks>
        [Authorize(Roles = UserRoleConstant.Director)]
        [HttpPut]
        public async Task<IActionResult> UpdateUserStatusAndRole([FromBody] UserUpdateModel userUpdate)
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


        /// <summary>
        /// Permanently deletes a user account by email.
        /// </summary>
        /// <remarks>
        /// Upon deletion. this removes all traces of the user from the system.
        /// </remarks>
        [Authorize(Roles = UserRoleConstant.Director)]
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string email)
        {
            if (string.IsNullOrWhiteSpace(email)) 
            { 
                return BadRequest("Email must be provided."); 
            }

            var result = await _userAccountService.DeleteAccountByEmailAsync(email);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("User deleted permanently.")),         
                Result.Failed => StatusCode(500, ApiResponse.FailMessage("Failed to delete user account.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };
        }

    }
}
