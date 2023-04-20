using Microsoft.EntityFrameworkCore.Migrations;

namespace WebDataDemo.Data.Migrations;

public partial class InitialModel : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Authors",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Authors", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Courses",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                Slug = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Courses", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "CourseAuthor",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AuthorId = table.Column<int>(type: "int", nullable: false),
                CourseId = table.Column<int>(type: "int", nullable: false),
                RoyaltyPercentage = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_CourseAuthor", x => x.Id);
                table.ForeignKey(
                      name: "FK_CourseAuthor_Authors_AuthorId",
                      column: x => x.AuthorId,
                      principalTable: "Authors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                      name: "FK_CourseAuthor_Courses_CourseId",
                      column: x => x.CourseId,
                      principalTable: "Courses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_CourseAuthor_AuthorId",
            table: "CourseAuthor",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_CourseAuthor_CourseId",
            table: "CourseAuthor",
            column: "CourseId");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "CourseAuthor");

        migrationBuilder.DropTable(
            name: "Authors");

        migrationBuilder.DropTable(
            name: "Courses");
    }
}
