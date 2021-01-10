using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApp.API.Migrations
{
    public partial class Removed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Teacher",
                table: "Courses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Teacher",
                table: "Courses",
                type: "TEXT",
                nullable: true);
        }
    }
}
