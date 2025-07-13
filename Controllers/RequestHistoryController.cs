using DLARS.Constants;
using DLARS.Models.Pagination;
using DLARS.Models.RequestHistory;
using DLARS.Services;
using DLARS.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DLARS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoleConstant.AllAdminRoles)]
    public class RequestHistoryController : ControllerBase
    {
        private readonly IRequestHistoryService _requestHistoryService;

        public RequestHistoryController (IRequestHistoryService requestHistoryService)
        {
            _requestHistoryService = requestHistoryService;
        }


        /// <summary>
        /// Adds a new record to the request history.
        /// </summary>
        /// <remarks>
        /// The user ID who made the request action is automatically set based on the current authenticated user.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> AddRequestHistory([FromBody] RequestHistoryCreateModel history)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found.");
            }

            history.PerformedByUserId = userId;

            var result = await _requestHistoryService.AddRequestHistoryAsync(history);

            if (result == false)
            {
                return StatusCode(500, "Failed to add request history.");
            }

            return Ok(result);           
        }


        /// <summary>
        /// Retrieves a paginated list of all request history records.
        /// </summary>
        /// <remarks>
        /// Accessible to Chairperson and Director. Used to track the actions taken on student requests.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<PagedResult<RequestResultHistoryModelView>>> GetAllRequestHistory([FromQuery] PaginationParams pagination,
                                                                         [FromQuery] string? dateFilter, [FromQuery] string? studentNumberFilter )
        {
            var historyList = await _requestHistoryService.GetAllListAsync(pagination, dateFilter, studentNumberFilter);
            return Ok(historyList);
        }



    }
}
