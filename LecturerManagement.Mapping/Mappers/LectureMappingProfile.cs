using AutoMapper;
using LecturerManagement.DomainModel.Lecture;

namespace LecturerManagement.Mapping.Mappers
{
    public class LectureMappingProfile : Profile
    {
        public LectureMappingProfile()
        {
            CreateMap<LectureReadModel, Domain.Entities.Lecture>().ReverseMap();
            CreateMap<LectureCreateModel, Domain.Entities.Lecture>();
        }
    }
}