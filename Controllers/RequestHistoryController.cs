using DLARS.Models.Pagination;
using DLARS.Models.RequestHistory;
using DLARS.Services;
using DLARS.Views;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DLARS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class RequestHistoryController : ControllerBase
    {
        private readonly IRequestHistoryService _requestHistoryService;

        public RequestHistoryController (IRequestHistoryService requestHistoryService)
        {
            _requestHistoryService = requestHistoryService;
        }



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


        [HttpGet]
        public async Task<ActionResult<PagedResult<RequestResultHistoryModelView>>> GetAllRequestHistory([FromQuery] PaginationParams pagination,
                                                                         [FromQuery] string? dateFilter, [FromQuery] string? studentNumberFilter )
        {
            var historyList = await _requestHistoryService.GetAllListAsync(pagination, dateFilter, studentNumberFilter);
            return Ok(historyList);
        }
    }
}
