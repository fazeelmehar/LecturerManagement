using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniversityEnrollmentManager.Domain.Entities;

namespace UniversityEnrollmentManager.Data.Configuration
{
    public class EnrollmentConfiguration : BaseConfiguration<Enrollment, int>
    {
        public EnrollmentConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);

            builder.ToTable("Enrollment");

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_Enrollments_Student_StudentId");

            builder.HasOne(x => x.Lecture)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.LectureId)
                .HasConstraintName("FK_Enrollments_Lecture_LectureId");
        }
    }
}
