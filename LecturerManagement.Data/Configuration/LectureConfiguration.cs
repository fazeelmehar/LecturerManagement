using LecturerManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LecturerManagement.Data.Configuration
{
    public class LectureConfiguration : BaseConfiguration<Lecture, int>
    {
        public LectureConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);

            _builder.Property(p => p.IsDeleted)
                  .HasDefaultValue(false);

            //table
            _builder.ToTable("tblLecture");

            //relations
            _builder.HasOne(x => x.Subject)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.SubjectId)
                .HasConstraintName("FK_Lectures_Subject_SubjectId");

            _builder.HasOne(x => x.LectureTheatre)
                .WithMany(x => x.Lectures)
                .HasForeignKey(x => x.LectureTheatreId)
                .HasConstraintName("FK_Lectures_LectureTheatre_LectureTheatreId");
        }
    }

}
