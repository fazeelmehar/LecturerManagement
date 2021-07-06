using AutoMapper;
using UniversityEnrollmentManager.DomainModel.Lecture;

namespace UniversityEnrollmentManager.Mapping.MappingProfiles
{
    public class LectureMapping : Profile
    {
        public LectureMapping()
        {
            CreateMap<LectureReadModel, Domain.Entities.Lecture>().ReverseMap();
            CreateMap<LectureCreateModel, Domain.Entities.Lecture>();
        }
    }
}
