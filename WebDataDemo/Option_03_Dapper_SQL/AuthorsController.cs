using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using WebDataDemo.Dtos;

namespace WebDataDemo.Option_03_Dapper_SQL
{
    [Route("api/v03/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "3. Authors - Dapper SQL")]
    public class AuthorsController : ControllerBase
    {
        private readonly string _connString;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IConfiguration config,
            ILogger<AuthorsController> logger)
        {
            _connString = config.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        // GET: api/<AuthorsController>
        [HttpGet]
        public ActionResult<IEnumerable<AuthorDTO>> Get()
        {
            using var conn = new SqlConnection(_connString);
            var sql = "SELECT * FROM Authors";
            _logger.LogInformation("Executing query: {sql}", sql);
            var authors = conn.Query<AuthorDTO>(sql).ToList();

            return Ok(authors);
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public ActionResult<AuthorWithCoursesDTO> Get(int id)
        {
            using var conn = new SqlConnection(_connString);
            // https://medium.com/dapper-net/handling-multiple-resultsets-4b108a8c5172
            var sql = @"SELECT a.Id, a.Name FROM Authors a WHERE Id = @AuthorId;
SELECT ca.RoyaltyPercentage, ca.CourseId, ca.AuthorId, c.Title
FROM CourseAuthor ca
INNER JOIN Courses c ON c.Id = ca.CourseId
WHERE ca.AuthorId = @AuthorId";
            _logger.LogInformation("Executing query: {sql}", sql);

            var result = conn.QueryMultiple(sql, new { AuthorId = id });

            var author = result.ReadSingle<AuthorWithCoursesDTO>();
            var courses = result.Read<CourseDTO>().ToList();
            author.Courses.AddRange(courses);
            return Ok(author);
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public ActionResult<AuthorDTO> Post([FromBody] CreateAuthorRequest newAuthor)
        {
            // https://stackoverflow.com/a/8270264
            var sql = "INSERT Authors (name) VALUES (@name);SELECT CAST(scope_identity() AS int)";
            using var conn = new SqlConnection(_connString);

            int newId = conn.QuerySingle<int>(sql, new { name = newAuthor.Name });

            var authorDto = new AuthorDTO
            {
                Id = newId,
                Name = newAuthor.Name
            };

            return Ok(authorDto);
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            using var conn = new SqlConnection(_connString);
            var sql = "UPDATE Authors SET name = @Name WHERE Id = @Id";

            conn.Execute(sql, new { Name = value, Id = id });
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using var conn = new SqlConnection(_connString);
            var sql = "DELETE Authors WHERE Id = @AuthorId";

            conn.Execute(sql, new { AuthorId = id });

            return NoContent();
        }
    }
}
