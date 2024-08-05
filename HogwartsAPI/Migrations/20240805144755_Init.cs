using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HogwartsAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    WoodType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wands_Cores_CoreId",
                        column: x => x.CoreId,
                        principalTable: "Cores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Wands_WandId",
                        column: x => x.WandId,
                        principalTable: "Wands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultyLevel = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Houses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrophyCount = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Houses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Houses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WandId = table.Column<int>(type: "int", nullable: false),
                    SchoolYear = table.Column<int>(type: "int", nullable: false),
                    HouseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Houses_HouseId",
                        column: x => x.HouseId,
                        principalTable: "Houses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Wands_WandId",
                        column: x => x.WandId,
                        principalTable: "Wands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CourseStudent",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    StudentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStudent", x => new { x.CoursesId, x.StudentsId });
                    table.ForeignKey(
                        name: "FK_CourseStudent_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseStudent_Students_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasMagicAbility = table.Column<bool>(type: "bit", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Cores",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "A feather from a phoenix", "Phoenix Feather" },
                    { 2, "A heartstring from a dragon", "Dragon Heartstring" },
                    { 3, "A hair from a unicorn", "Unicorn Hair" }
                });

            migrationBuilder.InsertData(
                table: "Wands",
                columns: new[] { "Id", "Color", "CoreId", "Length", "Price", "WoodType" },
                values: new object[,]
                {
                    { 1, "Brown", 1, 11.0, 150m, "Holly" },
                    { 2, "Black", 2, 10.0, 120m, "Yew" },
                    { 3, "Light Brown", 3, 9.5, 90m, "Elm" },
                    { 4, "White", 2, 12.0, 80m, "Oak" },
                    { 5, "Dark Brown", 3, 10.5, 190m, "Ivory" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "DateOfBirth", "Name", "Surname", "WandId" },
                values: new object[,]
                {
                    { 1, new DateTime(1881, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Albus", "Dumbledore", 1 },
                    { 2, new DateTime(1930, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Severus", "Snape", 2 },
                    { 3, new DateTime(1922, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Minerva", "McGonagall", 3 },
                    { 4, new DateTime(1951, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pomona", "Sprout", 4 },
                    { 5, new DateTime(1943, 12, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Filius", "Flitwick", 5 }
                });

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "Date", "Description", "DifficultyLevel", "Name", "TeacherId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 8, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn how to defend against dark magic.", 3, "Defense Against the Dark Arts", 1 },
                    { 2, new DateTime(2024, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn how to brew magical potions.", 5, "Potions", 2 },
                    { 3, new DateTime(2024, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn how to cast advanced speels.", 2, "Spells", 5 },
                    { 4, new DateTime(2024, 8, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Learn how to take care of plants.", 3, "Herbology", 4 }
                });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "CreationDate", "Description", "Name", "TeacherId", "TrophyCount" },
                values: new object[,]
                {
                    { 1, new DateTime(990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Brave and daring", "Gryffindor", 3, 10 },
                    { 2, new DateTime(990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Loyal and fair", "Hufflepuff", 4, 5 },
                    { 3, new DateTime(990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wise and clever", "Ravenclaw", 5, 7 },
                    { 4, new DateTime(990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cunning and ambitious", "Slytherin", 2, 8 }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfBirth", "HouseId", "Name", "SchoolYear", "Surname", "WandId" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Harry", 1, "Potter", 1 },
                    { 2, new DateTime(1979, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Hermione", 1, "Granger", 3 },
                    { 3, new DateTime(1976, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Fred", 3, "Weasley", 4 },
                    { 4, new DateTime(1978, 9, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Hanna", 2, "Abbot", 3 },
                    { 5, new DateTime(1975, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Cedric", 4, "Diggory", 5 },
                    { 6, new DateTime(1979, 10, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "Cho", 1, "Chang", 5 },
                    { 7, new DateTime(1980, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Draco", 1, "Malfoy", 2 },
                    { 8, new DateTime(1979, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, "Vincent", 1, "Crabbe", 2 }
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "HasMagicAbility", "Name", "StudentId", "Type" },
                values: new object[,]
                {
                    { 1, false, "Hedwig", 1, "Owl" },
                    { 2, true, "Crookshanks", 2, "Cat" },
                    { 3, false, "Teodora", 4, "Frog" },
                    { 4, true, "Random Owl", 7, "Owl" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherId",
                table: "Courses",
                column: "TeacherId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseStudent_StudentsId",
                table: "CourseStudent",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Houses_TeacherId",
                table: "Houses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_StudentId",
                table: "Pets",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_HouseId",
                table: "Students",
                column: "HouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_WandId",
                table: "Students",
                column: "WandId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_WandId",
                table: "Teachers",
                column: "WandId");

            migrationBuilder.CreateIndex(
                name: "IX_Wands_CoreId",
                table: "Wands",
                column: "CoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseStudent");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Houses");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Wands");

            migrationBuilder.DropTable(
                name: "Cores");
        }
    }
}
