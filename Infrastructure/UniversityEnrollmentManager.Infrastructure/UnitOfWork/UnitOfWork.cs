using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityEnrollmentManager.Data.Context;
using UniversityEnrollmentManager.Utils.Interfaces;

namespace UniversityEnrollmentManager.Infrastructure.UnitOfWork
{
    // UOW concept taken from personal project looking in to CQRS.
    public class UnitOfWork<T> : IUnitOfWork<T> where T : DataContext
    {
        protected readonly T Context;
        private IDbContextTransaction transaction;
        private IsolationLevel? isolationLevel;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UnitOfWork(T dbContext, IHttpContextAccessor httpContextAccessor)
        {
            Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public UnitOfWork(T dbContext)
        {
            Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        private void StartNewTransactionIfNeeded()
        {
            if (transaction == null)
            {
                transaction = isolationLevel.HasValue ? Context.Database.BeginTransaction(isolationLevel.GetValueOrDefault()) : Context.Database.BeginTransaction();
            }
        }

        public DbSet<Y> Set<Y>() where Y : class
        {
            return Context.Set<Y>();
        }


        public void Entry<T1>(T1 entity, EntityState state) where T1 : class
        {
            Context.Entry(entity).State = state;
        }

        public DbSet<Y> Query<Y>() where Y : class
        {
            return Context.Set<Y>();
        }

        public void ForceBeginTransaction() => StartNewTransactionIfNeeded();

        public void CommitTransaction()
        {
            Context.SaveChanges();
            if (transaction == null) return;
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }

        public void Upsert<Y>(Y entity) where Y : class
        {
            Context.ChangeTracker.TrackGraph(entity, e =>
            {
                e.Entry.State = e.Entry.IsKeySet ? EntityState.Modified : EntityState.Added;
            });
        }

        public void Delete<Y>(Y entity) where Y : class
        {
            Context.ChangeTracker.TrackGraph(entity, e =>
            {
                e.Entry.CurrentValues["IsDeleted"] = true;
                e.Entry.State = EntityState.Deleted;
            });
        }

        public void RollbackTransaction()
        {
            if (transaction == null) return;
            transaction.Rollback();
            transaction.Dispose();
            transaction = null;
        }
        public int SaveChanges()
        {
            StartNewTransactionIfNeeded();
            OnBeforeSaving();
            var result = Context.SaveChanges();
            return result;
        }
        public async Task<int> SaveChangesNoConflict(CancellationToken cancellationToken)
        {
            try
            {
                OnBeforeSaving();
                var result = await Context.SaveChangesAsync(cancellationToken);
                return result;

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> SaveChanges(CancellationToken cancellationToken)
        {
            try
            {
                StartNewTransactionIfNeeded();
                OnBeforeSaving();
                var result = Context.SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SetIsolationLevel(IsolationLevel isolationLevel) => this.isolationLevel = isolationLevel;
        
        void OnBeforeSaving()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        // Not handling deleted.
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Modified:
                        break;
                }
            }
        }

        public void Dispose()
        {
            transaction?.Dispose();
            transaction = null;
        }
        public IEnumerable<string> GetMigrationHistory()
        {
            return Context.GetService<IHistoryRepository>()
               .GetAppliedMigrations()
               .Select(m => m.MigrationId);
        }
        public string GetSchema(object entry)
        {
            var schemaAnnotation = Context.Model.FindEntityType(entry.GetType()).GetAnnotations()
            .FirstOrDefault(a => a.Name == "Relational:Schema");
            return schemaAnnotation == null ? "dbo" : schemaAnnotation.Value.ToString();
        }

        public IEnumerable<string> GetMigrationAssembly()
        {
            return Context.GetService<IMigrationsAssembly>()
               .Migrations
               .Select(m => m.Key);
        }
        public void Migrate()
        {
            Context.Database.Migrate();
        }

        public void DropDatabase()
        {
            Context.Database.EnsureDeleted();
        }

        public int SaveChangesNoConflict()
        {
            return Context.SaveChanges();
        }
    }
}
