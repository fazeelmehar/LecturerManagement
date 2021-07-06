using LecturerManagement.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace LecturerManagement.Data.Configuration
{
    public class SubjectConfiguration : BaseConfiguration<Subject, int>
    {
        public SubjectConfiguration(ModelBuilder modelBuilder) : base(modelBuilder)
        {

        }
        public override void Configure(IHttpContextAccessor httpContextAccessor)
        {
            base.Configure(httpContextAccessor);
            _builder.Property(p => p.Name)
                .HasColumnType("varchar(80)");

            _builder.Property(p => p.IsDeleted)
                  .HasDefaultValue(false);

            //table
            _builder.ToTable("tblSubject");
        }
    }
}
