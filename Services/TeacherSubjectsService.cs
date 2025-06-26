using AutoMapper;
using DLARS.Entities;
using DLARS.Enums;
using DLARS.Models.TeacherSubjectModels;
using DLARS.Repositories;
using DLARS.Views;
using System.Reflection.Metadata.Ecma335;

namespace DLARS.Services
{
    public interface ITeacherSubjectsService
    {
        Task<Result> RegisterSubjectsToTeacher(TeacherSubjectsCodeModel teacherSubjectsCode);
        Task<List<TeacherAssignedSubjectsModelView>> GetAllListAsync();
        Task<Result> DeleteTeacherWithSubjectsAssignedAsync(int teacherId);
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



        public async Task<Result> RegisterSubjectsToTeacher(TeacherSubjectsCodeModel teacherSubjectsCode)
        {
            try
            {
                int teacherId = await _teacherRepository.GetTeacherIdByCodeAsync(teacherSubjectsCode.TeacherCode);

                if (teacherId <= 0)
                {
                    return Result.DoesNotExist;
                }

               var deleted = await _teacherSubjectsRepository.DeleteAllSubjectsByTeacherIdAsync(teacherId);


                foreach (var subjectCode in teacherSubjectsCode.SubjectCode)
                {
                    int subjectId = await _subjectRepository.GetSubjectIdByCodeAsync(subjectCode);
                    if (subjectId <= 0) continue;

                    var newModel = CreateNewTeacherSubjectsIdModel(teacherId, subjectId);
                    var subjectTeacherEntity = _mapper.Map<TeacherSubjectEntity>(newModel);

                    await _teacherSubjectsRepository.AddTeacherandSubjectAsync(subjectTeacherEntity);
                }

                return deleted ? Result.Updated : Result.Success;

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



        public async Task<List<TeacherAssignedSubjectsModelView>> GetAllListAsync()
        {
            try
            {
                return await _teacherSubjectsRepository.GetAllAsync();
            }
            catch (Exception ex) { throw new ApplicationException("Error occured while getting a list of teacher with their subjects", ex); }
        }



        public async Task<Result> DeleteTeacherWithSubjectsAssignedAsync(int teacherId)
        {
            try
            {
                var result = await _teacherSubjectsRepository.DeleteAllSubjectsByTeacherIdAsync(teacherId);

                if (result == false)
                {
                    return Result.Failed;
                }

                return Result.Success;
            }
            catch (Exception ex) { throw new ApplicationException("Error occured while deleting teacher and their subjects.", ex); }
            
        }

    }
}
