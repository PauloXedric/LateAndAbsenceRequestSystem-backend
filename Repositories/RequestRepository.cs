using DLARS.Data;
using DLARS.Entities;
using DLARS.Enums;

namespace DLARS.Repositories
{

    public interface IRequestRepository : IBaseRepository<RequestEntity>
    {
        IQueryable<RequestEntity> GetRequestByStatusId(RequestStatus statusId, string? filter);
        Task<bool> UpdateRequestStatusAsync(RequestEntity request);
        Task<bool> AddImageInRequestAsync(RequestEntity request);
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

  
        public async Task<bool> UpdateRequestStatusAsync(RequestEntity request) 
        {
            var result = await _dbContext.Request.FindAsync(request.RequestId);
            if (result == null) return false;

            result.SetStatus(request.StatusId);
            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddImageInRequestAsync(RequestEntity request) 
        {
            var result = await _dbContext.Request.FindAsync(request.RequestId);
            if (result == null) return false;

            
            result.ProofImage = request.ProofImage;
            result.ParentValidImage = request.ParentValidImage;
            result.MedicalCertificate = request.MedicalCertificate;
            result.SetStatus(request.StatusId);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
