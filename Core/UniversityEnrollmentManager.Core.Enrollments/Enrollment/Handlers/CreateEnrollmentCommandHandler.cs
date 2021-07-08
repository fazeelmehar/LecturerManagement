using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.Enrollment.Validation;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Enrollment;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Enrollment.Handlers
{
    public class CreateEnrollmentCommandHandler<TUnitOfWork>
       : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<EnrollmentCreateModel, EntityResponseModel<EnrollmentReadModel>>, EntityResponseModel<EnrollmentReadModel>>
       where TUnitOfWork : IUnitOfWork
    {
        public CreateEnrollmentCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<EnrollmentReadModel>> ProcessAsync(EntityRequestModel<EnrollmentCreateModel, EntityResponseModel<EnrollmentReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<EnrollmentReadModel>();
            try
            {
                var validate = new CreateEnrollmentCommandValidator(DataContext);
                var validateResult = validate.Validate(request.Model);
                if (!validateResult.IsValid)
                {
                    var errors = new Hashtable();
                    foreach (var validateResultError in validateResult.Errors)
                    {
                        errors.Add("Error", validateResultError.ErrorMessage);
                    }
                    return new EntityResponseModel<EnrollmentReadModel>
                    {
                        ReturnStatus = false,
                        Errors = errors
                    };
                }

                var enrollment = Mapper.Map<Domain.Entities.Enrollment>(request.Model);


                DataContext.Set<Domain.Entities.Enrollment>().Add(enrollment);
                DataContext.CommitTransaction();

                response.Data = Mapper.Map<EnrollmentReadModel>(
                                                            await DataContext.Set<Domain.Entities.Enrollment>()
                                                            .Include(s => s.Student)
                                                            .Include(s => s.Subject)
                                                            .FirstOrDefaultAsync(s => s.Id == enrollment.Id)
                                                            .ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to update record {typeof(Domain.Entities.Enrollment).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}