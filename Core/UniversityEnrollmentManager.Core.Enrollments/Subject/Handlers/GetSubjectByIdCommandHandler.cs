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
    public class GetSubjectByIdCommandHandler<TUnitOfWork>
        : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseModel<SubjectReadModel>>, EntityResponseModel<SubjectReadModel>>
        where TUnitOfWork : IUnitOfWork
    {
        public GetSubjectByIdCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<SubjectReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseModel<SubjectReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<SubjectReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.Subject>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No Subject Found");
                    response.ReturnStatus = false;
                    return response;
                }
                response.Data = Mapper.Map<SubjectReadModel>(await DataContext.Set<Domain.Entities.Subject>().Where(s => s.Id == request.Id).FirstOrDefaultAsync());
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
