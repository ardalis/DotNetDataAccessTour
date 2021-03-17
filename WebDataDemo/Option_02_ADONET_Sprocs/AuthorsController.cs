using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using WebDataDemo.Dtos;

namespace WebDataDemo.Option_02_ADONET_Sprocs
{
    [Route("api/v02/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "2. Authors - ADO.NET SPROCS")]
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
            var sql = "ListAuthors";
            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
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
            var sql = "ListAuthorsWithCourses";
            var cmd = new SqlCommand(sql, conn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
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
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
