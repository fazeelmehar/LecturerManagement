using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using UniversityEnrollmentManager.DomainModel.Enrollment;
using UniversityEnrollmentManager.Infrastructure.DBEntityValidation;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Enrollment.Validation
{
    public class CreateEnrollmentCommandValidator : BaseValidator<EnrollmentCreateModel, Domain.Entities.Enrollment, int>
    {
        const int MaxClassAttendanceHoursAllowed = 10;

        public CreateEnrollmentCommandValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            RuleFor(x => x.SubjectId)
                .NotNull()
                .NotEmpty()
                .WithMessage("A SubjectId must be supplied to allow an Enrollment to occur.");

            RuleFor(x => x)
           .Cascade(CascadeMode.Stop)
           .Must(x => HaveASubjectToEnrolIn(unitOfWork, x.SubjectId))
           .Must(x => CheckStudentIsntCurrentlyEnrolledInSubject(x.StudentId, x.SubjectId, unitOfWork))
           .WithMessage("Enrollment rejected as the student is currently enrolled in this subject.")
           .Must(x => CheckCapacityIsNotExceed(x.SubjectId, unitOfWork))
           .WithMessage("Enrolment rejected as seating capacity for a given lecture theatre for the subject has been exceeded.")
            .Must(x => CheckWeeklyHoursIsExceed(x.StudentId, unitOfWork))
           .WithMessage($"Enrolment rejected as a maximum of {MaxClassAttendanceHoursAllowed} subject hours are allowed per week.");
        }

        bool HaveASubjectToEnrolIn(IUnitOfWork unitOfWork, int subjectId)
        {
            return unitOfWork.Set<Domain.Entities.Subject>().Any(s => s.Id == subjectId);
        }

        bool CheckStudentIsntCurrentlyEnrolledInSubject(int studentId, int subjectId, IUnitOfWork unitOfWork)
        {
            return !unitOfWork.Set<Domain.Entities.Enrollment>()
                                .Any(e => e.StudentId == studentId && e.SubjectId == subjectId);
        }

        bool CheckWeeklyHoursIsExceed(int studentId, IUnitOfWork unitOfWork)
        {
            var enrollments = unitOfWork.Set<Domain.Entities.Enrollment>()
                                        .Include(s => s.Subject)
                                        .ThenInclude(l => l.Lectures)
                                        .Where(s => s.StudentId == studentId)
                                        .AsNoTracking()
                                        .ToList();

            if (enrollments != null)
            {
                return !(enrollments.SelectMany(s => s.Subject.Lectures).Sum(l => l.Duration.Hours) > MaxClassAttendanceHoursAllowed);
            }

            return false;
        }

        bool CheckCapacityIsNotExceed(int subjectId, IUnitOfWork unitOfWork)
        {
            var lectures = unitOfWork.Set<Domain.Entities.Lecture>()
                                .Include(l => l.LectureTheatre)
                                .Include(l => l.Subject)
                                .Where(l => l.SubjectId == subjectId && l.Subject.Enrollments.Any())
                                .AsNoTracking();

            if (lectures != null && lectures.Any())
            {
                return !(lectures.All(l => l.Subject.Enrollments.Count + 1 > l.LectureTheatre.SeatingCapacity));
            }

            return true;
        }
    }
}
