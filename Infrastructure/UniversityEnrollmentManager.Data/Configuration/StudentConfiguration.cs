using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentManager.Domain.Entities;

namespace UniversityEnrollmentManager.Data.Configuration
{
    public class StudentConfiguration : BaseConfiguration<Student, int>
    {
        public StudentConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);

            builder.Property(p => p.FirstName)
                .HasColumnType("varchar(255)");
            builder.Property(p => p.LastName)
                .HasColumnType("varchar(255)");

            builder.ToTable("Student");
        }
    }
}
