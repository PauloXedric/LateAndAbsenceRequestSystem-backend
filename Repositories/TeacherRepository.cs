using DLARS.Data;
using DLARS.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace DLARS.Repositories
{

    public interface ITeacherRepository
    {
        Task<int> AddNewTeacherAsync(TeacherEntity teacher);
        Task<int> GetTeacherIdAsync(string teacherCode);
    }



    public class TeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext _dbContext;

        public TeacherRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<int> AddNewTeacherAsync(TeacherEntity teacher)
        {

                _dbContext.Teacher.Add(teacher);
                await _dbContext.SaveChangesAsync();
                return teacher.TeacherId;
        }


        public async Task<int> GetTeacherIdAsync(string teacherCode)
        {
            var teacherId = await _dbContext.Teacher
                .Where(t => t.TeacherCode == teacherCode)
                .Select(t => t.TeacherId)
                .FirstOrDefaultAsync();

            return teacherId;
            
        }



    }
}
