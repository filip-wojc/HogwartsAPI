using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HogwartsAPI.Migrations
{
    /// <inheritdoc />
    public partial class revertcoursedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CreatedById", "Description", "DifficultyLevel", "Name", "TeacherId" },
                values: new object[,]
                {
                    { 1, null, "Learn how to defend against dark magic.", 3, "Defense Against the Dark Arts", 1 },
                    { 2, null, "Learn how to brew magical potions.", 5, "Potions", 2 },
                    { 3, null, "Learn how to cast advanced speels.", 2, "Spells", 5 },
                    { 4, null, "Learn how to take care of plants.", 3, "Herbology", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
