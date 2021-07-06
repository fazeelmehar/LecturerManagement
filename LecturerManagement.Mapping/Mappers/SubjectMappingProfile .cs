using AutoMapper;
using LecturerManagement.DomainModel.Subject;

namespace LecturerManagement.Mapping.Mappers
{
    public class SubjectMappingProfile : Profile
    {
        public SubjectMappingProfile()
        {
            CreateMap<SubjectReadModel, Domain.Entities.Subject>().ReverseMap();
            CreateMap<SubjectCreateModel, Domain.Entities.Subject>();
        }
    }
}
