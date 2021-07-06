
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using LecturerManagement.Data.Interface;
using LecturerManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace LecturerManagement.Data
{
    public abstract class BaseConfiguration<T,TKey> : IEntityConfiguration where T : Entity<TKey>
    {
        protected readonly ModelBuilder _modelBuilder;
        public static IEnumerable<System.Security.Claims.ClaimsIdentity> _claims;
        protected EntityTypeBuilder<T> _builder => _modelBuilder.Entity<T>();        
        protected BaseConfiguration(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }
        public virtual void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _builder.HasKey(t =>  t.Id);
            // Properties
            _builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");
            _builder.HasQueryFilter(t => !t.IsDeleted);     
           
        }

        public virtual void Configure(IHttpContextAccessor httpContextAccessor, bool IsDeleted)
        {
            _builder.HasKey(t => t.Id);
            // Properties
            _builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");
            if(IsDeleted) _builder.HasQueryFilter(t => !t.IsDeleted);

        }
    }
}
