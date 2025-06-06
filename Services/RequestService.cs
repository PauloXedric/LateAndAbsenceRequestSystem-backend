using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLARS.Entities;
using DLARS.Models;
using DLARS.Models.Pagination;
using DLARS.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Services
{

    public interface IRequestService 
    {
        Task<int> AddRequestAsync(RequestCreateModel request);
        Task<PagedResult<RequestReadModel>> GetRequestByStatusIdAsync(int statusId, PaginationParams pagination);
        Task<bool> UpdateRequestStatusAsync(RequestUpdateModel requestUpdate);
    }


    public class RequestService : IRequestService   
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _requestRepository;

        public RequestService(IMapper mapping, IRequestRepository requestRepository)
        {
            _mapper = mapping;
            _requestRepository = requestRepository;
        }


        public async Task<int> AddRequestAsync(RequestCreateModel request)
        {
            try 
            {
                var requestEntity = _mapper.Map<RequestEntity>(request);

                return await _requestRepository.AddRequestAsync(requestEntity);

            } 
            catch (Exception ex) 
            { 
                throw new ApplicationException("Error occured when adding new request.", ex); 
            }
           

        }


        public async Task<PagedResult<RequestReadModel>> GetRequestByStatusIdAsync(int statusId, PaginationParams pagination) 
        {
            try
            {
                var query = _requestRepository.GetRequestByStatusId(statusId);

                var totalCount = await query.CountAsync();

                var pagedData = await query
                    .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ProjectTo<RequestReadModel>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new PagedResult<RequestReadModel>(pagedData, totalCount, pagination.PageNumber, pagination.PageSize);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured in displaying request data.", ex);
            }
        }


        public async Task<bool> UpdateRequestStatusAsync(RequestUpdateModel requestUpdate) 
        {
            try
            {
                var requestEntity = _mapper.Map<RequestEntity>(requestUpdate);
                return await _requestRepository.UpdateRequestStatusAsync(requestEntity);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured in updating request status.", ex);
            }
        }
    }
}
