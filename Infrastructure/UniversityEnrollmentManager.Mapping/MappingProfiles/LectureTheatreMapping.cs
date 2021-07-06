using AutoMapper;
using UniversityEnrollmentManager.DomainModel.LectureTheatre;

namespace UniversityEnrollmentManager.Mapping.MappingProfiles
{
    public class LectureTheatreMapping : Profile
    {
        public LectureTheatreMapping()
        {
            CreateMap<LectureTheatreReadModel, Domain.Entities.LectureTheatre>().ReverseMap();
            CreateMap<LectureTheatreCreateModel, Domain.Entities.LectureTheatre>();
        }
    }
}
