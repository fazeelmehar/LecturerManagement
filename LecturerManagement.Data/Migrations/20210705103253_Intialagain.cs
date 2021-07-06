using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LecturerManagement.Data.Migrations
{
    public partial class Intialagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "tblStudent");

            migrationBuilder.DropColumn(
                name: "RollNumber",
                table: "tblStudent");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "tblStudent",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "tblStudent",
                type: "varchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "tblLectureTheatre",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Location = table.Column<string>(type: "varchar(250)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLectureTheatre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblSubject",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "varchar(80)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSubject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblLecture",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    DateTime = table.Column<DateTimeOffset>(nullable: false),
                    SubjectId = table.Column<int>(nullable: true),
                    LectureTheatreId = table.Column<int>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblLecture", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lectures_LectureTheatre_LectureTheatreId",
                        column: x => x.LectureTheatreId,
                        principalTable: "tblLectureTheatre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lectures_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "tblSubject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tblEnrollment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValue: false),
                    StudentId = table.Column<int>(nullable: true),
                    LectureId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEnrollment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollments_Lecture_LectureId",
                        column: x => x.StudentId,
                        principalTable: "tblLecture",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "tblStudent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblEnrollment_StudentId",
                table: "tblEnrollment",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_tblLecture_LectureTheatreId",
                table: "tblLecture",
                column: "LectureTheatreId");

            migrationBuilder.CreateIndex(
                name: "IX_tblLecture_SubjectId",
                table: "tblLecture",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEnrollment");

            migrationBuilder.DropTable(
                name: "tblLecture");

            migrationBuilder.DropTable(
                name: "tblLectureTheatre");

            migrationBuilder.DropTable(
                name: "tblSubject");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "tblStudent",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "tblStudent",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "tblStudent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RollNumber",
                table: "tblStudent",
                nullable: true);
        }
    }
}
