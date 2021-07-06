using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Subject;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Subject.Handlers
{
    public class CreateSubjectCommandHandler<TUnitOfWork>
        : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<SubjectCreateModel, EntityResponseModel<SubjectReadModel>>, EntityResponseModel<SubjectReadModel>>
        where TUnitOfWork : IUnitOfWork
    {
        public CreateSubjectCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }
        protected override async Task<EntityResponseModel<SubjectReadModel>> ProcessAsync(EntityRequestModel<SubjectCreateModel, EntityResponseModel<SubjectReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<SubjectReadModel>();
            try
            {
                var subject = Mapper.Map<Domain.Entities.Subject>(request.Model);
                DataContext.Set<Domain.Entities.Subject>().Add(subject);
                DataContext.CommitTransaction();

                response.Data = Mapper.Map<SubjectReadModel>(await DataContext.Set<Domain.Entities.Subject>()
                    .FirstOrDefaultAsync(s => s.Id == subject.Id)
                    .ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to update record {ex.Message} {typeof(Domain.Entities.Subject).Name}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
