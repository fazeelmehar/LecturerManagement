using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Core.Enrollments.RequestHandlers
{
    // Overall approach taken from researching Mediatr and CQRS patterns
    // During self-directed learning
    public abstract class DataContextHandlerBase<TUnitOfWork, TRequest, TResponse>
       : RequestHandlerBase<TRequest, TResponse>
       where TUnitOfWork : IUnitOfWork
       where TRequest : IRequest<TResponse>
    {
        protected DataContextHandlerBase(ILoggerFactory loggerFactory, TUnitOfWork dataContext, IMapper mapper)
            : base(loggerFactory)
        {
            Assert.NotNull(loggerFactory, nameof(loggerFactory));
            Assert.NotNull(mapper, nameof(mapper));
            DataContext = dataContext;
            Mapper = mapper;
        }
        protected TUnitOfWork DataContext { get; }

        protected IMapper Mapper { get; }
    }
}
