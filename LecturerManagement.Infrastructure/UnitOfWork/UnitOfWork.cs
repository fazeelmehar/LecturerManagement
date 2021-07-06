using LecturerManagement.Data.Context;
using LecturerManagement.Data.Extensions;
using LecturerManagement.Domain.Interfaces;
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


namespace LecturerManagement.Infrastructure.UnitOfWork
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : DataContext
    {
       
        protected readonly T Context;
        private IDbContextTransaction _transaction;
        private IsolationLevel? _isolationLevel;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UnitOfWork(T dbContext, IHttpContextAccessor httpContextAccessor)
        {
            Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }
        public UnitOfWork(T dbContext)
        {
            Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }
        private void StartNewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                _transaction = _isolationLevel.HasValue ? Context.Database.BeginTransaction(_isolationLevel.GetValueOrDefault()) : Context.Database.BeginTransaction();
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

        public DbQuery<Y> Query<Y>() where Y : class
        {
            return Context.Query<Y>();
        }

        public void ForceBeginTransaction() => StartNewTransactionIfNeeded();

        public void CommitTransaction()
        {
            Context.SaveChanges();
            if (_transaction == null) return;
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
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
            if (_transaction == null) return;
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
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
        public void SetIsolationLevel(IsolationLevel isolationLevel) => _isolationLevel = isolationLevel;
        private void OnBeforeSaving()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        foreach (var navigationEntry in entry.Navigations.Where(n => !n.Metadata.IsDependentToPrincipal()))
                        {
                            if (navigationEntry is CollectionEntry collectionEntry)
                            {
                                var currVal = collectionEntry.CurrentValue;                               
                                if (currVal != null)
                                {
                                    foreach (var dependentEntry in currVal)
                                    {
                                        if (dependentEntry != null)
                                        {
                                            HandleDependent(Context.Entry(dependentEntry));
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var dependentEntry = navigationEntry.CurrentValue;
                                if (dependentEntry != null)
                                {
                                    HandleDependent(Context.Entry(dependentEntry));
                                }
                            }
                        }
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

        private void HandleDependent(EntityEntry entry)
        {
            entry.CurrentValues["IsDeleted"] = true;
        }
        public void Dispose()
        {
            _transaction?.Dispose();
            _transaction = null;
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

        public List<T1> ExecuteSP<T1>(List<KeyValuePair<string, string>> param, string spName) where T1 : class
        {
          
            var result = new List<T1>();
            var res = Context.LoadStoredProc(spName);
            foreach (var (key, value) in param)
            {
                res.WithSqlParam(key, value);
            }
            res.ExecuteStoredProc((handler) =>
            {
                result = handler.ReadToList<T1>().ToList();
            });

            return result;
        }

        public List<T1> SPExecute<T1>(object[] parameters, string spName) where T1 : class
        {
            var result = new List<T1>();
             Context.Database.ExecuteSqlCommand(spName, parameters);
            return result;
        }

        public int SaveChangesNoConflict()
        {
            return Context.SaveChanges(); 
        }
    }
}
