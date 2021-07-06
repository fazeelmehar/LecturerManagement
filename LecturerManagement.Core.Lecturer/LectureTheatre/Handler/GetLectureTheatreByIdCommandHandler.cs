using System; 
using AutoMapper;
using Intellix.Core.CQRS.Queries;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.LectureTheatre;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.LectureTheatre.Handler
{
    public class GetLectureTheatreByIdCommandHandler<TUnitOfWork>
         : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseModel<LectureTheatreReadModel>>, EntityResponseModel<LectureTheatreReadModel>>
         where TUnitOfWork : IUnitOfWork
    {
        public GetLectureTheatreByIdCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<LectureTheatreReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseModel<LectureTheatreReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<LectureTheatreReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.LectureTheatre>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No LectureTheatre Found");
                    response.ReturnStatus = false;
                    return response;
                }
                response.Data = Mapper.Map<LectureTheatreReadModel>(await DataContext.Set<Domain.Entities.LectureTheatre>().Where(s => s.Id == request.Id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add(String.Format("Unable to Update Record {0}" + ex.Message, typeof(Domain.Entities.LectureTheatre).Name));
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
