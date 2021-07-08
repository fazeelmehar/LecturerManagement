﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.Enrollment.Validation;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Lecture;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Lecture.Handlers
{
    public class CreateLectureCommandHandler<TUnitOfWork>
       : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>
       where TUnitOfWork : IUnitOfWork
    {
        public CreateLectureCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<LectureReadModel>> ProcessAsync(EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<LectureReadModel>();
            try
            {
                var validate = new CreateLectureCommandValidator(DataContext);
                var validateResult = validate.Validate(request.Model);
                if (!validateResult.IsValid)
                {
                    var errors = new Hashtable();
                    foreach (var validateResultError in validateResult.Errors)
                    {
                        errors.Add("Error", validateResultError.ErrorMessage);
                    }
                    return new EntityResponseModel<LectureReadModel>
                    {
                        ReturnStatus = false,
                        Errors = errors
                    };
                }

                var lecture = Mapper.Map<Domain.Entities.Lecture>(request.Model);

                // Check lecture theatre exists
                if (!request.Model.LectureTheatreId.HasValue ||
                   !DataContext.Set<Domain.Entities.LectureTheatre>().Any(s => s.Id == request.Model.LectureTheatreId))
                {
                    response.ReturnMessage.Add("No lecture theatre exists for the lecture theatre Id supplied.");
                    response.ReturnStatus = false;
                    return response;
                }

                // Check subject exists
                if (!request.Model.SubjectId.HasValue ||
                   !DataContext.Set<Domain.Entities.Subject>().Any(s => s.Id == request.Model.SubjectId))
                {
                    response.ReturnMessage.Add("No subject exists for the subject Id supplied.");
                    response.ReturnStatus = false;
                    return response;
                }

                // Check lecture theatre not already booked at the same time or overlapping durations
               // To Do

                DataContext.Set<Domain.Entities.Lecture>().Add(lecture);
                DataContext.CommitTransaction();

                response.Data = Mapper.Map<LectureReadModel>(
                                                            await DataContext.Set<Domain.Entities.Lecture>()
                                                            .Include(s => s.Subject)
                                                            .Include(s => s.LectureTheatre)
                                                            .FirstOrDefaultAsync(s => s.Id == lecture.Id)
                                                            .ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to update record {typeof(Domain.Entities.Lecture).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
