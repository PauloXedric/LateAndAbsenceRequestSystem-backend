using DLARS.Models;
using DLARS.Models.Pagination;
using DLARS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {

        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }



        [HttpPost("AddRequest")]
        public async Task<IActionResult> AddRequest([FromBody] RequestCreateModel request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestService.AddRequestAsync(request);

            if (result <= 0)
            {
                return BadRequest("Error occured in adding a request");
            }

            return Ok(result);
        }


        [Authorize(Roles = "Secretary,chairperson,Director")]
        [HttpGet("DisplayRequest")]
        public async Task<IActionResult> GetRequests([FromQuery] int? statusId, [FromQuery] PaginationParams pagination, [FromQuery] string? filter)
        {
            if (!statusId.HasValue || statusId.Value <= 0)
            {
                return BadRequest("A valid statusId must be provided.");
            }

            var requestResult = await _requestService.GetRequestByStatusIdAsync(statusId.Value, pagination, filter);
            return Ok(requestResult);

        }


        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus([FromBody] RequestUpdateModel requestUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestService.UpdateRequestStatusAsync(requestUpdate);

            return Ok(result);
        }


        [HttpPut("AddImageProof")]
        public async Task<IActionResult> AddImageProof([FromBody] AddImageInRequestModel imageRequest) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestService.AddImageInRequestAsync(imageRequest);

            return Ok(result);
        }





    }
}
