using Microsoft.EntityFrameworkCore.Migrations;

namespace LecturerManagement.Data.Migrations
{
    public partial class updteEnroll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Lecture_LectureId",
                table: "tblEnrollment");

            migrationBuilder.CreateIndex(
                name: "IX_tblEnrollment_LectureId",
                table: "tblEnrollment",
                column: "LectureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Lecture_LectureId",
                table: "tblEnrollment",
                column: "LectureId",
                principalTable: "tblLecture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Lecture_LectureId",
                table: "tblEnrollment");

            migrationBuilder.DropIndex(
                name: "IX_tblEnrollment_LectureId",
                table: "tblEnrollment");

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Lecture_LectureId",
                table: "tblEnrollment",
                column: "StudentId",
                principalTable: "tblLecture",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
