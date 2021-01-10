using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApp.API.Migrations
{
    public partial class UpdatedCourses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Courses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Courses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Faculty",
                table: "Courses",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Specialization",
                table: "Courses",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Faculty",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Specialization",
                table: "Courses");
        }
    }
}
