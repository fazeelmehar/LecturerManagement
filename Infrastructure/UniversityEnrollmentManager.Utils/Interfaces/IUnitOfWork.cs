using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniversityEnrollmentManager.Utils.Interfaces
{
    public interface IUnitOfWork<T> : IUnitOfWork where T : DbContext
    {
    }
    public interface IUnitOfWork
    {
        DbSet<T> Set<T>() where T : class;
        void Entry<T>(T entity, EntityState state) where T : class;
        DbSet<T> Query<T>() where T : class;
        void Upsert<Y>(Y entity) where Y : class;
        void Delete<Y>(Y entity) where Y : class;
        void ForceBeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        int SaveChanges();
        Task<int> SaveChanges(CancellationToken cancellationToken);
        Task<int> SaveChangesNoConflict(CancellationToken cancellationToken);
        int SaveChangesNoConflict();
        void SetIsolationLevel(IsolationLevel isolationLevel);
        IEnumerable<string> GetMigrationHistory();
        IEnumerable<string> GetMigrationAssembly();
        void Migrate();
        void DropDatabase();
        string GetSchema(object entry);
    }
}
