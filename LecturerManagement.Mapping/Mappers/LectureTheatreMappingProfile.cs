using AutoMapper;
using LecturerManagement.DomainModel.LectureTheatre;

namespace LecturerManagement.Mapping.Mappers
{
    public class LectureTheatreMappingProfile : Profile
    {
        public LectureTheatreMappingProfile()
        {
            CreateMap<LectureTheatreReadModel, Domain.Entities.LectureTheatre>().ReverseMap();
            CreateMap<LectureTheatreCreateModel, Domain.Entities.LectureTheatre>();
        }
    }
}
