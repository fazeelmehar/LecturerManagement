using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.DomainModel.Subject;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.Subject.Handlers
{
    public class GetAllStudentsEnrolledInSubjectCommandHandler<TUnitOfWork>
        : DataContextHandlerBase<TUnitOfWork, EntityIdentifierQuery<int, EntityResponseListModel<SubjectEnrollmentsReadModel>>, EntityResponseListModel<SubjectEnrollmentsReadModel>>
        where TUnitOfWork : IUnitOfWork
    {
        public GetAllStudentsEnrolledInSubjectCommandHandler(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper) : base(loggerFactory, dataContext, mapper)
        {
        }

        protected override async Task<EntityResponseListModel<SubjectEnrollmentsReadModel>> ProcessAsync(EntityIdentifierQuery<int, EntityResponseListModel<SubjectEnrollmentsReadModel>> request, CancellationToken cancellationToken)
        {
            // TO DO - This is not complete
            var response = new EntityResponseListModel<SubjectEnrollmentsReadModel>();

            try
            {
                if (!DataContext.Set<Domain.Entities.Subject>().Any(s => s.Id == request.Id))
                {
                    response.ReturnMessage.Add("No Subject Found");
                    response.ReturnStatus = false;
                    return response;
                }

                response.Data = await DataContext.Set<Domain.Entities.Subject>()
                                      .Include(s => s.Enrollments)
                                      .Include(s => s.Enrollments).ThenInclude(s => s.Student)
                                      .Where(s => s.Id == request.Id)
                                      .Select(s => new SubjectEnrollmentsReadModel
                                      {
                                          Id = s.Id,
                                          Name = s.Name,
                                          Students = Mapper.Map<List<StudentReadModel>>(s.Enrollments.Where(m => m.SubjectId == s.Id).Select(s => s.Student).ToList())
                                      })
                                      .AsNoTracking()
                                      .ToListAsync();

            }
            catch (Exception ex)
            {
                response.ReturnMessage.Add($"Unable to Get Record {typeof(Domain.Entities.Subject).Name} {ex.Message}");
                response.ReturnStatus = false;
                DataContext.RollbackTransaction();
            }
            return response;
        }
    }
}
