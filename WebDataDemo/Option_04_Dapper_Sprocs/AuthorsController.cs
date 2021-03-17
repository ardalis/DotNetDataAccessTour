using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Linq;
using WebDataDemo.Dtos;

namespace WebDataDemo.Option_04_Dapper_Sprocs
{
    [Route("api/v04/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "4. Authors - Dapper SPROCS")]
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
            using var conn = new SqlConnection(_connString);
            var sql = "ListAuthors";
            var authors = conn.Query<AuthorDTO>(sql, commandType: System.Data.CommandType.StoredProcedure)
                .ToList();

            return Ok(authors);
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public ActionResult<AuthorWithCoursesDTO> Get(int id)
        {
            using var conn = new SqlConnection(_connString);
            var sql = "ListAuthorsWithCoursesMulti";

            var result = conn.QueryMultiple(sql, new { AuthorId = id }, commandType: System.Data.CommandType.StoredProcedure);

            var author = result.ReadSingle<AuthorWithCoursesDTO>();
            var courses = result.Read<CourseDTO>().ToList();
            author.Courses.AddRange(courses);
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
