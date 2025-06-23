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

        public TeacherService(IMapper mapper, ITeacherRepository teacherRepository)
        {
            _mapper = mapper;   
            _teacherRepository = teacherRepository;
        }


        public async Task<Result> CheckAndAddTeacherAsync(TeacherCreateModel teacher) 
        {
            try
            {
                int existingTeacher = await _teacherRepository.GetTeacherIdByCodeAsync(teacher.TeacherCode);


                if (existingTeacher > 0) 
                {
                    return Result.AlreadyExist;
                }

                var teacherEntity = _mapper.Map<TeacherEntity>(teacher);

                var result =  await _teacherRepository.AddNewTeacherAsync(teacherEntity);

                return result > 0 ? Result.Success : Result.Failed;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in Checking Teacher.", ex);
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
                throw new ApplicationException("Error occurred in listing all teacher's data", ex);
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

                return Result.Success;
            }
            catch (Exception ex) 
            { 
                throw new ApplicationException("Error occured in updating teacher.", ex); 
            }
        }


        public async Task<Result> DeleteTeacherAsync(int teacherId) 
        {
            try
            {
                var result = await _teacherRepository.DeleteTeacherAsync(teacherId);
                if (result == false)
                {
                    return Result.Failed;
                }
                return Result.Success;
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("Error occured in deleting teacher.", ex);
            }


        }

    }
}
