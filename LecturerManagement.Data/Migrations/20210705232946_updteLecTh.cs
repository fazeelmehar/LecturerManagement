using Microsoft.EntityFrameworkCore.Migrations;

namespace LecturerManagement.Data.Migrations
{
    public partial class updteLecTh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatingCapacity",
                table: "tblLectureTheatre",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatingCapacity",
                table: "tblLectureTheatre");
        }
    }
}
