using System;
using AutoMapper;
using LecturerManagement.Core.CQRS.Core.Handlers;
using LecturerManagement.Core.CQRS.Model;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Lecture;
using LecturerManagement.Utility.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LecturerManagement.Core.Lecturer.Lecture.Handler
{
    public class CreateLecturerCommandHandler<TUnitOfWork>
       : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>
       where TUnitOfWork : IUnitOfWork
    {
        public CreateLecturerCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<LectureReadModel>> ProcessAsync(EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<LectureReadModel>();
            try
            {
                var lecture = Mapper.Map<Domain.Entities.Lecture>(request.Model);

                // check LectureTheatre exsit
                if (!request.Model.LectureTheatreId.HasValue ||
                   !DataContext.Set<Domain.Entities.LectureTheatre>().Any(s => s.Id == request.Model.LectureTheatreId))
                {
                    response.ReturnMessage.Add("No Lecture Theatre Found");
                    response.ReturnStatus = false;
                    return response;
                }
                // check subject exsit
                if (!request.Model.SubjectId.HasValue ||
                   !DataContext.Set<Domain.Entities.Subject>().Any(s => s.Id == request.Model.SubjectId))
                {
                    response.ReturnMessage.Add("No Subject Found");
                    response.ReturnStatus = false;
                    return response;
                }

                DataContext.Set<Domain.Entities.Lecture>().Add(lecture);
                DataContext.CommitTransaction();

                response.Data = Mapper.Map<LectureReadModel>(
                                                            await DataContext.Set<Domain.Entities.Lecture>()
                                                            .Include(s => s.Subject)
                                                            .Include(s => s.LectureTheatre)
                                                            .FirstOrDefaultAsync(s => s.Id == lecture.Id)
                                                            .ConfigureAwait(true));
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
