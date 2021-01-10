using Microsoft.EntityFrameworkCore.Migrations;

namespace StudentApp.API.Migrations
{
    public partial class Update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_UserId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_UserId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Assignations_CourseId",
                table: "Assignations");

            migrationBuilder.DropIndex(
                name: "IX_Assignations_UserId",
                table: "Assignations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Assignations_UserId",
                table: "Assignations",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assignations_UserId",
                table: "Assignations");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Courses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_UserId",
                table: "Courses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignations_CourseId",
                table: "Assignations",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assignations_UserId",
                table: "Assignations",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_UserId",
                table: "Courses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
