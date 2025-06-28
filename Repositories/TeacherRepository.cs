using DLARS.Data;
using DLARS.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Threading.Tasks;

namespace DLARS.Repositories
{

    public interface ITeacherRepository : IBaseRepository<TeacherEntity>
    {
        Task<int> GetTeacherIdByCodeAsync(string teacherCode);
        Task<List<TeacherEntity>> GetAllTeacherAsync(); 
    }



    public class TeacherRepository : BaseRepository<TeacherEntity> , ITeacherRepository
    {
        private readonly AppDbContext _dbContext;

        public TeacherRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
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


    }
}
