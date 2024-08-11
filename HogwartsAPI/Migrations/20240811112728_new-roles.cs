using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HogwartsAPI.Migrations
{
    /// <inheritdoc />
    public partial class newroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Wands",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Pets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Pets",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "HouseManager");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "WandsManager");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 4, "CourseManager" },
                    { 5, "PetManager" },
                    { 6, "Admin" }
                });

            migrationBuilder.UpdateData(
                table: "Wands",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Wands",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Wands",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Wands",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedById",
                value: null);

            migrationBuilder.UpdateData(
                table: "Wands",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedById",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Wands_CreatedById",
                table: "Wands",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_CreatedById",
                table: "Pets",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatedById",
                table: "Courses",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Users_CreatedById",
                table: "Courses",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pets_Users_CreatedById",
                table: "Pets",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wands_Users_CreatedById",
                table: "Wands",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Users_CreatedById",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Pets_Users_CreatedById",
                table: "Pets");

            migrationBuilder.DropForeignKey(
                name: "FK_Wands_Users_CreatedById",
                table: "Wands");

            migrationBuilder.DropIndex(
                name: "IX_Wands_CreatedById",
                table: "Wands");

            migrationBuilder.DropIndex(
                name: "IX_Pets_CreatedById",
                table: "Pets");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CreatedById",
                table: "Courses");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Wands");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Pets");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Courses");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Manager");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Admin");
        }
    }
}
