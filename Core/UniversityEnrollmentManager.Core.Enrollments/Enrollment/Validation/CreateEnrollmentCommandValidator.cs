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
            RuleFor(x => x)
           .Cascade(CascadeMode.Stop)
           .Must(x => CheckCapacityIsExceed(x.LectureId, unitOfWork))
           .WithMessage("Enrolment rejected as seating capacity for a given lecture theatre for the subject has been exceeded.")
            .Must(x => CheckWeeklyHoursIsExceed(x.StudentId, unitOfWork))
           .WithMessage($"Enrolment rejected as a maximum of {MaxClassAttendanceHoursAllowed} subject hours are allowed per week.");
        }

        bool CheckWeeklyHoursIsExceed(int? studentId, IUnitOfWork unitOfWork)
        {
            var enrollments = unitOfWork.Set<Domain.Entities.Enrollment>()
                                        .Include(s => s.Lecture)
                                        .Where(s => s.StudentId == studentId)
                                        .AsNoTracking()
                                        .ToList();

            if (enrollments != null)
            {
                return !(enrollments.Sum(s => s.Lecture.Duration.Hours) > MaxClassAttendanceHoursAllowed);
            }

            return false;
        }

        bool CheckCapacityIsExceed(int? lectureId, IUnitOfWork unitOfWork)
        {
            var lectures = unitOfWork.Set<Domain.Entities.Lecture>()
                                     .Include(x => x.Subject)
                                     .Include(x => x.LectureTheatre)
                                     .Include(x => x.Enrollments)
                                     .Where(s => s.Id == lectureId)
                                     .AsNoTracking()
                                     .FirstOrDefault();
            if (lectures != null)
            {
                return !(lectures.Subject.Lectures.FirstOrDefault().Enrollments != null && lectures.Subject.Lectures.FirstOrDefault().Enrollments.Count + 1 > lectures.LectureTheatre.SeatingCapacity);
            }

            return false;
        }
    }
}
