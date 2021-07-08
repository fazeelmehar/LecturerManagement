using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
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

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.SubjectId)
                .HasConstraintName("FK_Enrollments_Subject_SubjectId");

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_Enrollments_Student_StudentId");
        }
    }
}
