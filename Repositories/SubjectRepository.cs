using DLARS.Data;
using DLARS.Entities;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Repositories
{

    public interface ISubjectRepository 
    {
        Task<int> AddNewSubjectAsync(SubjectsEntity subject);
        Task<int> GetSubjectIdAsync(string subjectCode);
    }


    public class SubjectRepository : ISubjectRepository
    {

        private readonly AppDbContext _dbContext;

        public SubjectRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<int> AddNewSubjectAsync(SubjectsEntity subject)
        {

            _dbContext.Subject.Add(subject);
            await _dbContext.SaveChangesAsync();
            return subject.SubjectId;
        }


        public async Task<int> GetSubjectIdAsync(string subjectCode)
        {
            var subjectId = await _dbContext.Subject
                       .Where(s => s.SubjectCode == subjectCode)
                       .Select(s => s.SubjectId)
                       .FirstOrDefaultAsync();

            return subjectId; 
        }

    }
}
