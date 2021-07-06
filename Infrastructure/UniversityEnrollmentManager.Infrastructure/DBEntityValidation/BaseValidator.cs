using System;
using FluentValidation;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentManager.Utils;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Infrastructure.DBEntityValidation
{
    // Validation concept taken from personal project looking in to CQRS.
    public abstract class BaseValidator<T, TEntity, TKey> : AbstractValidator<T>
        where TEntity : Domain.Entities.Entity<TKey>
    {
        readonly IUnitOfWork unitOfWork;
        protected DbSet<TEntity> entities;
        protected IQueryable<TEntity> entitiesNoTracking => entities.AsNoTracking();

        protected BaseValidator()
        {
            entities = unitOfWork.Set<TEntity>();
        }

        protected BaseValidator(IUnitOfWork unitOfWork)
        {
            Assert.NotNull(unitOfWork, nameof(unitOfWork));
            this.unitOfWork = unitOfWork;
            entities = unitOfWork.Set<TEntity>();
        }
    }
}
