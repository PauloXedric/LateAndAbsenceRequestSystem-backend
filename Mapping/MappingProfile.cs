using AutoMapper;
using DLARS.Entities;
using DLARS.Models;

namespace DLARS.Mappings
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<RequestEntity, RequestCreateModel>();
            CreateMap<RequestCreateModel, RequestEntity>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => 1));

            CreateMap<RequestEntity, RequestReadModel>();
            CreateMap<RequestUpdateModel, RequestEntity>();

            CreateMap<TeacherModel, TeacherEntity>();

            CreateMap<SubjectModel, SubjectsEntity>();

            CreateMap<TeacherSubjectsIdModel, TeacherSubjectsEntity>();

            CreateMap<AddImageUploadInRequestModel, RequestEntity>();
        }


    }
}