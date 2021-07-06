
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using UniversityEnrollmentManager.Data.Context;
using UniversityEnrollmentManager.Domain.Entities;

namespace UniversityEnrollmentManager.Data
{
    //Taken from a personal exploration in to CQRS and setup.
    public abstract class BaseConfiguration<T, TKey> : IEntityConfiguration where T : Entity<TKey>
    {
        protected readonly ModelBuilder modelBuilder;
        public static IEnumerable<System.Security.Claims.ClaimsIdentity> claims;
        protected EntityTypeBuilder<T> builder => modelBuilder.Entity<T>();
        protected BaseConfiguration(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }
        public virtual void Configure(IHttpContextAccessor httpContextAccessor)
        {
            builder.HasKey(t => t.Id);
            // Properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");
        }

        public virtual void Configure(IHttpContextAccessor httpContextAccessor, bool IsDeleted)
        {
            builder.HasKey(t => t.Id);
            // Properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id");
        }
    }
}
