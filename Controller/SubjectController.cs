using DLARS.Enums;
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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;


        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        [HttpPost("Addsubject")]
        public async Task<IActionResult> AddNewSubject([FromBody] SubjectModel subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subjectService.CheckAndAddSubjectAsync(subject);

            return result switch
            {

                AddingSubjectTeacherResult.Success => Ok(new { message = "Subject added successfully." }),
                AddingSubjectTeacherResult.AlreadyExist => Conflict(new { message = "Subject already exists." }),
                _ => StatusCode(500, new { message = "Failed to add subject." })
            };

        }


    }
}
