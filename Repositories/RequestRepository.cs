using DLARS.Data;
using DLARS.Entities;
using DLARS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace DLARS.Repositories
{

    public interface IRequestRepository 
    {
        Task<int> AddRequestAsync(RequestEntity request);
        IQueryable<RequestEntity> GetRequestByStatusId(int statusId, string? filter);
        Task<bool> UpdateRequestStatusAsync(RequestEntity request);
        Task<bool> AddImageInRequestAsync(RequestEntity request);

    }


    public class RequestRepository : IRequestRepository
    {

        private readonly AppDbContext _dbContext;

        public RequestRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }


        public async Task<int> AddRequestAsync(RequestEntity request) 
        {
                _dbContext.Request.Add(request);
                await _dbContext.SaveChangesAsync();
                return request.RequestId;
        }
        

        public IQueryable<RequestEntity> GetRequestByStatusId(int statusId, string? filter) 
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

            result.StatusId = request.StatusId;
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
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
