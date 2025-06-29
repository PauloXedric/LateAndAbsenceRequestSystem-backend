using AutoMapper;
using DLARS.Entities;
using DLARS.Models.Requests;
using DLARS.Models.SubjectModel;
using DLARS.Models.TeacherModels;
using DLARS.Models.TeacherSubjectModels;
using DLARS.Views;

namespace DLARS.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<RequestEntity, RequestCreateModel>();
            CreateMap<RequestCreateModel, RequestEntity>();
             
            CreateMap<RequestEntity, RequestReadModel>();
            CreateMap<RequestUpdateModel, RequestEntity>();

            CreateMap<TeacherCreateModel, TeacherEntity>();
            CreateMap<TeacherEntity, TeacherReadModel>();

            CreateMap<SubjectCreateModel, SubjectEntity>();
            CreateMap<SubjectEntity, SubjectReadModel>();

            CreateMap<TeacherSubjectsIdModel, TeacherSubjectEntity>();

            CreateMap<AddImageUploadInRequestModel, RequestEntity>();


            CreateMap<TeacherSubjectEntity, TeacherAssignedSubjectsModelView>();
        }


    }
}