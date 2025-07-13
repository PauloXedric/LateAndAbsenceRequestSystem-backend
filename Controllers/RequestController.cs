using DLARS.Constants;
using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.Pagination;
using DLARS.Models.Requests;
using DLARS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoleConstant.AllAdminRoles)]
    public class RequestController : ControllerBase
    {

        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;         
        }


        /// <summary>
        /// Submits a new request to the system.
        /// </summary>
        /// <remarks>
        /// This endpoint is accessible to unauthenticated users. Used by students or external users to submit requests for processing.
        /// </remarks>
        [AllowAnonymous]
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


        /// <summary>
        /// Retrieves all requests filtered by status.
        /// </summary>
        /// <remarks>
        /// This endpoint is restricted to authorized admin roles. Use query parameters to filter by status,
        /// paginate results, and apply optional search filtering.
        /// </remarks>
        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<RequestReadModel>>> GetAllRequests([FromQuery] RequestStatus? statusId, [FromQuery] PaginationParams pagination, [FromQuery] string? filter)
        {
            if (!statusId.HasValue)
            {
                return BadRequest("A valid statusId must be provided.");
            }

            var requestResult = await _requestService.GetRequestByStatusIdAsync(statusId.Value, pagination, filter);
            return Ok(requestResult);
        }


        /// <summary>
        /// Checks if the request ID has already submitted image proof for a specific request.
        /// </summary>
        /// <remarks>
        /// Used by the jwt token to determine whether to show the upload form or an invalid message
        /// </remarks>
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


        /// <summary>
        /// Updates the approval status of a request.
        /// </summary>
        /// <remarks>
        /// Part of the core request workflow. It advances the request to the next
        /// approval stage (Secretary → Chairperson → Director), allowing the system to track
        /// where in the process the request currently is.
        /// </remarks>
        [HttpPatch("update-status")]
        public async Task<IActionResult> UpdateRequestStatus([FromBody] RequestUpdateModel requestUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _requestService.UpdateRequestStatusAsync(requestUpdate);

            return Ok(result);
        }


        /// <summary>
        /// Uploads image proof to an existing request.
        /// </summary>
        /// <remarks>
        /// Accessible to the specific user who was approved during the initial request. It allows students to 
        /// attach supporting image files after receiving a secure upload link.
        /// </remarks>
        [AllowAnonymous]
        [HttpPatch("add-image-proof")]
        public async Task<IActionResult> AddImageProofInRequest([FromForm] AddImageReceivedInRequestModel imageRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _requestService.AddImageInRequestAsync(imageRequest);
            return Ok(result);
        }



    }
}
