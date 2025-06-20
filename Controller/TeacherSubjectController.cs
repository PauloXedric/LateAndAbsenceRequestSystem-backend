using DLARS.Enums;
using DLARS.Models;
using DLARS.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DLARS.Controller
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



        [HttpPost("AssignSubject")]
        public async Task<IActionResult> AssignSubjectToTeacher([FromBody] TeacherSubjectsCodeModel teacherSubjects)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _teacherSubjectsService.RegisterSubjectsToTeacher(teacherSubjects);

            return result switch
            {
                AddingSubjectTeacherResult.Success => Ok("User registered successfully."),
                AddingSubjectTeacherResult.DoesNotExist => NotFound("User does not exist."),
                AddingSubjectTeacherResult.AlreadyExist => Conflict("User already exists."),
                _ => StatusCode(500, "Unknown error")
            };

        }

    }
}
