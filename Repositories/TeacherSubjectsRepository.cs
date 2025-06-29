
using DLARS.Data;
using DLARS.Entities;
using DLARS.Views;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Repositories
{

    public interface ITeacherSubjectsRepository : IBaseRepository<TeacherSubjectEntity>
    {
        Task<List<TeacherAssignedSubjectsModelView>> GetViewAsync();
        Task<bool> GetSubjectAndTeacherByIdAsync(int teacherId, int subjectId);
        Task<bool>DeleteAllSubjectsByTeacherIdAsync(int teacherId);
        Task DeleteAllTeacherBySubjectIdAsync(int subjectId);
    }


    public class TeacherSubjectsRepository : BaseRepository<TeacherSubjectEntity>, ITeacherSubjectsRepository
    {
        private readonly AppDbContext _dbContext;

        public TeacherSubjectsRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<List<TeacherAssignedSubjectsModelView>> GetViewAsync()
        {
            return await _dbContext.TeacherAssignedSubjects.ToListAsync();
        }

        public async Task<bool> GetSubjectAndTeacherByIdAsync(int teacherId, int subjectId)
        {
            return await  _dbContext.TeacherSubject
                .AnyAsync(ts => ts.TeacherId == teacherId && ts.SubjectId == subjectId);
        }


        public async Task<bool> DeleteAllSubjectsByTeacherIdAsync(int teacherId)
        {
            var existing = _dbContext.TeacherSubject.Where(ts => ts.TeacherId == teacherId);
            _dbContext.TeacherSubject.RemoveRange(existing);
            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task DeleteAllTeacherBySubjectIdAsync(int subjectId)
        {
            var existing = _dbContext.TeacherSubject.Where(ts => ts.SubjectId == subjectId);
            _dbContext.TeacherSubject.RemoveRange(existing);
            await _dbContext.SaveChangesAsync();
        }

    }
}
