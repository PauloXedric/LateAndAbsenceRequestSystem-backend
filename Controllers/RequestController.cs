using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.Pagination;
using DLARS.Models.Requests;
using DLARS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controllers
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



        [HttpPost]
        public async Task<IActionResult> AddNewRequest([FromBody] RequestCreateModel request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestService.AddRequestAsync(request);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Request submitted successfully.")),
                Result.AlreadyExist=> Conflict(ApiResponse.FailMessage("Request already submit, please wait for approval.")),
                Result.Failed => StatusCode(500, ApiResponse.FailMessage("Failed to submit request.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };
        }


        //[Authorize(Roles = "Secretary,chairperson,Director")]
        [HttpGet("All")]
        public async Task<ActionResult<PagedResult<RequestReadModel>>> GetAllRequests([FromQuery] RequestStatus? statusId, [FromQuery] PaginationParams pagination, [FromQuery] string? filter)
        {
            if (!statusId.HasValue)
            {
                return BadRequest("A valid statusId must be provided.");
            }

            var requestResult = await _requestService.GetRequestByStatusIdAsync(statusId.Value, pagination, filter);
            return Ok(requestResult);
        }


        [HttpGet("{requestId}")]
        public async Task<ActionResult<bool>> GetSubmittedStatus([FromRoute] int requestId)
        {
            if (requestId <= 0)
            {
                return BadRequest("Invalid requestId.");
            }

            var result = await _requestService.CheckRequestSubmittedStatusAsync(requestId);
            return Ok(result);
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
