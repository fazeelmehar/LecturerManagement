using MediatR;
using LecturerManagement.Core.CQRS.Queries;
using LecturerManagement.Core.CQRS.Model;
using LecturerManagement.Core.Lecturer.LectureTheatre.Handler;
using LecturerManagement.Core.Lecturer.Student.Handler;
using LecturerManagement.Core.Lecturer.Subject.Handler;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.DomainModel.Lecture;
using LecturerManagement.DomainModel.LectureTheatre;
using LecturerManagement.DomainModel.Student;
using LecturerManagement.DomainModel.Subject;
using LecturerManagement.Utility.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using LecturerManagement.Core.Lecturer.Lecture.Handler;
using LecturerManagement.DomainModel.Enrollment;
using LecturerManagement.Core.Lecturer.Enrollment.Handler;

namespace LecturerManagement.Core.Lecturer
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
            serviceCollection.TryAddTransient<IRequestHandler<EntityRequestModel<LectureCreateModel, EntityResponseModel<LectureReadModel>>, EntityResponseModel<LectureReadModel>>, CreateLecturerCommandHandler<IUnitOfWork>>();
            #endregion

            #region Enrollment
            serviceCollection.TryAddTransient<IRequestHandler<EntityIdentifierQuery<int, EntityResponseModel<EnrollmentReadModel>>, EntityResponseModel<EnrollmentReadModel>>, GetEnrollmentByIdCommandHandler<IUnitOfWork>>();
            serviceCollection.TryAddTransient<IRequestHandler<EntityRequestModel<EnrollmentCreateModel, EntityResponseModel<EnrollmentReadModel>>, EntityResponseModel<EnrollmentReadModel>>, CreateEnrollmentCommandHandler<IUnitOfWork>>();
            #endregion

            return serviceCollection;
        }
    }

}




