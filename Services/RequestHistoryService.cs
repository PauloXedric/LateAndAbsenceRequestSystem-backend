using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Helpers;
using DLARS.Models.Pagination;
using DLARS.Models.RequestHistory;
using DLARS.Models.Requests;
using DLARS.Repositories;
using DLARS.Views;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Services
{
    public interface IRequestHistoryService
    {
        Task<bool> AddRequestHistoryAsync(RequestHistoryCreateModel history);
        Task<PagedResult<RequestResultHistoryModelView>> GetAllListAsync(PaginationParams pagination,
                                                     string? dateFilter, string? studentNumberFilter);
    }

    public class RequestHistoryService : IRequestHistoryService
    {
        private readonly IRequestHistoryRepository _requestHistoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RequestHistoryService> _logger;

        public RequestHistoryService(IRequestHistoryRepository requestHistoryRepository, IMapper mapper,
                                    ILogger<RequestHistoryService> logger)
        {
            _requestHistoryRepository = requestHistoryRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<bool> AddRequestHistoryAsync(RequestHistoryCreateModel history) 
        {
            try
            {
                var mapped = _mapper.Map<RequestHistoryEntity>(history);

                mapped.ActionDate = TimeHelper.GetPhilippineTimeNow();

                var result = await _requestHistoryRepository.AddAsync(mapped);

                if (result <= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while adding history for request with ID {RequestId}", history.RequestId);
                throw;
            }
               
        }


        public async Task<PagedResult<RequestResultHistoryModelView>> GetAllListAsync(PaginationParams pagination ,
                                                                    string? dateFilter, string? studentNumberFilter)
        {
            try
            {
                var query = _requestHistoryRepository.GetViewAsync(dateFilter, studentNumberFilter);

                var totalCount = await query.CountAsync();

                var pagedData = await query
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                return new PagedResult<RequestResultHistoryModelView>(pagedData, totalCount, pagination.PageNumber, pagination.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting the list of request history");
                throw;
            }
        }

       


    }
}
