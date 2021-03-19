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
        public ActionResult<AuthorDTO> Post([FromBody] CreateAuthorRequest newAuthor)
        {
            using var conn = new SqlConnection(_connString);
            var sql = "InsertAuthor";

            int newId = conn.QuerySingle<int>(sql, new { name = newAuthor.Name }, commandType: System.Data.CommandType.StoredProcedure);

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
            var sql = "UpdateAuthor";
            conn.Query(sql, new { Name = value, Id = id }, commandType: System.Data.CommandType.StoredProcedure);
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            using var conn = new SqlConnection(_connString);
            var sql = "DeleteAuthor";
            conn.Execute(sql, new { AuthorId = id }, commandType: System.Data.CommandType.StoredProcedure);

            return NoContent();
        }
    }
}
