using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebDataDemo.Dtos;

namespace WebDataDemo.Option_02_ADONET_Sprocs;

[Route("api/v02/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "2. Authors - ADO.NET SPROCS")]
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
    var authors = new List<AuthorDTO>();
    using var conn = new SqlConnection(_connString);
    var sql = "ListAuthors";
    using var cmd = new SqlCommand(sql, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    conn.Open();
    _logger.LogInformation("Executing stored proc: {sql}", sql);
    using var reader = cmd.ExecuteReader();
    if (reader.HasRows)
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
    using var cmd = new SqlCommand(sql, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@AuthorId", id);
    conn.Open();
    _logger.LogInformation("Executing stored proc: {sql}", sql);
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
    var sql = "InsertAuthor";
    var cmd = new SqlCommand(sql, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@Name", newAuthor.Name);
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
    using var conn = new SqlConnection(_connString);
    var sql = "UpdateAuthor";
    var cmd = new SqlCommand(sql, conn);
    cmd.CommandType = System.Data.CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@Id", id);
    cmd.Parameters.AddWithValue("@Name", value);

    conn.Open();

    cmd.ExecuteScalar();

    conn.Close();
  }

  // DELETE api/<AuthorsController>/5
  [HttpDelete("{id}")]
  public void Delete(int id)
  {
  }
}
