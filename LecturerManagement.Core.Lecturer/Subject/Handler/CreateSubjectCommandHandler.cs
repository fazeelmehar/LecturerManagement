using System;
using AutoMapper;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Core.CQRS.Model;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Subject;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.Subject.Handler
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

                response.Data = Mapper.Map<SubjectReadModel>(
                                                            await DataContext.Set<Domain.Entities.Subject>()
                                                                             .FirstOrDefaultAsync(s => s.Id == subject.Id)
                                                                             .ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add(String.Format("Unable to Update Record {0}" + ex.Message, typeof(Domain.Entities.Subject).Name));
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
