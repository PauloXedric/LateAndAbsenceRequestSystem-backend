using DLARS.Data;
using DLARS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DLARS.Repositories
{

    public interface ISubjectRepository 
    {
        Task<int> AddNewSubjectAsync(SubjectsEntity subject);
        Task<int> GetSubjectIdByCodeAsync(string subjectCode);
        Task<List<SubjectsEntity>> GetAllSubjectAsync();
        Task<SubjectsEntity?> GetByIdAsync(int id);
        Task<bool> UpdateSubjectAsync(SubjectsEntity subject);
        Task<bool> DeleteSubjectAsync(int subjectId);
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


        public async Task<int> GetSubjectIdByCodeAsync(string subjectCode)
        {
            var subjectId = await _dbContext.Subject
                       .Where(s => s.SubjectCode == subjectCode)
                       .Select(s => s.SubjectId)
                       .FirstOrDefaultAsync();

            return subjectId; 
        }


        public async Task<List<SubjectsEntity>> GetAllSubjectAsync() 
        {
            return await _dbContext.Subject
                .OrderBy(s => s.SubjectName)
                .ToListAsync();
        }



        public async Task<SubjectsEntity?> GetByIdAsync(int id)
        {
            return await _dbContext.Subject.FindAsync(id);
        }


        public async Task<bool> UpdateSubjectAsync(SubjectsEntity subject)
        {
            _dbContext.Subject.Update(subject);
            return await _dbContext.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteSubjectAsync(int subjectId) 
        {
            var existingSubject = await GetByIdAsync(subjectId);

            if (existingSubject == null) 
            {
                return false; 
            }
            _dbContext.Subject.Remove(existingSubject);
            return await _dbContext.SaveChangesAsync() > 0;
        }

    }
}
