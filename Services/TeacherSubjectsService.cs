using AutoMapper;
using DLARS.Controller;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.TeacherSubjectModels;
using DLARS.Repositories;
using DLARS.Views;

namespace DLARS.Services
{
    public interface ITeacherSubjectsService
    {
        Task<Result> RegisterSubjectsToTeacher(TeacherSubjectsCodeModel teacherSubjectsCode);
        Task<List<TeacherAssignedSubjectsModelView>> GetAllListAsync();
        Task<Result> DeleteTeacherWithSubjectsAssignedAsync(int teacherId);
    }


    public class TeacherSubjectsService : ITeacherSubjectsService
    {
        private readonly IMapper _mapper;
        private readonly ITeacherSubjectsRepository _teacherSubjectsRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly ILogger<TeacherSubjectsService> _logger;


        public TeacherSubjectsService(IMapper mapper, ITeacherSubjectsRepository teacherSubjectsRepository,
                                      ITeacherRepository teacherRepository, ISubjectRepository subjectRepository, 
                                      ILogger<TeacherSubjectsService> logger)
        {
            _mapper = mapper;
            _teacherSubjectsRepository = teacherSubjectsRepository;
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
            _logger = logger;
        }



        public async Task<Result> RegisterSubjectsToTeacher(TeacherSubjectsCodeModel teacherSubjectsCode)
        {
            try
            {
                int teacherId = await _teacherRepository.GetTeacherIdByCodeAsync(teacherSubjectsCode.TeacherCode);

                if (teacherId <= 0)
                {
                    return Result.DoesNotExist;
                }

               var deleted = await _teacherSubjectsRepository.DeleteAllSubjectsByTeacherIdAsync(teacherId);


                foreach (var subjectCode in teacherSubjectsCode.SubjectCode)
                {
                    int subjectId = await _subjectRepository.GetSubjectIdByCodeAsync(subjectCode);
                    if (subjectId <= 0) continue;

                    var newModel = CreateNewTeacherSubjectsIdModel(teacherId, subjectId);
                    var subjectTeacherEntity = _mapper.Map<TeacherSubjectEntity>(newModel);

                    await _teacherSubjectsRepository.AddAsync(subjectTeacherEntity);
                }

                return deleted ? Result.Updated : Result.Success;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering subject with Code {SubjectCode} to teacher with Code {TeacherCode}", 
                    teacherSubjectsCode.SubjectCode, 
                    teacherSubjectsCode.TeacherCode);
                throw;
            }
        }


        public TeacherSubjectsIdModel  CreateNewTeacherSubjectsIdModel(int teacherId, int subjectId)
        { 
                return new TeacherSubjectsIdModel
                {
                    TeacherId = teacherId,
                    SubjectId = subjectId
                };     
        }



        public async Task<List<TeacherAssignedSubjectsModelView>> GetAllListAsync()
        {
            try
            {
                return await _teacherSubjectsRepository.GetViewAsync();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while getting the list of teacher with their subjects");
                throw;
            }
        }



        public async Task<Result> DeleteTeacherWithSubjectsAssignedAsync(int teacherId)
        {
            try
            {
                var result = await _teacherSubjectsRepository.DeleteAllSubjectsByTeacherIdAsync(teacherId);

                if (result == false)
                {
                    return Result.Failed;
                }

                return Result.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting teacher with ID {TeacherId} with its subjects", teacherId);
                throw;
            }
        }

    }
}
