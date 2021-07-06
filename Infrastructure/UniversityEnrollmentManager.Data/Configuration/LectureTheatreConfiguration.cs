using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentManager.Domain.Entities;

namespace UniversityEnrollmentManager.Data.Configuration
{
    public class LectureTheatreConfiguration : BaseConfiguration<LectureTheatre, int>
    {
        public LectureTheatreConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);
            builder.Property(p => p.Name)
                .HasColumnType("varchar(100)");

            builder.Property(p => p.Location)
                .HasColumnType("varchar(250)");

            builder.ToTable("LectureTheatre");
        }
    }
}
