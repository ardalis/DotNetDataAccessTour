using Microsoft.EntityFrameworkCore.Migrations;

namespace WebDataDemo.Data.Migrations;

public partial class InitialData : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "Title",
            table: "Courses",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Slug",
            table: "Courses",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Authors",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.InsertData(
            table: "Authors",
            columns: new[] { "Id", "Name" },
            values: new object[,]
            {
                  { 1, "Steve Smith" },
                  { 2, "Julia Lerman" }
            });

        migrationBuilder.InsertData(
            table: "Courses",
            columns: new[] { "Id", "Slug", "Title" },
            values: new object[,]
            {
                  { 1, "csharp-solid-principles", "SOLID Principles for C# Developers" },
                  { 2, "design-patterns-overview", "Design Patterns Overview" },
                  { 3, "domain-driven-design-fundamentals", "Domain-Driven Design Fundamentals" }
            });

        migrationBuilder.InsertData(
            table: "CourseAuthor",
            columns: new[] { "Id", "AuthorId", "CourseId", "RoyaltyPercentage" },
            values: new object[,]
            {
                  { 1, 1, 1, 100 },
                  { 2, 1, 2, 100 },
                  { 3, 1, 3, 50 },
                  { 4, 2, 3, 50 }
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "CourseAuthor",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "CourseAuthor",
            keyColumn: "Id",
            keyValue: 2);

        migrationBuilder.DeleteData(
            table: "CourseAuthor",
            keyColumn: "Id",
            keyValue: 3);

        migrationBuilder.DeleteData(
            table: "CourseAuthor",
            keyColumn: "Id",
            keyValue: 4);

        migrationBuilder.DeleteData(
            table: "Authors",
            keyColumn: "Id",
            keyValue: 1);

        migrationBuilder.DeleteData(
            table: "Authors",
            keyColumn: "Id",
            keyValue: 2);

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

        migrationBuilder.AlterColumn<string>(
            name: "Title",
            table: "Courses",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Slug",
            table: "Courses",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Name",
            table: "Authors",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(100)",
            oldMaxLength: 100,
            oldNullable: true);
    }
}
