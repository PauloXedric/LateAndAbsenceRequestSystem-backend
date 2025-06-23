using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.SubjectModel;
using DLARS.Models.SubjectModels;
using DLARS.Repositories;
using System.Threading.Tasks;

namespace DLARS.Services
{

    public interface ISubjectService
    {
        Task<Result> CheckAndAddSubjectAsync(SubjectCreateModel subject);
        Task<List<SubjectReadModel>> GetAllSubjectAsync();
        Task<Result> CheckAndUpdateSubjectAsync(SubjectUpdateModel updateSubject);
        Task<Result> DeleteSubjectAsync(int subjectId);
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
                int existingSubject = await _subjectRepository.GetSubjectIdByCodeAsync(subject.SubjectCode);


                if (existingSubject > 0)
                {
                    return Result.AlreadyExist;
                }

                var subjectEntity = _mapper.Map<SubjectsEntity>(subject);

                var result = await _subjectRepository.AddNewSubjectAsync(subjectEntity);

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
                throw new ApplicationException("Error occured in listing all subject's data.", ex);
            }
        }


        public async Task<Result> CheckAndUpdateSubjectAsync(SubjectUpdateModel updateSubject) 
        {
            try
            {
                var existingSubject = await _subjectRepository.GetByIdAsync(updateSubject.SubjectId);

                if (existingSubject == null)
                {
                    return Result.DoesNotExist;
                }

                existingSubject.SubjectCode = updateSubject.SubjectCode;
                existingSubject.SubjectName = updateSubject.SubjectName;

                await _subjectRepository.UpdateSubjectAsync(existingSubject);
                return Result.Success;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occured in updating subject.", ex);
            }
        }


        public async Task<Result> DeleteSubjectAsync(int subjectId) 
        {
            try
            {
                var result = await _subjectRepository.DeleteSubjectAsync(subjectId);
                if (result == false)
                {
                    return Result.Failed;
                }

                return Result.Success;
            }
            catch (Exception ex) 
            {
                throw new ApplicationException("Error occured in deleting subject", ex);
            }
        }

    }
}
