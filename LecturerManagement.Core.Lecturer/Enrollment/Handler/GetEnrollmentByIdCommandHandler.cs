using AutoMapper;
using LecturerManagement.Core.CQRS.Queries;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Enrollment;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.Enrollment.Handler
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
                                                                              .Include(s => s.Lecture)
                                                                              .Where(s => s.Id == request.Id)
                                                                              .FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add(String.Format("Unable to Update Record {0}" + ex.Message, typeof(Domain.Entities.Lecture).Name));
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
