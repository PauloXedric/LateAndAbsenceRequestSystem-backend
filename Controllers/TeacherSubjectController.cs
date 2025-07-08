using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.TeacherSubjectModels;
using DLARS.Services;
using DLARS.Views;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherSubjectController : ControllerBase
    {

        private readonly ITeacherSubjectsService _teacherSubjectsService;

        public TeacherSubjectController(ITeacherSubjectsService teacherSubjectsService)
        {
            _teacherSubjectsService = teacherSubjectsService;
        }



        [HttpPost]
        public async Task<IActionResult> AssignSubjectsToTeacher([FromBody] TeacherSubjectsCodeModel teacherSubjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherSubjectsService.RegisterSubjectsToTeacher(teacherSubjects);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Subject assigned successfully.")),
                Result.Updated => Ok(ApiResponse.SuccessMessage("The assigned subjects were updated successfully.")),
                Result.DoesNotExist => NotFound(ApiResponse.FailMessage("Instructor/Subject does not exist.")),
                Result.AlreadyExist => Conflict(ApiResponse.FailMessage("Subject Already assigned to the instructor.")),
                _ => StatusCode(500, "Unknown error")
            };
        }


        [HttpGet]
        public async Task<ActionResult<List<TeacherAssignedSubjectsModelView>>> GetAllTeacherAssignedSubjects()
        {
            var assignedList = await _teacherSubjectsService.GetAllListAsync();
            return Ok(assignedList);
        }


        [HttpDelete("{teacherId}")]
        public async Task<IActionResult> DeleteTeacherWithSubjects([FromRoute] int teacherId)
        {
            if (teacherId <= 0)
            {
                return BadRequest("Invalid Teacher Id");
            }

            var result = await _teacherSubjectsService.DeleteTeacherWithSubjectsAssignedAsync(teacherId);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Teacher and their subjects were deleted successfully.")),
                Result.Failed => StatusCode(500, ApiResponse.FailMessage("Failed to delete the teacher and their subjects.")),
                _ => StatusCode(500, "An unknown error occurred.")
            };
        }
        


    }
}
