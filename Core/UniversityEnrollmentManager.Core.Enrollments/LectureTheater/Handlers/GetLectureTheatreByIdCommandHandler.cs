using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Utils.Interfaces;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.DomainModel.LectureTheatre;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;

namespace UniversityEnrollmentManager.Core.Enrollments.LectureTheater.Handlers
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
                    response.ReturnMessage.Add("No Lecture Theatre Found");
                    response.ReturnStatus = false;
                    return response;
                }
                response.Data = Mapper.Map<LectureTheatreReadModel>(await DataContext.Set<Domain.Entities.LectureTheatre>().Where(s => s.Id == request.Id).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to Update Record {typeof(Domain.Entities.LectureTheatre).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}