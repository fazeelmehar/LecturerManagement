using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
    public class CreateStudentCommandHandler<TUnitOfWork>
        : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<StudentCreateModel, EntityResponseModel<StudentReadModel>>, EntityResponseModel<StudentReadModel>>
        where TUnitOfWork : IUnitOfWork
    {
        public CreateStudentCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }
        protected override async Task<EntityResponseModel<StudentReadModel>> ProcessAsync(EntityRequestModel<StudentCreateModel, EntityResponseModel<StudentReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<StudentReadModel>();
            try
            {
                var student = Mapper.Map<Domain.Entities.Student>(request.Model);

                DataContext.Set<Domain.Entities.Student>().Add(student);
                DataContext.CommitTransaction();

                response.Data = Mapper.Map<StudentReadModel>(await DataContext.Set<Domain.Entities.Student>()
                    .FirstOrDefaultAsync(s => s.Id == student.Id)
                    .ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to update record {typeof(Domain.Entities.Student).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
