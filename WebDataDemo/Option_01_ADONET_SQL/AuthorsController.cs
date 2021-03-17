using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebDataDemo.Dtos;

namespace WebDataDemo.Option_01_ADONET_SQL
{
    [Route("api/v01/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "1. Authors - ADO.NET SQL")]
    public class AuthorsController : ControllerBase
    {
        private readonly string _connString;

        public AuthorsController(IConfiguration config)
        {
            _connString = config.GetConnectionString("DefaultConnection");
        }

        // GET: api/<AuthorsController>
        [HttpGet]
        public ActionResult<AuthorDTO> Get()
        {
            var authors = new List<AuthorDTO>();
            using var conn = new SqlConnection(_connString);
            var sql = "SELECT * FROM Authors";
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                while (reader.Read())
                {
                    var author = new AuthorDTO();
                    author.Id = reader.GetInt32(0);
                    author.Name = reader.GetString(1);
                    authors.Add(author);
                }
            }

            return Ok(authors);
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public ActionResult<AuthorWithCoursesDTO> Get(int id)
        {
            var author = new AuthorWithCoursesDTO();
            using var conn = new SqlConnection(_connString);
            var sql = @"SELECT a.Id, a.Name, ca.RoyaltyPercentage, ca.CourseId, ca.AuthorId, c.Title
FROM Authors a
INNER JOIN CourseAuthor ca ON a.Id = ca.AuthorId
INNER JOIN Courses c ON c.Id = ca.CourseId
WHERE a.Id = @AuthorId";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AuthorId", id);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    author.Id = reader.GetInt32(0);
                    author.Name = reader.GetString(1);
                    author.Courses.Add(new CourseDTO
                    {
                        Id = reader.GetInt32(3),
                        AuthorId = reader.GetInt32(4),
                        RoyaltyPercentage = reader.GetInt32(2),
                        Title = reader.GetString(5)
                    });
                }
            }

            return Ok(author);
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public ActionResult<AuthorDTO> Post([FromBody] CreateAuthorRequest newAuthor)
        {
            using var conn = new SqlConnection(_connString);
            var sql = "INSERT Authors (name) VALUES (@name);SELECT CAST(scope_identity() AS int)";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", newAuthor.Name);
            conn.Open();
            int newId = (int)cmd.ExecuteScalar();

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
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using var conn = new SqlConnection(_connString);
            var sql = "DELETE Authors WHERE Id = @AuthorId";
            var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AuthorId", id);
            conn.Open();
            cmd.ExecuteNonQuery();

            return NoContent();
        }
    }
}
