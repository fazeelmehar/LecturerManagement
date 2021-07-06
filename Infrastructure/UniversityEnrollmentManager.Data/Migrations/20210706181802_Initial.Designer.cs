﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UniversityEnrollmentManager.Data.Context;

namespace UniversityEnrollmentManager.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210706181802_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("UniversityEnrollmentManager.Domain.Entities.Enrollment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LectureId");

                    b.Property<int?>("StudentId");

                    b.HasKey("Id");

                    b.HasIndex("LectureId");

                    b.HasIndex("StudentId");

                    b.ToTable("Enrollment");
                });

            modelBuilder.Entity("UniversityEnrollmentManager.Domain.Entities.Lecture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("DateTime");

                    b.Property<TimeSpan>("Duration");

                    b.Property<int?>("LectureTheatreId");

                    b.Property<int?>("SubjectId");

                    b.HasKey("Id");

                    b.HasIndex("LectureTheatreId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Lecture");
                });

            modelBuilder.Entity("UniversityEnrollmentManager.Domain.Entities.LectureTheatre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Location")
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(100)");

                    b.Property<int>("SeatingCapacity");

                    b.HasKey("Id");

                    b.ToTable("LectureTheatre");
                });

            modelBuilder.Entity("UniversityEnrollmentManager.Domain.Entities.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Student");
                });

            modelBuilder.Entity("UniversityEnrollmentManager.Domain.Entities.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("varchar(80)");

                    b.HasKey("Id");

                    b.ToTable("Subject");
                });

            modelBuilder.Entity("UniversityEnrollmentManager.Domain.Entities.Enrollment", b =>
                {
                    b.HasOne("UniversityEnrollmentManager.Domain.Entities.Lecture", "Lecture")
                        .WithMany("Enrollments")
                        .HasForeignKey("LectureId")
                        .HasConstraintName("FK_Enrollments_Lecture_LectureId");

                    b.HasOne("UniversityEnrollmentManager.Domain.Entities.Student", "Student")
                        .WithMany("Enrollments")
                        .HasForeignKey("StudentId")
                        .HasConstraintName("FK_Enrollments_Student_StudentId");
                });

            modelBuilder.Entity("UniversityEnrollmentManager.Domain.Entities.Lecture", b =>
                {
                    b.HasOne("UniversityEnrollmentManager.Domain.Entities.LectureTheatre", "LectureTheatre")
                        .WithMany("Lectures")
                        .HasForeignKey("LectureTheatreId")
                        .HasConstraintName("FK_Lectures_LectureTheatre_LectureTheatreId");

                    b.HasOne("UniversityEnrollmentManager.Domain.Entities.Subject", "Subject")
                        .WithMany("Lectures")
                        .HasForeignKey("SubjectId")
                        .HasConstraintName("FK_Lectures_Subject_SubjectId");
                });
#pragma warning restore 612, 618
        }
    }
}
