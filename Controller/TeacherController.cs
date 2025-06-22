using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.TeacherModels;
using DLARS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controller
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "chairperson,Director")]
    [ApiController]
    public class TeacherController : ControllerBase
    {

        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService) 
        {
            _teacherService = teacherService;
        }

        
        [HttpPost("AddTeacher")]
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



        [HttpGet("TeacherList")]
        public async Task<ActionResult<List<TeacherReadModel>>> TeacherList()
        {
            var teacherList = await _teacherService.GetAllTeacherAsync();
            return Ok(teacherList);
        }

    }
}
