using DLARS.Services;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DLARS.Models.UrlTokenModels;
using Microsoft.AspNetCore.Authorization;
using DLARS.Constants;

namespace DLARS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoleConstant.AllAdminRoles)]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService) 
        {
                _tokenService = tokenService;
        }


        /// <summary>
        /// Generates a secure tokenized URL for students to add supporting documents.
        /// </summary>
        /// <remarks>
        /// This endpoint is used to generate a short-lived secure token that can be sent to student email account ,
        /// enabling them to upload supporting documents for an existing request without authentication.
        /// </remarks>
        [HttpPost("generate-url-token")]
        public IActionResult GenerateNewToken([FromBody] RequestGenTokenModel request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var urlToken =  _tokenService.GenerateUrlToken(request);
            return Ok(new { urlToken });
        }


        /// <summary>
        /// Generates an invitation token for new user registration.
        /// </summary>
        /// <remarks>
        /// This endpoint is used by director to generate a secure invitation link,
        /// allowing invited users to register with a predefined role and email.
        /// </remarks>
        [Authorize(Roles = UserRoleConstant.Director)]
        [HttpPost("generate-invitation-token")]
        public IActionResult GenerateInvitationLink([FromBody] InvitationGenTokenModel invitation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var invitationToken = _tokenService.GenerateInvitationUrlToken(invitation);
            return Ok(new { invitationToken });
        }    

   
        
    }
}
