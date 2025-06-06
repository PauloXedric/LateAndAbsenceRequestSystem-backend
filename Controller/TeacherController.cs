using DLARS.Models;
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
        public async Task<IActionResult> CreateNewTeacher([FromBody] TeacherModel teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherService.CheckAndAddTeacherAsync(teacher);

            return Ok(result);

        }


    }
}
