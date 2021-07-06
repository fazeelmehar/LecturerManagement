using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UniversityEnrollmentManager.Core.Enrollments.Enrollment.Handlers;
using UniversityEnrollmentManager.Core.Enrollments.Lecture.Handlers;
using UniversityEnrollmentManager.Core.Enrollments.LectureTheater.Handlers;
using UniversityEnrollmentManager.Core.Enrollments.RequestHandlers;
using UniversityEnrollmentManager.Core.Enrollments.Student.Handlers;
using UniversityEnrollmentManager.Core.Enrollments.Subject.Handlers;
using UniversityEnrollmentManager.DomainModel.Enrollment;
using UniversityEnrollmentManager.DomainModel.Lecture;
using UniversityEnrollmentManager.DomainModel.LectureTheatre;
using UniversityEnrollmentManager.DomainModel.Student;
using UniversityEnrollmentManager.DomainModel.Subject;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterHandlers(this IServiceCollection serviceCollection)
        {
            #region Student
            serviceCollection.TryAddTransient<IRequestHandler<EntityIdentifierQuery<int, EntityResponseModel<StudentReadModel>>, EntityResponseModel<StudentReadModel>>, GetStudentByIdCommandHandler<IUnitOfWork>>();
            serviceCollection.TryAddTransient<IRequestHandler<EntityRequestModel<StudentCreateModel, EntityResponseModel<StudentReadModel>>, EntityResponseModel<StudentReadModel>>, CreateStudentCommandHandler<IUnitOfWork>>();
            #endregion

            #region Subject
            serviceCollection.TryAddTransient<IRequestHandler<EntityIdentifierQuery<int, EntityResponseModel<SubjectReadModel>>, EntityResponseModel<SubjectReadModel>>, GetSubjectByIdCommandHandler<IUnitOfWork>>();
            serviceCollection.TryAddTransient<IRequestHandler<EntityRequestModel<SubjectCreateModel, EntityResponseModel<SubjectReadModel>>, EntityResponseModel<SubjectReadModel>>, CreateSubjectCommandHandler<IUnitOfWork>>();
            #endregion

            #region LectureTheatre
            serviceCollection.TryAddTransient<IRequestHandler<EntityIdentifierQuery<int, EntityResponseModel<LectureTheatreReadModel>>, EntityResponseModel<LectureTheatreReadModel>>, GetLectureTheatreByIdCommandHandler<IUnitOfWork>>();
            serviceCollection.TryAddTransient<IRequestHandler<EntityRequestModel<LectureTheatreCreateModel, EntityResponseModel<LectureTheatreReadModel>>, EntityResponseModel<LectureTheatreReadModel>>, CreateLectureTheatreCommandHandler<IUnitOfWork>>();
            #endregion

            #region Lecture
            serviceCollection.TryAddTransient<IRequestHandler<EntityIdentifierQuery<int, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>, GetLectureByIdCommandHandler<IUnitOfWork>>();
            serviceCollection.TryAddTransient<IRequestHandler<EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>, CreateLectureCommandHandler<IUnitOfWork>>();
            #endregion

            #region Enrollment
            serviceCollection.TryAddTransient<IRequestHandler<EntityIdentifierQuery<int, EntityResponseModel<EnrollmentReadModel>>, EntityResponseModel<EnrollmentReadModel>>, GetEnrollmentByIdCommandHandler<IUnitOfWork>>();
            serviceCollection.TryAddTransient<IRequestHandler<EntityRequestModel<EnrollmentCreateModel, EntityResponseModel<EnrollmentReadModel>>, EntityResponseModel<EnrollmentReadModel>>, CreateEnrollmentCommandHandler<IUnitOfWork>>();
            #endregion

            return serviceCollection;
        }
    }
}
