using AutoMapper;
using LecturerManagement.DomainModel.Student;

namespace LecturerManagement.Mapping.Mappers
{
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            CreateMap<StudentReadModel, Domain.Entities.Student>().ReverseMap();
            CreateMap<StudentCreateModel, Domain.Entities.Student>();
        }
    }
}
