
using DLARS.Data;
using DLARS.Entities;
using DLARS.Views;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Repositories
{

    public interface ITeacherSubjectsRepository
    {
        Task AddTeacherandSubjectAsync(TeacherSubjectEntity teacherSubjects);
        Task<bool> GetSubjectAndTeacherByIdAsync(int teacherId, int subjectId);
        Task<List<TeacherAssignedSubjectsModelView>> GetAllAsync();
    }


    public class TeacherSubjectsRepository : ITeacherSubjectsRepository
    {
        private readonly AppDbContext _dbContext;

        public TeacherSubjectsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task AddTeacherandSubjectAsync(TeacherSubjectEntity teacherSubjects) 
        {
            _dbContext.TeacherSubject.Add(teacherSubjects);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<bool> GetSubjectAndTeacherByIdAsync(int teacherId, int subjectId)
        {
            return await  _dbContext.TeacherSubject
                .AnyAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
        }



        public async Task<List<TeacherAssignedSubjectsModelView>> GetAllAsync() 
        {
            return await _dbContext.TeacherAssignedSubjects.ToListAsync();
        }

    }
}
