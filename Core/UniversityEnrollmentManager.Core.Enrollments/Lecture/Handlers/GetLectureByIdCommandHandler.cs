using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Lecture;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace UniversityEnrollmentManager.Core.Enrollments.Lecture.Handlers
{
    public class GetLectureByIdCommandHandler<TUnitOfWork>
         : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>
         where TUnitOfWork : IUnitOfWork
    {
        public GetLectureByIdCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseModel<LectureReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseModel<LectureReadModel>> request, CancellationToken cancellationToken)
        {
            var response = new EntityResponseModel<LectureReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.Lecture>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No lecture found");
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
                response.ReturnMessage.Add($"Unable to update record {typeof(Domain.Entities.Lecture).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
