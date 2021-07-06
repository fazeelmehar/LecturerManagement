using AutoMapper;
using Intellix.Core.CQRS.Queries;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Subject;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.Subject.Handler
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
                response.ReturnMessage.Add(String.Format("Unable to Update Record {0}" + ex.Message, typeof(Domain.Entities.Subject).Name));
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
