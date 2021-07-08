using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Enrollment;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Enrollment.Handlers
{
    public class GetEnrollmentByIdCommandHandler<TUnitOfWork>
          : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseModel<EnrollmentReadModel>>, EntityResponseModel<EnrollmentReadModel>>
          where TUnitOfWork : IUnitOfWork
    {
        public GetEnrollmentByIdCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }
        protected override async Task<EntityResponseModel<EnrollmentReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseModel<EnrollmentReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<EnrollmentReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.Enrollment>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No Enrollment Found");
                    response.ReturnStatus = false;
                    return response;
                }
                response.Data = Mapper.Map<EnrollmentReadModel>(await DataContext.Set<Domain.Entities.Enrollment>()
                                                                                 .Include(s => s.Student)
                                                                                 .Include(s => s.Subject)
                                                                                 .Where(s => s.Id == request.Id)
                                                                                 .FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to Get Record {typeof(Domain.Entities.Enrollment).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
