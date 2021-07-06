using LecturerManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LecturerManagement.Data.Configuration
{
    public class LectureTheatreConfiguration : BaseConfiguration<LectureTheatre, int>
    {
        public LectureTheatreConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);
            _builder.Property(p => p.Name)
                .HasColumnType("varchar(100)");

            _builder.Property(p => p.Location)
                .HasColumnType("varchar(250)");

            _builder.Property(p => p.IsDeleted)
                  .HasDefaultValue(false);

            //table
            _builder.ToTable("tblLectureTheatre");
        }
    }

}
