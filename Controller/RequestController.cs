using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.Pagination;
using DLARS.Models.Requests;
using DLARS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        public async Task<IActionResult> AddNewRequest([FromBody] RequestCreateModel request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestService.AddRequestAsync(request);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Request submit successfully.")),
                Result.Failed => StatusCode(500, ApiResponse.FailMessage("Failed to submit request.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };
        }


        [Authorize(Roles = "Secretary,chairperson,Director")]
        [HttpGet("DisplayRequest")]
        public async Task<ActionResult<PagedResult<RequestReadModel>>> ReadRequest([FromQuery] int? statusId, [FromQuery] PaginationParams pagination, [FromQuery] string? filter)
        {
            if (!statusId.HasValue || statusId.Value <= 0)
            {
                return BadRequest("A valid statusId must be provided.");
            }

            var requestResult = await _requestService.GetRequestByStatusIdAsync(statusId.Value, pagination, filter);
            return Ok(requestResult);

        }


        [HttpPut("UpdateStatus")]
        public async Task<IActionResult> UpdateRequestStatus([FromBody] RequestUpdateModel requestUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestService.UpdateRequestStatusAsync(requestUpdate);

            return Ok(result);
        }


        [HttpPut("AddImageProof")]
        public async Task<IActionResult> AddImageProofInRequest([FromForm] AddImageReceivedInRequestModel imageRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _requestService.AddImageInRequestAsync(imageRequest);
            return Ok(result);
        }




    }
}
