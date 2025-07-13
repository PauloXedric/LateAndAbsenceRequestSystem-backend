using DLARS.Data;
using DLARS.Entities;
using DLARS.Views;

namespace DLARS.Repositories
{

    public interface IRequestHistoryRepository : IBaseRepository<RequestHistoryEntity> 
    {
        IQueryable<RequestResultHistoryModelView> GetViewAsync(string? dateFilter, string? studentNumberFilter);
    }


    public class RequestHistoryRepository : BaseRepository<RequestHistoryEntity>, IRequestHistoryRepository
    {
        private readonly AppDbContext _dbContext;

        public RequestHistoryRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public  IQueryable<RequestResultHistoryModelView> GetViewAsync(string? dateFilter, string? studentNumberFilter)
        {

            var query = _dbContext.RequestResultHistory.AsQueryable();

            if (!string.IsNullOrWhiteSpace(dateFilter) &&
                DateTime.TryParse(dateFilter, out var parsedDate))
            {
                query = query.Where(r => r.ActionDate.Date == parsedDate.Date);
            }

            if (!string.IsNullOrWhiteSpace(studentNumberFilter))
            {
                query = query.Where(r => r.StudentNumber.Contains(studentNumberFilter));
            }

            return query.OrderByDescending(r => r.ActionDate);
        }


      
    }
}
