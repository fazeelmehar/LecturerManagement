using AutoMapper;
using LecturerManagement.Core.CQRS.Queries;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Core.CQRS.Model;
using LecturerManagement.Core.Lecturer.Enrollment.Validation;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Enrollment;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.Enrollment.Handler
{
    public class CreateEnrollmentCommandHandler<TUnitOfWork>
       : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<EnrollmentCreateModel, EntityResponseModel<EnrollmentReadModel>>, EntityResponseModel<EnrollmentReadModel>>
       where TUnitOfWork : IUnitOfWork
    {
        public CreateEnrollmentCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
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
                                                            .Include(s => s.Lecture)
                                                            .FirstOrDefaultAsync(s => s.Id == enrollment.Id)
                                                            .ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add(String.Format("Unable to Update Record {0}" + ex.Message, typeof(Domain.Entities.Enrollment).Name));
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;



        }
    }
}
