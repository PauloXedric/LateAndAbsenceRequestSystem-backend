
using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.SubjectModel;
using DLARS.Models.SubjectModels;
using DLARS.Services;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controller
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "chairperson,Director")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;


        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        [HttpPost("Addsubject")]
        public async Task<IActionResult> AddNewSubject([FromBody] SubjectCreateModel subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subjectService.CheckAndAddSubjectAsync(subject);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Subject added successfully.")),
                Result.AlreadyExist => Conflict(ApiResponse.FailMessage("Subject already exists.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };

        }


        [HttpGet("SubjectList")]
        public async Task<ActionResult<List<SubjectReadModel>>> SubjectList()
        {
            var subjectList = await _subjectService.GetAllSubjectAsync();
            return Ok(subjectList);
        }



        [HttpPut("UpdateSubject")]
        public async Task<IActionResult> UpdateSubject([FromBody] SubjectUpdateModel updatemodel) 
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState); 
            }

            var updated = await _subjectService.CheckAndUpdateSubjectAsync(updatemodel);

            return updated switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Subject updated successfully.")),
                Result.DoesNotExist => NotFound(ApiResponse.FailMessage("Subject does not exists.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };

        }

    }
}
