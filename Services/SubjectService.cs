using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.SubjectModel;
using DLARS.Repositories;

namespace DLARS.Services
{

    public interface ISubjectService
    {
        Task<Result> CheckAndAddSubjectAsync(SubjectCreateModel subject);
        Task<List<SubjectReadModel>> GetAllSubjectAsync();
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


        public async Task<Result> CheckAndAddSubjectAsync(SubjectCreateModel subject)
        {
            try
            {
                int existingSubject = await _subjectRepository.GetSubjectIdAsync(subject.SubjectCode);


                if (existingSubject > 0)
                {
                    return Result.AlreadyExist;
                }

                var teacherEntity = _mapper.Map<SubjectsEntity>(subject);

                var result = await _subjectRepository.AddNewSubjectAsync(teacherEntity);

                return result > 0 ? Result.Success : Result.Failed;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in Checking Teacher.", ex);
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
                throw new ApplicationException("Error occured in listing all subject's data.");
            }
        }


    }
}
