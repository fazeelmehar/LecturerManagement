using LecturerManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LecturerManagement.Data.Configuration
{
   public class StudentConfiguration : BaseConfiguration<Student, int>
    {
        public StudentConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);

            _builder.Property(p => p.FirstName)
                .HasColumnType("varchar(100)");
            _builder.Property(p => p.LastName)
                .HasColumnType("varchar(100)");

            _builder.Property(p => p.IsDeleted)
                  .HasDefaultValue(false);

            //table
            _builder.ToTable("tblStudent");
        }

    }
}
