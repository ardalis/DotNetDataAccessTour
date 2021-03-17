using Microsoft.EntityFrameworkCore.Migrations;

namespace WebDataDemo.Data.Migrations
{
    public partial class StoredProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
    EXEC ('CREATE PROCEDURE ListAuthors
    AS
        SELECT Id, Name FROM Authors
')");
            migrationBuilder.Sql(
            @"
    EXEC ('CREATE PROCEDURE ListAuthorsWithCourses
        @AuthorId int
    AS
        SELECT a.Id, a.Name, ca.RoyaltyPercentage, ca.CourseId, ca.AuthorId, c.Title
        FROM Authors a
        INNER JOIN CourseAuthor ca ON a.Id = ca.AuthorId
        INNER JOIN Courses c ON c.Id = ca.CourseId
        WHERE a.Id = @AuthorId
')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
    EXEC ('DROP PROCEDURE ListAuthors');
    EXEC ('DROP PROCEDURE ListAuthorsWithCourses')
");
        }
    }
}
