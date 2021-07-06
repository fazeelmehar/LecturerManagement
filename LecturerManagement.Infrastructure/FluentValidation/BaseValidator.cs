using FluentValidation;
using LecturerManagement.Domain.Interfaces;
using LecturerManagement.Utility;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LecturerManagement.Infrastructure.FluentValidation
{
    public abstract class BaseValidator<T,TEntity,TKey>: AbstractValidator<T>
        where TEntity: Domain.Entities.Entity<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;
        protected DbSet<TEntity> _Entities;
        protected IQueryable<TEntity> _EntitiesNoTracking => _Entities.AsNoTracking();

        protected BaseValidator()
        {
            _Entities = _unitOfWork.Set<TEntity>();

        }

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            Assert.NotNull(unitOfWork, nameof(unitOfWork));
            _unitOfWork = unitOfWork;
            _Entities = _unitOfWork.Set<TEntity>();
            
        }
    }
}
