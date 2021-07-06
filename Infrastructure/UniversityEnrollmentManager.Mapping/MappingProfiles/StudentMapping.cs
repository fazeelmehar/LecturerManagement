using AutoMapper;
using UniversityEnrollmentManager.DomainModel.Student;

namespace UniversityEnrollmentManager.Mapping.MappingProfiles
{
    public class StudentMapping : Profile
    {
        public StudentMapping()
        {
            CreateMap<StudentReadModel, Domain.Entities.Student>().ReverseMap();
            CreateMap<StudentCreateModel, Domain.Entities.Student>();
        }
    }
}
