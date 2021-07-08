using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Subject;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Subject.Handlers
{
    public class GetAllStudentsEnrolledInSubjectCommandHandler<TUnitOfWork>
        : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseModel<SubjectEnrollmentsReadModel>>, EntityResponseModel<SubjectEnrollmentsReadModel>>
        where TUnitOfWork : IUnitOfWork
    {
        public GetAllStudentsEnrolledInSubjectCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<SubjectEnrollmentsReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseModel<SubjectEnrollmentsReadModel>> request, CancellationToken cancellationToken)
        {
            // TO DO - This is not complete
            var response = new EntityResponseModel<SubjectEnrollmentsReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.Subject>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No Subject Found");
                    response.ReturnStatus = false;
                    return response;
                }
                response.Data = Mapper.Map<SubjectEnrollmentsReadModel>(await DataContext.Set<Domain.Entities.Subject>().Where(s => s.Id == request.Id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to Update Record {typeof(Domain.Entities.Subject).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
