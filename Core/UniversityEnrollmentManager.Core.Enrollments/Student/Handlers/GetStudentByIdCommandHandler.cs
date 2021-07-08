using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Student.Handlers
{
    // Overall approach taken from researching Mediatr and CQRS patterns
    // During self-directed learning
    public class GetStudentByIdCommandHandler<TUnitOfWork>
          : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseModel<StudentReadModel>>, EntityResponseModel<StudentReadModel>>
          where TUnitOfWork : IUnitOfWork
    {
        public GetStudentByIdCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<StudentReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseModel<StudentReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<StudentReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.Student>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No Student Found");
                    response.ReturnStatus = false;
                    return response;
                }
                response.Data = Mapper.Map<StudentReadModel>(
                    await DataContext.Set<Domain.Entities.Student>()
                                     .Include(s => s.Enrollments)
                                     .Include(s => s.Enrollments).ThenInclude(s => s.Subject)
                                     .Where(s => s.Id == request.Id)
                                     .FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add(String.Format("Unable to Update Record {0}" + ex.Message, typeof(Domain.Entities.Student).Name));
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}