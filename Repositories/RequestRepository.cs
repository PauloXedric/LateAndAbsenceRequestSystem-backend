using DLARS.Data;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Repositories
{

    public interface IRequestRepository : IBaseRepository<RequestEntity>
    {
        IQueryable<RequestEntity> GetRequestByStatusId(RequestStatus statusId, string? filter);
        Task<bool> SubjectExistsInRequestAsync(string studentNumber, string subjectCode);
        Task<bool> GetSubmittedStatusByidAsync(int requestId);
        Task<int> GetPendingRequestsOlderThanAsync(RequestStatus status, TimeSpan duration);
    }


    public class RequestRepository : BaseRepository<RequestEntity>, IRequestRepository
    {

        private readonly AppDbContext _dbContext;

        public RequestRepository(AppDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

    
        public IQueryable<RequestEntity> GetRequestByStatusId(RequestStatus statusId, string? filter) 
        {
            var query = _dbContext.Request
                .Where(r => r.StatusId == statusId);

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(r => r.StudentNumber.Contains(filter));
            }

            return query.OrderBy(r => r.StatusId);
        }


        public async Task<bool> SubjectExistsInRequestAsync(string studentNumber, string subjectCode)
        {
            return await _dbContext.Request
                  .AnyAsync(r => r.StudentNumber == studentNumber && r.SubjectCode == subjectCode);
            
        }


        public async Task<bool> GetSubmittedStatusByidAsync(int requestId)
        {
            var isSubmitted =  await _dbContext.Request
                .Where(r => r.RequestId == requestId)
                .Select(r => r.Submitted)
                .FirstOrDefaultAsync();

            return isSubmitted;
        }


        public async Task<int> GetPendingRequestsOlderThanAsync(RequestStatus status, TimeSpan duration)
        {
            var threshold = TimeHelper.GetPhilippineTimeNow().Subtract(duration);

            return await _dbContext.Request
                .Where(r => r.StatusId == status && r.ModifiedOn <= threshold)
                .CountAsync();
        }



    }
}
