using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentManager.Domain.Entities;

namespace UniversityEnrollmentManager.Data.Configuration
{
    public class LectureConfiguration : BaseConfiguration<Lecture, int>
    {
        public LectureConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);

            builder.ToTable("Lecture");

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.SubjectId)
                .HasConstraintName("FK_Lectures_Subject_SubjectId");

            builder.HasOne(x => x.LectureTheatre)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.LectureTheatreId)
                .HasConstraintName("FK_Lectures_LectureTheatre_LectureTheatreId");
        }
    }
}
