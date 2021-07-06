using AutoMapper;
using LecturerManagement.DomainModel.Enrollment;

namespace LecturerManagement.Mapping.Mappers
{
    public class EnrollmentMappingProfile : Profile
    {
        public EnrollmentMappingProfile()
        {
            CreateMap<EnrollmentReadModel, Domain.Entities.Enrollment>().ReverseMap();
            CreateMap<EnrollmentCreateModel, Domain.Entities.Enrollment>();
        }
    }
}