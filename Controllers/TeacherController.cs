using DLARS.Constants;
using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.TeacherModels;
using DLARS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoleConstant.ChairpersonAndDirector)]
    public class TeacherController : ControllerBase
    {

        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService) 
        {
            _teacherService = teacherService;
        }


        /// <summary>
        /// Adds a new teacher to the system.
        /// </summary>
        /// <remarks>
        /// Only Chairperson and Director roles are allowed to add new teachers.
        /// Checks for duplicates before saving.
        /// </remarks>
        [HttpPost]
        public async Task<IActionResult> AddNewTeacher([FromBody] TeacherCreateModel teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherService.CheckAndAddTeacherAsync(teacher);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Instructor added successfully.")),
                Result.AlreadyExist => Conflict(ApiResponse.FailMessage("Instructor already exists.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };
        }


        /// <summary>
        /// Retrieves all registered teachers.
        /// </summary>
        /// <remarks>
        ///  Returns a list of all teachers together with their code.
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult<List<TeacherReadModel>>> GetAllTeachers()
        {
            var teacherList = await _teacherService.GetAllTeacherAsync();
            return Ok(teacherList);
        }


        /// <summary>
        /// Updates an existing teacher's information.
        /// </summary>
        /// <remarks>
        /// Only accessible to Chairperson and Director. Fails if the teacher does not exist.
        /// </remarks>
        [HttpPut]
        public async Task<IActionResult> UpdateTeacher([FromBody] TeacherUpdateModel updateTeacher) 
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);  
            }

            var result = await _teacherService.CheckAndUpdateTeacherAsync(updateTeacher);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Teacher updated successfully.")),
                Result.DoesNotExist => NotFound(ApiResponse.FailMessage("Teacher does not exists.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };
            
        }


        /// <summary>
        /// Deletes a teacher by ID.
        /// </summary>
        /// <remarks>
        /// Validates the teacher ID before deletion.
        /// </remarks>
        [HttpDelete("{teacherId}")]
        public async Task<IActionResult> DeleteTeacher([FromRoute] int teacherId)
        {
            if (teacherId <= 0)
            { 
                return BadRequest("Invalid teacher id"); 
            }

            var result = await _teacherService.DeleteTeacherAsync(teacherId);

            return result switch
            {
                Result.Success => Ok(ApiResponse.SuccessMessage("Teacher has been deleted successfully.")),
                Result.Failed => StatusCode(500, ApiResponse.FailMessage("Failed to delete teacher due to a system error.")),
                _ => StatusCode(500, ApiResponse.FailMessage("Unexpected result."))
            };
        }



    }
}
