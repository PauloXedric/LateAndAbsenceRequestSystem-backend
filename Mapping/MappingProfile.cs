using AutoMapper;
using DLARS.Entities;
using DLARS.Models.RequestHistory;
using DLARS.Models.Requests;
using DLARS.Models.SubjectModel;
using DLARS.Models.SubjectModels;
using DLARS.Models.TeacherModels;
using DLARS.Models.TeacherSubjectModels;
using DLARS.Views;

namespace DLARS.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            //Request Entity 
            CreateMap<RequestEntity, RequestCreateModel>();
            CreateMap<RequestCreateModel, RequestEntity>();                       
            CreateMap<RequestEntity, RequestReadModel>();

            CreateMap<AddImageReceivedInRequestModel, AddImageUploadInRequestModel>()
               .ForMember(dest => dest.ProofImage, opt => opt.Ignore())
               .ForMember(dest => dest.ParentValidImage, opt => opt.Ignore())
               .ForMember(dest => dest.MedicalCertificate, opt => opt.Ignore());

            CreateMap<AddImageUploadInRequestModel, RequestEntity>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<RequestUpdateModel, RequestEntity>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            //RequestHistory Entity 
            CreateMap<RequestHistoryCreateModel, RequestHistoryEntity>();

            //Teacher Entity 
            CreateMap<TeacherCreateModel, TeacherEntity>();
            CreateMap<TeacherUpdateModel, TeacherEntity>();
            CreateMap<TeacherEntity, TeacherReadModel>();

            //Subject Entity 
            CreateMap<SubjectCreateModel, SubjectEntity>();
            CreateMap<SubjectUpdateModel, SubjectEntity>();
            CreateMap<SubjectEntity, SubjectReadModel>();

            //TeacherSubject Entity 
            CreateMap<TeacherSubjectsIdModel, TeacherSubjectEntity>();
           
        }


    }
}