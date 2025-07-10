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
        Task<bool> CheckRequestSubmittedStatusAsync(int requestId);
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
                bool exist = await _requestRepository.SubjectExistsInRequestAsync(request.StudentNumber, request.SubjectCode);

                if (exist)
                {
                    return Result.AlreadyExist;
                }
                var requestEntity = _mapper.Map<RequestEntity>(request);
                
                var result = await _requestRepository.AddAsync(requestEntity);

                return result > 0 ? Result.Success : Result.Failed;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding new request with student number {StudentNumber}", request.StudentNumber);
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
                _logger.LogError(ex, "Error occurred while getting the list request data");
                throw;
            }
        }


        public async Task<bool> UpdateRequestStatusAsync(RequestUpdateModel requestUpdate)
        {
            try
            {
                var requestEntity = await _requestRepository.GetByIdAsync(requestUpdate.RequestId);
                if (requestEntity == null) 
                { 
                    _logger.LogWarning("Request with id {RequestId} does not exist", requestUpdate.RequestId);
                    return false;
                }

                _mapper.Map(requestUpdate, requestEntity);

                return await _requestRepository.UpdateAsync(requestEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating request with ID {RequestId}", requestUpdate.RequestId);
                throw;
            }
        }

        public async Task<bool> AddImageInRequestAsync(AddImageReceivedInRequestModel imageReceived)
        {
            try
            {
                var proofPath = _fileStorageService.SaveFile(imageReceived.ProofImage, "imageproof");
                var parentIdPath = _fileStorageService.SaveFile(imageReceived.ParentValidImage, "parentvalidid");
                var medCertPath = _fileStorageService.SaveFile(imageReceived.MedicalCertificate! , "medicalcertificate");

                var requestEntity = await _requestRepository.GetByIdAsync(imageReceived.RequestId);
                if (requestEntity == null)
                {
                    _logger.LogWarning("Request with ID {RequestId} not found.", imageReceived.RequestId);
                    return false;
                }

                var uploadModel = _mapper.Map<AddImageUploadInRequestModel>(imageReceived);
                uploadModel.ProofImage = proofPath;
                uploadModel.ParentValidImage = parentIdPath;
                uploadModel.MedicalCertificate = medCertPath;

                _mapper.Map(uploadModel, requestEntity);

                return await _requestRepository.UpdateAsync(requestEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding supporting documents in request with ID {ReqeustId}", imageReceived.RequestId);
                throw;
            }
        }


        public async Task<bool> CheckRequestSubmittedStatusAsync(int requestId)
        {
            try
            {
                return await _requestRepository.GetSubmittedStatusByidAsync(requestId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking submitted status of request with Id {RequestId}", requestId);
                throw;
            }
        }
   
    }
}
