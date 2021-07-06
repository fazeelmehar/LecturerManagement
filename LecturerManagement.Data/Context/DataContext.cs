using LecturerManagement.Data.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Linq;
using System.Reflection;


namespace LecturerManagement.Data.Context
{
    public class DataContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataContext(DbContextOptions contextOptions,
           IHttpContextAccessor httpContextAccessor) : base(contextOptions)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public DataContext(DbContextOptions contextOptions) : base(contextOptions)
        { }

        protected override void OnModelCreating(ModelBuilder _modelBuilder)
        {
            GetType().Assembly.GetTypes()
                                .Where(t => !t.GetTypeInfo().IsAbstract && t.GetInterfaces().Contains(typeof(IEntityConfiguration)))
                                .ToList()
                                .ForEach(t =>
                                {
                                    ((IEntityConfiguration)Activator.CreateInstance(t, new[] { _modelBuilder })).Configure(_httpContextAccessor);
                                });

            //restrict cascade delete
            var cascadeFKs = _modelBuilder.Model.GetEntityTypes()
                                        .SelectMany(t => t.GetForeignKeys())
                                        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

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
