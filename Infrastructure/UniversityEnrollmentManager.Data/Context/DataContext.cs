using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using System.Reflection;

namespace UniversityEnrollmentManager.Data.Context
{
    public interface IEntityConfiguration
    {
        void Configure(IHttpContextAccessor httpContextAccessor);
    }

    public class DataContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        public DataContext(DbContextOptions contextOptions,
           IHttpContextAccessor httpContextAccessor) : base(contextOptions)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GetType().Assembly.GetTypes()
                                .Where(t => !t.GetTypeInfo().IsAbstract && t.GetInterfaces().Contains(typeof(IEntityConfiguration)))
                                .ToList()
                                .ForEach(t =>
                                {
                                    ((IEntityConfiguration)Activator.CreateInstance(t, new[] { modelBuilder })).Configure(httpContextAccessor);
                                });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.ConfigureWarnings(w =>
                  w.Throw(RelationalEventId.QueryClientEvaluationWarning));
#endif
            base.OnConfiguring(optionsBuilder);
        }
    }
}
