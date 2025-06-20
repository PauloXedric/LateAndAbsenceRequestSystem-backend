using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models;
using DLARS.Repositories;

namespace DLARS.Services
{

    public interface ISubjectService
    {
        Task<AddingSubjectTeacherResult> CheckAndAddSubjectAsync(SubjectModel subject);
    }


    public class SubjectService : ISubjectService
    {

        private readonly IMapper _mapper;
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(IMapper mapper, ISubjectRepository subjectRepository)
        {
            _mapper = mapper;
            _subjectRepository = subjectRepository;
        }


        public async Task<AddingSubjectTeacherResult> CheckAndAddSubjectAsync(SubjectModel subject)
        {
            try
            {
                int existingTeacher = await _subjectRepository.GetSubjectIdAsync(subject.SubjectCode);


                if (existingTeacher > 0)
                {
                    return AddingSubjectTeacherResult.AlreadyExist;
                }

                var teacherEntity = _mapper.Map<SubjectsEntity>(subject);

                var result = await _subjectRepository.AddNewSubjectAsync(teacherEntity);

                return result > 0 ? AddingSubjectTeacherResult.Success : AddingSubjectTeacherResult.Failed;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in Checking Teacher.", ex);
            }
        }
    }
}
