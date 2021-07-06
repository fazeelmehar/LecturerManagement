using AutoMapper;
using Intellix.Core.CQRS.Handlers;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.Utility;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LecturerManagement.Core.CQRS.Core.Handlers
{
    /// <summary></summary>
    /// <typeparam name="TUnitOfWork">The type of the unit of work.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    /// <seealso cref="LecturerManagement.Core.CQRS.Handlers.RequestHandlerBase{TRequest, TResponse}" />
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
