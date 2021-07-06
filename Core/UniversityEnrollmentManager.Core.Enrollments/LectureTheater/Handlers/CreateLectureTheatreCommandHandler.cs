using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Utils.Interfaces;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.LectureTheatre;
using UniversityEnrollmentManager.Utils;

namespace UniversityEnrollmentManager.Core.Enrollments.LectureTheater.Handlers
{
    public class CreateLectureTheatreCommandHandler<TUnitOfWork>
      : DataContextHandlerBase<TUnitOfWork, EntityRequestModel<LectureTheatreCreateModel, EntityResponseModel<LectureTheatreReadModel>>, EntityResponseModel<LectureTheatreReadModel>>
      where TUnitOfWork : IUnitOfWork
    {
        public CreateLectureTheatreCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }
        protected override async Task<EntityResponseModel<LectureTheatreReadModel>> ProcessAsync(EntityRequestModel<LectureTheatreCreateModel, EntityResponseModel<LectureTheatreReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<LectureTheatreReadModel>();
            try
            {
                var lectureTheatre = Mapper.Map<Domain.Entities.LectureTheatre>(request.Model);

                DataContext.Set<Domain.Entities.LectureTheatre>().Add(lectureTheatre);
                DataContext.CommitTransaction();

                response.Data = Mapper.Map<LectureTheatreReadModel>(
                                                            await DataContext.Set<Domain.Entities.LectureTheatre>()
                                                            .FirstOrDefaultAsync(s => s.Id == lectureTheatre.Id)
                                                            .ConfigureAwait(true));
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to update record {typeof(Domain.Entities.LectureTheatre).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
