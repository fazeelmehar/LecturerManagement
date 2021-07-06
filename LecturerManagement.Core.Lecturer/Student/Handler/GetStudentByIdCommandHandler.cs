using System; 
using AutoMapper;
using LecturerManagement.Core.CQRS.Queries;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Student;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.Student.Handler
{
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
                response.Data = Mapper.Map<StudentReadModel>(await DataContext.Set<Domain.Entities.Student>().Where(s => s.Id == request.Id).FirstOrDefaultAsync());
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
