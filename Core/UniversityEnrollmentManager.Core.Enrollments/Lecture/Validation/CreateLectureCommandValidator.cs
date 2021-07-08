using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using UniversityEnrollmentManager.DomainModel.Lecture;
using UniversityEnrollmentManager.Infrastructure.DBEntityValidation;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Enrollment.Validation
{
    public class CreateLectureCommandValidator : BaseValidator<LectureCreateModel, Domain.Entities.Lecture, int>
    {

        public CreateLectureCommandValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            RuleFor(x => x.SubjectId)
                .NotNull()
                .NotEmpty()
                .WithMessage("A SubjectId must be supplied to allow an Enrollment to occur.");

            RuleFor(x => x.LectureTheatreId)
                .NotNull()
                .NotEmpty()
                .WithMessage("A LectureTheatreId must be supplied to allow an Enrollment to occur.");

            RuleFor(x => x)
           .Cascade(CascadeMode.Stop)
            .Must(x => CheckLectureTheatreIsAvailable(x, unitOfWork))
           .WithMessage("Lecture Theatre already booked");
        }

        private bool CheckLectureTheatreIsAvailable(LectureCreateModel request, IUnitOfWork unitOfWork)
        {
            return !unitOfWork.Set<Domain.Entities.Lecture>()
                              .Any(s => s.LectureTheatreId == request.LectureTheatreId &&
                                        s.DateTime == request.DateTime && 
                                        s.Duration == request.Duration);
        }
    }
}
