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
        public async Task<IActionResult> GenerateNewToken([FromBody] RequestGenTokenModel request) 
        {
            var urlToken = _tokenService.GenerateUrlToken(request);
            return Ok(new { urlToken });
        }


        [HttpPost("GenerateInvitationLink")]
        public async Task<IActionResult> GenerateInvitationLink([FromBody] InvitationGenTokenModel invitation)
        {
            var invitationToken = _tokenService.GenerateInvitationUrlToken(invitation);
            return Ok(new { invitationToken });
        }


    }
}
