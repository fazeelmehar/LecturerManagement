using FluentValidation;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Enrollment;
using LecturerManagement.Infrastructure.FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Linq.Dynamic.Core;

namespace LecturerManagement.Core.Lecturer.Enrollment.Validation
{
    public class CreateEnrollmentCommandValidator : BaseValidator<EnrollmentCreateModel, Domain.Entities.Enrollment, int>
    {
        public CreateEnrollmentCommandValidator(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            RuleFor(x => x)
           .Cascade(CascadeMode.StopOnFirstFailure)
           .Must(x => CheckCapacityIsExceed(x.LectureId, unitOfWork))
           .WithMessage("Enrolment Rejected Due to Exceed Limit")
            .Must(x => CheckWeeklyHoursIsExceed(x.LectureId, x.StudentId, unitOfWork))
           .WithMessage("Enrolment Rejected Due to Exceed Weekly Hours Limit");
        }

        private bool CheckWeeklyHoursIsExceed(int? lectureId, int? studentId, IUnitOfWork unitOfWork)
        {
            var enrollments = unitOfWork.Set<Domain.Entities.Enrollment>()
                                        .Include(s => s.Lecture)
                                        .Where(s => s.StudentId == studentId)
                                        .AsNoTracking()
                                        .ToList();

            if (enrollments != null)
            {
                if (enrollments.Sum(s => s.Lecture.Duration.Hours) > 10)
                    return false;
                else
                    return true;
            }
            return false;
        }

        private bool CheckCapacityIsExceed(int? lectureId, IUnitOfWork unitOfWork)
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
                if (lectures.Subject.Lectures.FirstOrDefault().Enrollments != null && lectures.Subject.Lectures.FirstOrDefault().Enrollments.Count + 1 > lectures.LectureTheatre.SeatingCapacity)
                    return false;
                else
                    return true;
            }
            return false;
        }
    }
}
