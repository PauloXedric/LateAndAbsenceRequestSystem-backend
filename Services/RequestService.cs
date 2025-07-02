using AutoMapper;
using AutoMapper.QueryableExtensions;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.Pagination;
using DLARS.Models.Requests;
using DLARS.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DLARS.Services
{

    public interface IRequestService 
    {
        Task<Result> AddRequestAsync(RequestCreateModel request);
        Task<PagedResult<RequestReadModel>> GetRequestByStatusIdAsync(RequestStatus statusId, PaginationParams pagination, string? filter);
        Task<bool> UpdateRequestStatusAsync(RequestUpdateModel requestUpdate);
        Task<bool> AddImageInRequestAsync(AddImageReceivedInRequestModel addImageInRequest);
    }


    public class RequestService : IRequestService
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _requestRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<RequestService> _logger;
       

        public RequestService(IMapper mapping, IRequestRepository requestRepository, 
                              IFileStorageService fileStorageService, ILogger<RequestService> logger)
        {
            _mapper = mapping;
            _requestRepository = requestRepository;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }


        public async Task<Result> AddRequestAsync(RequestCreateModel request)
        {
            try
            {
                var requestEntity = _mapper.Map<RequestEntity>(request);
                
                var result = await _requestRepository.AddAsync(requestEntity);

                return result > 0 ? Result.Success : Result.Failed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new request");
                throw;
            }
        }


        public async Task<PagedResult<RequestReadModel>> GetRequestByStatusIdAsync(RequestStatus statusId, PaginationParams pagination, string? filter)
        {
            try
            {
                var query = _requestRepository.GetRequestByStatusId(statusId, filter);

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
                _logger.LogError(ex, "Error occurred while displaying request data");
                throw;
            }
        }


        public async Task<bool> UpdateRequestStatusAsync(RequestUpdateModel requestUpdate)
        {
            try
            {
                return await _requestRepository.UpdateRequestStatusAsync(
                    requestUpdate.RequestId,
                    requestUpdate.StatusId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating request with ID {RequestId}", requestUpdate.RequestId);
                throw;
            }
        }

        public async Task<bool> AddImageInRequestAsync(AddImageReceivedInRequestModel imagedReceived)
        {
            try
            {
                var proofPath = _fileStorageService.SaveFile(imagedReceived.ProofImage, "imageproof");
                var parentIdPath = _fileStorageService.SaveFile(imagedReceived.ParentValidImage, "parentvalidid");
                var medCertPath = _fileStorageService.SaveFile(imagedReceived.MedicalCertificate, "medicalcertificate");

                var updatedModel = new AddImageUploadInRequestModel
                {
                    RequestId = imagedReceived.RequestId,
                    StatusId = imagedReceived.StatusId,
                    ProofImage = proofPath,
                    ParentValidImage = parentIdPath,
                    MedicalCertificate = medCertPath
                };
                var requestEntity = _mapper.Map<RequestEntity>(updatedModel);
                return await _requestRepository.AddImageInRequestAsync(requestEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding supporting documents in request with ID {ReqeustId}", imagedReceived.RequestId);
                throw;
            }
        }

   
    }
}
