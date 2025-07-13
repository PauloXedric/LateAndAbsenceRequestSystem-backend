using DLARS.Data;
using DLARS.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DLARS.Repositories
{

    public interface ISubjectRepository : IBaseRepository<SubjectEntity>
    {
        Task<int> GetSubjectIdByCodeAsync(string subjectCode);
        Task<List<SubjectEntity>> GetAllSubjectAsync();
    }


    public class SubjectRepository : BaseRepository<SubjectEntity>, ISubjectRepository
    {

        private readonly AppDbContext _dbContext;

        public SubjectRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<int> GetSubjectIdByCodeAsync(string subjectCode)
        {
            var subjectId = await _dbContext.Subject
                       .Where(s => s.SubjectCode == subjectCode)
                       .Select(s => s.SubjectId)
                       .FirstOrDefaultAsync();

            return subjectId; 
        }


        public async Task<List<SubjectEntity>> GetAllSubjectAsync() 
        {
            return await _dbContext.Subject
                .OrderBy(s => s.SubjectName)
                .ToListAsync();
        }



    }
}
