
using DLARS.Constants;
using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.SubjectModel;
using DLARS.Models.SubjectModels;
using DLARS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoleConstant.ChairpersonAndDirector)]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        /// <summary>
        /// Adds a new subject to the system.
        /// </summary>
        /// <remarks>
        /// Only accessible to users with Chairperson or Director roles. Checks for duplicate before saving.
        /// </remarks>
        [HttpPost]
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


        /// <summary>
        /// Retrieves a list of all registered subjects.
        /// </summary>
        /// <remarks>
        /// Returns all subjects in the system together with its code.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<List<SubjectReadModel>>> GetAllSubjects()
        {
            var subjectList = await _subjectService.GetAllSubjectAsync();
            return Ok(subjectList);
        }


        /// <summary>
        /// Updates the details of an existing subject.
        /// </summary>
        /// <remarks>
        /// Checks if the subject exists before updating.
        /// </remarks>
        [HttpPut]
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


        /// <summary>
        /// Deletes a subject by its ID.
        /// </summary>
        /// <remarks>
        /// Validates the subject ID before attempting deletion.
        /// </remarks>
        [HttpDelete("{subjectId}")]
        public async Task<IActionResult> DeleteSubject([FromRoute] int subjectId)
        {
            if (subjectId <= 0)
            {
                return BadRequest("Invalid subject id");
            }
            var result = await _subjectService.DeleteSubjectAsync(subjectId);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Subject has been deleted successfully.")),
                Result.Failed => StatusCode(500, ApiResponse.FailMessage("Failed to delete subject due to a system error.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };
        }
        


    }
}
