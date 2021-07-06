using System;
using AutoMapper;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Core.CQRS.Model;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Student;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.Student.Handler
{
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

                response.Data = Mapper.Map<StudentReadModel>(
                                                            await DataContext.Set<Domain.Entities.Student>()
                                                                             .FirstOrDefaultAsync(s => s.Id == student.Id)
                                                                             .ConfigureAwait(true));
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
