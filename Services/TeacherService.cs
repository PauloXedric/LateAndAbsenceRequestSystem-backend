using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.TeacherModels;
using DLARS.Repositories;

namespace DLARS.Services
{

    public interface ITeacherService
    {
        Task<Result> CheckAndAddTeacherAsync(TeacherCreateModel teacher);
        Task<List<TeacherReadModel>> GetAllTeacherAsync();
        Task<Result> CheckAndUpdateTeacherAsync(TeacherUpdateModel updateTeacher);
        Task<Result> DeleteTeacherAsync(int teacherId);
    }

    public class TeacherService : ITeacherService   
    {
        private readonly IMapper _mapper;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ITeacherSubjectsRepository _teacherSubjectsRepository;
        private readonly ILogger<TeacherService> _logger;

        public TeacherService(IMapper mapper, ITeacherRepository teacherRepository, 
                               ITeacherSubjectsRepository teacherSubjectsRepository, ILogger<TeacherService> logger)
        {
            _mapper = mapper;   
            _teacherRepository = teacherRepository;
            _teacherSubjectsRepository = teacherSubjectsRepository;
            _logger = logger;
        }


        public async Task<Result> CheckAndAddTeacherAsync(TeacherCreateModel teacher) 
        {
            try
            {
                int existingTeacher = await _teacherRepository.GetTeacherIdByCodeAsync(teacher.TeacherCode);


                if (existingTeacher > 0) 
                {
                    _logger.LogWarning("Duplicate teacher code: {TeacherCode}", teacher.TeacherCode);
                    return Result.AlreadyExist;
                }

                var teacherEntity = _mapper.Map<TeacherEntity>(teacher);

                var result =  await _teacherRepository.AddAsync(teacherEntity);

                return result > 0 ? Result.Success : Result.Failed;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add teacher with code {TeacherCode}", teacher.TeacherCode);
                throw;
            }
        }


        public async Task<List<TeacherReadModel>> GetAllTeacherAsync()
        {
            try
            {         

                var teacherList = await _teacherRepository.GetAllTeacherAsync();

                var teacherReadModel = _mapper.Map<List<TeacherReadModel>>(teacherList);

                return teacherReadModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all teachers.");
                throw;
            }
        }


        public async Task<Result> CheckAndUpdateTeacherAsync(TeacherUpdateModel updateTeacher) 
        {
            try
            {
                var existingTeacher = await _teacherRepository.GetByIdAsync(updateTeacher.TeacherId);

                if (existingTeacher == null)
                {
                    return Result.DoesNotExist;
                }

                existingTeacher.TeacherCode = updateTeacher.TeacherCode;
                existingTeacher.TeacherName = updateTeacher.TeacherName;

                await _teacherRepository.UpdateAsync(existingTeacher);
                return Result.Success;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while updating teacher with ID {TeacherId}", updateTeacher.TeacherId);
                throw;
            }
        }


        public async Task<Result> DeleteTeacherAsync(int teacherId) 
        {
            try
            {
       
                var result = await _teacherRepository.DeleteAsync(teacherId);
                if (result == false)
                {
                    return Result.Failed;
                }

                await _teacherSubjectsRepository.DeleteAllSubjectsByTeacherIdAsync(teacherId);

                return Result.Success;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error occurred while deleting teacher with ID {TeacherId}", teacherId);
                throw;
            }


        }

    }
}
