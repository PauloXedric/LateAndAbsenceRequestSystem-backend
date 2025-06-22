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
                int existingTeacher = await _teacherRepository.GetTeacherIdAsync(teacher.TeacherCode);


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


    }
}
