using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models;
using DLARS.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace DLARS.Services
{
    public interface ITeacherSubjectsService
    {
        Task<AssignSubjectResult> RegisterSubjectsToTeacher(TeacherSubjectsCodeModel teacherSubjectsCode);
    }


    public class TeacherSubjectsService : ITeacherSubjectsService
    {
        private readonly IMapper _mapper;
        private readonly ITeacherSubjectsRepository _teacherSubjectsRepository;
        private readonly ITeacherRepository _teacherRepository;
        private readonly ISubjectRepository _subjectRepository;

        public TeacherSubjectsService(IMapper mapper, ITeacherSubjectsRepository teacherSubjectsRepository,
                                      ITeacherRepository teacherRepository, ISubjectRepository subjectRepository)
        {
            _mapper = mapper;
            _teacherSubjectsRepository = teacherSubjectsRepository;
            _teacherRepository = teacherRepository;
            _subjectRepository = subjectRepository;
        }



        public async Task<AssignSubjectResult> RegisterSubjectsToTeacher(TeacherSubjectsCodeModel teacherSubjectsCode)
        {
            try
            {
                int teacherId = await _teacherRepository.GetTeacherIdAsync(teacherSubjectsCode.TeacherCode);
                int subjectId = await _subjectRepository.GetSubjectIdAsync(teacherSubjectsCode.SubjectCode);

                if (teacherId <= 0 || subjectId <= 0)
                {
                    return AssignSubjectResult.DoesNotExist;
                }

                bool result = await _teacherSubjectsRepository.GetSubjectAndTeacherByIdAsync(teacherId, subjectId);

                if (result == true)
                {
                    return AssignSubjectResult.AlreadyExist;
                }

                var newModel = CreateNewTeacherSubjectsIdModel(teacherId, subjectId);

                var subjectTeacherEntity = _mapper.Map<TeacherSubjectsEntity>(newModel);

                await _teacherSubjectsRepository.AddTeacherandSubjectAsync(subjectTeacherEntity);

                return AssignSubjectResult.Success;


            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured in registering subjects to teacher.", ex);
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




    }
}
