using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.SubjectModel;
using DLARS.Models.SubjectModels;
using DLARS.Repositories;
using System.Threading.Tasks;

namespace DLARS.Services
{

    public interface ISubjectService
    {
        Task<Result> CheckAndAddSubjectAsync(SubjectCreateModel subject);
        Task<List<SubjectReadModel>> GetAllSubjectAsync();
        Task<Result> CheckAndUpdateSubjectAsync(SubjectUpdateModel updateSubject);
        Task<Result> DeleteSubjectAsync(int subjectId);
    }


    public class SubjectService : ISubjectService
    {

        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ITeacherSubjectsRepository _teacherSubjectsRepository;
        private readonly ILogger<TeacherService> _logger;

        public SubjectService(IMapper mapper, ISubjectRepository subjectRepository, 
                              ITeacherSubjectsRepository teacherSubjectsRepository, ILogger<TeacherService> logger)
        {
            _mapper = mapper;
            _subjectRepository = subjectRepository;
            _teacherSubjectsRepository = teacherSubjectsRepository;
            _logger = logger;
        }


        public async Task<Result> CheckAndAddSubjectAsync(SubjectCreateModel subject)
        {
            try
            {
                int existingSubject = await _subjectRepository.GetSubjectIdByCodeAsync(subject.SubjectCode);


                if (existingSubject > 0)
                {
                    _logger.LogWarning("Duplicate subject Code: {SubjectCode}", subject.SubjectCode);
                    return Result.AlreadyExist;
                }

                var subjectEntity = _mapper.Map<SubjectEntity>(subject);

                var result = await _subjectRepository.AddAsync(subjectEntity);

                return result > 0 ? Result.Success : Result.Failed;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add subject with Code {SubjectCode}", subject.SubjectCode);
                throw;
            }
        }


        public async Task<List<SubjectReadModel>> GetAllSubjectAsync() 
        {
            try
            {
                var subjectList = await _subjectRepository.GetAllSubjectAsync();

                var subjectReadModel = _mapper.Map<List<SubjectReadModel>>(subjectList);

                return subjectReadModel;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occured while retrieving all subject data.");
                throw;
            }
        }


        public async Task<Result> CheckAndUpdateSubjectAsync(SubjectUpdateModel updateSubject) 
        {
            try
            {
                var existingSubject = await _subjectRepository.GetByIdAsync(updateSubject.SubjectId);

                if (existingSubject == null)
                {
                    return Result.DoesNotExist;
                }

                existingSubject.SubjectCode = updateSubject.SubjectCode;
                existingSubject.SubjectName = updateSubject.SubjectName;

                await _subjectRepository.UpdateAsync(existingSubject);
                return Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating subject with Code {SubjectCode}", updateSubject.SubjectCode);
                throw;
            }
        }


        public async Task<Result> DeleteSubjectAsync(int subjectId) 
        {
            try
            {
                var result = await _subjectRepository.DeleteAsync(subjectId);
                if (result == false)
                {
                    return Result.Failed;
                }
                await _teacherSubjectsRepository.DeleteAllTeacherBySubjectIdAsync(subjectId);

                return Result.Success;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occured while deleting subject with ID {SubjectId}", subjectId);
                throw;
            }
        }

    }
}
