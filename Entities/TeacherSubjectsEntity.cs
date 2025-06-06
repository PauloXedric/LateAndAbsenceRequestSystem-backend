using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DLARS.Entities
{
    [PrimaryKey(nameof(TeacherId), nameof(SubjectId))]
    public class TeacherSubjectsEntity
    {

        public int TeacherId { get; set; }

        public int SubjectId { get; set; }
    }
}
