using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Enrollment;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Enrollment.Handlers
{
    public class GetAllEnrollmentCommandHandler<TUnitOfWork>
       : DataContextHandlerBase<TUnitOfWork, EntityListQuery<EntityResponseListModel<EnrollmentStudentsReadModel>>, EntityResponseListModel<EnrollmentStudentsReadModel>>
       where TUnitOfWork : IUnitOfWork
    {
        public GetAllEnrollmentCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseListModel<EnrollmentStudentsReadModel>> ProcessAsync(EntityListQuery<EntityResponseListModel<EnrollmentStudentsReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseListModel<EnrollmentStudentsReadModel>();

            try
            {
                response.Data = Mapper.Map<List<EnrollmentStudentsReadModel>>(await DataContext.Set<Domain.Entities.Enrollment>()
                    .Include(s => s.Student)
                    .AsNoTracking()
                    .ToListAsync());
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to Update Record {typeof(Domain.Entities.Lecture).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }

    }
}
