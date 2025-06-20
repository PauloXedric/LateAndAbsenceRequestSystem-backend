using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models;
using DLARS.Repositories;

namespace DLARS.Services
{

    public interface ITeacherService
    {
        Task<AddingSubjectTeacherResult> CheckAndAddTeacherAsync(TeacherModel teacher);
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


        public async Task<AddingSubjectTeacherResult> CheckAndAddTeacherAsync(TeacherModel teacher) 
        {
            try
            {
                int existingTeacher = await _teacherRepository.GetTeacherIdAsync(teacher.TeacherCode);


                if (existingTeacher > 0) 
                {
                    return AddingSubjectTeacherResult.AlreadyExist;
                }

                var teacherEntity = _mapper.Map<TeacherEntity>(teacher);

                var result =  await _teacherRepository.AddNewTeacherAsync(teacherEntity);

                return result > 0 ? AddingSubjectTeacherResult.Success : AddingSubjectTeacherResult.Failed;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in Checking Teacher.", ex);
            }
        }
    }
}
