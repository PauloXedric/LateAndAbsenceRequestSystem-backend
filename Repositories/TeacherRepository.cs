using DLARS.Data;
using DLARS.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace DLARS.Repositories
{

    public interface ITeacherRepository
    {
        Task<int> AddNewTeacherAsync(TeacherEntity teacher);
        Task<int> GetTeacherIdByCodeAsync(string teacherCode);
        Task<List<TeacherEntity>> GetAllTeacherAsync();
        Task<TeacherEntity?> GetByIdAsync(int id);
        Task<bool> UpdateTeacherAsync(TeacherEntity teacher);
        Task<bool> DeleteTeacherAsync(int teacherId);
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


        public async Task<int> GetTeacherIdByCodeAsync(string teacherCode)
        {
            var teacherId = await _dbContext.Teacher
                .Where(t => t.TeacherCode == teacherCode)
                .Select(t => t.TeacherId)
                .FirstOrDefaultAsync();

            return teacherId;
            
        }


        public async Task<List<TeacherEntity>> GetAllTeacherAsync() 
        {
            return await _dbContext.Teacher
                .OrderBy(t => t.TeacherName)
                .ToListAsync();
        }


        public async Task<TeacherEntity?> GetByIdAsync(int id)
        {
            return await _dbContext.Teacher.FindAsync(id);
        }


        public async Task<bool> UpdateTeacherAsync(TeacherEntity teacher)
        {
            _dbContext.Teacher.Update(teacher);
            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteTeacherAsync(int teacherId)
        {
            var existingTeacher = await GetByIdAsync(teacherId);
            if (existingTeacher == null) 
            { 
                return false;
            }

            _dbContext.Teacher.Remove(existingTeacher);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
