using Microsoft.AspNetCore.Mvc;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_06_Repo;

[Route("api/v06/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "6. Authors - AuthorRepo (Simple)")]
public class AuthorsController : ControllerBase
{
  private readonly IAuthorRepository _authorRepository;

  public AuthorsController(IAuthorRepository authorRepository)
  {
    _authorRepository = authorRepository;
  }

  // GET: api/<AuthorsController>
  [HttpGet]
  public async Task<ActionResult<AuthorDTO>> Get()
  {
    var authors = (await _authorRepository.ListAsync())
    .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name }) // note this no longer impacts query generation
    .ToList(); // this is an extra list operation; we could use a cast instead

    return Ok(authors);
  }

  // GET api/<AuthorsController>/5
  [HttpGet("{id}")]
  public async Task<ActionResult<AuthorWithCoursesDTO>> Get(int id)
  {
    var author = await _authorRepository.GetByIdAsyncWithCourses(id);

    var authorDTO = new AuthorWithCoursesDTO
    {
      Id = author.Id,
      Name = author.Name,
      Courses = author.Courses.Select(c => new CourseDTO
      {
        Id = c.Id,
        Title = c.Course.Title,
        AuthorId = author.Id,
        RoyaltyPercentage = c.RoyaltyPercentage
      }).ToList()
    };

    return authorDTO;
  }

  // POST api/<AuthorsController>
  [HttpPost]
  public async Task<ActionResult<AuthorDTO>> Post([FromBody] CreateAuthorRequest newAuthor)
  {
    var author = new Author
    {
      Name = newAuthor.Name
    };
    await _authorRepository.CreateAsync(author);

    var authorDto = new AuthorDTO
    {
      Id = author.Id,
      Name = author.Name
    };

    return Ok(authorDto);
  }

  // PUT api/<AuthorsController>/5
  [HttpPut("{id}")]
  public async Task Put(int id, [FromBody] string value)
  {
    var authorToUpdate = await _authorRepository.GetByIdAsync(id);

    authorToUpdate.Name = value;

    await _authorRepository.UpdateAsync(authorToUpdate);
  }

  // DELETE api/<AuthorsController>/5
  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(int id)
  {
    var authorToDelete = await _authorRepository.GetByIdAsync(id);
    await _authorRepository.DeleteAsync(authorToDelete);
    return NoContent();
  }
}
