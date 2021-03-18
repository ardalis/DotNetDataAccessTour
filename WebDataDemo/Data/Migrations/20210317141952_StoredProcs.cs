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
        LEFT JOIN CourseAuthor ca ON a.Id = ca.AuthorId
        LEFT JOIN Courses c ON c.Id = ca.CourseId
        WHERE a.Id = @AuthorId
')");
            migrationBuilder.Sql(
            @"
    EXEC ('CREATE PROCEDURE ListAuthorsWithCoursesMulti
        @AuthorId int
    AS
SELECT a.Id, a.Name FROM Authors a WHERE Id = @AuthorId;
SELECT ca.RoyaltyPercentage, ca.CourseId, ca.AuthorId, c.Title
FROM CourseAuthor ca
INNER JOIN Courses c ON c.Id = ca.CourseId
WHERE ca.AuthorId = @AuthorId')");

            migrationBuilder.Sql(
            @"
    EXEC ('CREATE PROCEDURE InsertAuthor
        @name varchar(100)
    AS
INSERT Authors (name) VALUES (@name);SELECT CAST(scope_identity() AS int);')");

            migrationBuilder.Sql(
            @"
    EXEC ('CREATE PROCEDURE DeleteAuthor
        @AuthorId int
    AS
DELETE Authors WHERE Id = @AuthorId;')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
    EXEC ('DROP PROCEDURE ListAuthors');
    EXEC ('DROP PROCEDURE ListAuthorsWithCourses')
    EXEC ('DROP PROCEDURE ListAuthorsWithCoursesMulti')
    EXEC ('DROP PROCEDURE InsertAuthor')
    EXEC ('DROP PROCEDURE DeleteAuthors')
");
        }
    }
}
