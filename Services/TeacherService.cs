using AutoMapper;
using DLARS.Entities;
using DLARS.Models;
using DLARS.Repositories;

namespace DLARS.Services
{

    public interface ITeacherService
    {
        Task<bool> CheckAndAddTeacherAsync(TeacherModel teacher);
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


        public async Task<bool> CheckAndAddTeacherAsync(TeacherModel teacher) 
        {
            try
            {
                int existingTeacher = await _teacherRepository.GetTeacherIdAsync(teacher.TeacherCode);


                if (existingTeacher <= 0) 
                {
                    return false;
                }

                var teacherEntity = _mapper.Map<TeacherEntity>(teacher);

                return await _teacherRepository.AddNewTeacherAsync(teacherEntity) > 0;
                            
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in Checking Teacher.", ex);
            }
        }
    }
}
