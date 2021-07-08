﻿using AutoMapper;
using UniversityEnrollmentManager.DomainModel.Subject;

namespace UniversityEnrollmentManager.Mapping.MappingProfiles
{
    public class SubjectMapping : Profile
    {
        public SubjectMapping()
        {
            CreateMap<SubjectEnrollmentsReadModel, Domain.Entities.Subject>().ReverseMap();
            CreateMap<SubjectReadModel, Domain.Entities.Subject>().ReverseMap();
            CreateMap<SubjectCreateModel, Domain.Entities.Subject>();
        }
    }
}
