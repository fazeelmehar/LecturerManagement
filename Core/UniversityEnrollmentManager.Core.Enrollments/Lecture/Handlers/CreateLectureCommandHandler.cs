using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Lecture;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Lecture.Handlers
{
    public class CreateLectureCommandHandler<TUnitOfWork>
       : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>
       where TUnitOfWork : IUnitOfWork
    {
        public CreateLectureCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<LectureReadModel>> ProcessAsync(EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<LectureReadModel>();
            try
            {
                var lecture = Mapper.Map<Domain.Entities.Lecture>(request.Model);

                // Check lecture theatre exists
                if (!request.Model.LectureTheatreId.HasValue ||
                   !DataContext.Set<Domain.Entities.LectureTheatre>().Any(s => s.Id == request.Model.LectureTheatreId))
                {
                    response.ReturnMessage.Add("No lecture theatre found");
                    response.ReturnStatus = false;
                    return response;
                }
                // Check subject exists
                if (!request.Model.SubjectId.HasValue ||
                   !DataContext.Set<Domain.Entities.Subject>().Any(s => s.Id == request.Model.SubjectId))
                {
                    response.ReturnMessage.Add("No subject found");
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
                response.ReturnMessage.Add($"Unable to update record {typeof(Domain.Entities.Lecture).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
