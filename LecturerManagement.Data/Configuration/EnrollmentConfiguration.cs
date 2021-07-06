using LecturerManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LecturerManagement.Data.Configuration
{
    public class EnrollmentConfiguration : BaseConfiguration<Enrollment, int>
    {
        public EnrollmentConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);

            _builder.Property(p => p.IsDeleted)
                  .HasDefaultValue(false);

            //table
            _builder.ToTable("tblEnrollment");

            //relations
            _builder.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_Enrollments_Student_StudentId");

            _builder.HasOne(x => x.Lecture)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.LectureId)
                .HasConstraintName("FK_Enrollments_Lecture_LectureId");
        }
    }
}
