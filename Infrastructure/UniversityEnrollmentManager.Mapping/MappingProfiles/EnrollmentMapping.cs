using AutoMapper;
using UniversityEnrollmentManager.DomainModel.Enrollment;


namespace UniversityEnrollmentManager.Mapping.MappingProfiles
{
    public class EnrollmentMapping : Profile
    {
        public EnrollmentMapping()
        {
            CreateMap<EnrollmentReadModel, Domain.Entities.Enrollment>().ReverseMap();
            CreateMap<EnrollmentCreateModel, Domain.Entities.Enrollment>();
        }
    }
}
