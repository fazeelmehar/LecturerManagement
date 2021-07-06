using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentManager.Domain.Entities;

namespace UniversityEnrollmentManager.Data.Configuration
{
    public class SubjectConfiguration : BaseConfiguration<Subject, int>
    {
        public SubjectConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {
        }

        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);
            builder.Property(p => p.Name)
                .HasColumnType("varchar(80)");

            builder.ToTable("Subject");
        }
    }
}
