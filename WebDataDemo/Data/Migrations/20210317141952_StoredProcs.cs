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
    GO;')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
    EXEC ('DROP PROCEDURE ListAuthors')");
        }
    }
}
