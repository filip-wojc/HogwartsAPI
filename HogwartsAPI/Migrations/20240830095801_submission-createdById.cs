using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HogwartsAPI.Migrations
{
    /// <inheritdoc />
    public partial class submissioncreatedById : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "HomeworkSubmissions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomeworkSubmissions_CreatedById",
                table: "HomeworkSubmissions",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_HomeworkSubmissions_Users_CreatedById",
                table: "HomeworkSubmissions",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HomeworkSubmissions_Users_CreatedById",
                table: "HomeworkSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_HomeworkSubmissions_CreatedById",
                table: "HomeworkSubmissions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "HomeworkSubmissions");
        }
    }
}
