using AutoMapper;
using DLARS.Entities;
using DLARS.Models;
using DLARS.Repositories;

namespace DLARS.Services
{

    public interface ISubjectService
    {
        Task<bool> CheckAndAddSubjectAsync(SubjectModel subject);
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


        public async Task<bool> CheckAndAddSubjectAsync(SubjectModel subject)
        {
            try
            {
                int existingSubject = await _subjectRepository.GetSubjectIdAsync(subject.SubjectCode);


                if (existingSubject <= 0)
                {
                    return false;
                }

                var subjectEntity = _mapper.Map<SubjectsEntity>(subject);

                return await  _subjectRepository.AddNewSubjectAsync(subjectEntity) > 0;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured while checking or adding subject.", ex);
            }
        }
    }
}
