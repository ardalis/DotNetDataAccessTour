using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebDataDemo.Dtos;

namespace WebDataDemo.Option_04_Dapper_Sprocs;

[Route("api/v04/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "4. Authors - Dapper SPROCS")]
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
  public ActionResult<AuthorDTO> Get()
  {
    using var conn = new SqlConnection(_connString);
    var sql = "ListAuthors";
    var authors = conn.Query<AuthorDTO>(sql, 
      commandType: CommandType.StoredProcedure)
        .ToList();

    return Ok(authors);
  }

  // GET api/<AuthorsController>/5
  [HttpGet("{id}")]
  public ActionResult<AuthorWithCoursesDTO> Get(int id)
  {
    using var conn = new SqlConnection(_connString);
    var sql = "ListAuthorsWithCoursesMulti";

    _logger.LogInformation("Executing stored proc: {sql}", sql);

    var result = conn.QueryMultiple(sql, new { AuthorId = id },
      commandType: CommandType.StoredProcedure);

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

    _logger.LogInformation("Executing stored proc: {sql}", sql);

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
    _logger.LogInformation("Executing stored proc: {sql}", sql);
    conn.Query(sql, new { Name = value, Id = id }, commandType: System.Data.CommandType.StoredProcedure);
  }

  // DELETE api/<AuthorsController>/5
  [HttpDelete("{id}")]
  public ActionResult Delete(int id)
  {
    using var conn = new SqlConnection(_connString);
    var sql = "DeleteAuthor";
    _logger.LogInformation("Executing stored proc: {sql}", sql);
    conn.Execute(sql, new { AuthorId = id }, commandType: System.Data.CommandType.StoredProcedure);

    return NoContent();
  }
}
