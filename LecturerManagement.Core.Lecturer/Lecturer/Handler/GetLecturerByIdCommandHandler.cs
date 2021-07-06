using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LecturerManagement.Core.CQRS.Queries;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Lecture;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LecturerManagement.Core.Lecturer.Lecture.Handler
{
    public class GetLectureByIdCommandHandler<TUnitOfWork>
         : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>
         where TUnitOfWork : IUnitOfWork
    {
        public GetLectureByIdCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<LectureReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseModel<LectureReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<LectureReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.Lecture>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No Lecture Found");
                    response.ReturnStatus = false;
                    return response;
                }
                response.Data = Mapper.Map<LectureReadModel>(await DataContext.Set<Domain.Entities.Lecture>()
                                                                              .Include(s => s.Subject)
                                                                              .Include(s => s.LectureTheatre)
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
