using DLARS.Data;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Repositories
{

    public interface IRequestRepository : IBaseRepository<RequestEntity>
    {
        IQueryable<RequestEntity> GetRequestByStatusId(RequestStatus statusId, string? filter);
        Task<bool> UpdateRequestStatusAsync(int requestId, RequestStatus status);
        Task<bool> AddImageInRequestAsync(AddImageUploadInRequestModel request);
        Task<bool> SubjectExistsInRequestAsync(string studentNumber, string subjectCode);
        Task<bool> GetSubmittedStatusByidAsync(int requestId);
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

  
        public async Task<bool> UpdateRequestStatusAsync(int requestId, RequestStatus status) 
        {
            var result = await _dbContext.Request.FindAsync(requestId);
            if (result == null) return false;

            result.SetStatus(status);
            await _dbContext.SaveChangesAsync();

            return true;
        }


        public async Task<bool> AddImageInRequestAsync(AddImageUploadInRequestModel request) 
        {
            var result = await _dbContext.Request.FindAsync(request.RequestId);
            if (result == null) return false;

            
            result.ProofImage = request.ProofImage;
            result.ParentValidImage = request.ParentValidImage;
            result.MedicalCertificate = request.MedicalCertificate;
            result.SetStatus(request.StatusId);
            result.Submitted = request.Submitted;
            await _dbContext.SaveChangesAsync();

            return true;
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

    }
}
