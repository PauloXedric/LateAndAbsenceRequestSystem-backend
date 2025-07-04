using DLARS.Services;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DLARS.Models.UrlTokenModels;

namespace DLARS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService) 
        {
                _tokenService = tokenService;
        }

       
        [HttpPost("GenerateUrlToken")]
        public IActionResult GenerateNewToken([FromBody] RequestGenTokenModel request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var urlToken =  _tokenService.GenerateUrlToken(request);
            return Ok(new { urlToken });
        }


        [HttpPost("GenerateInvitationLink")]
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
