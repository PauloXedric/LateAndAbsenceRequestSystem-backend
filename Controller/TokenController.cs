using DLARS.Services;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DLARS.Models.Requests;

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
            var token = _tokenService.GenerateUrlToken(request);
            return Ok(new { token });
        }


    }
}
