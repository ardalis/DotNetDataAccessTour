using Microsoft.AspNetCore.Mvc;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_08_Repo;

/// <summary>
/// Accepting predicates in the repo interface
/// </summary>
[Route("api/v08/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "8. Authors - AuthorRepo (w/Predicates)")]
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
    var authors = (await _authorRepository.List(a => true))
        .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name }) // projection/mapping in memory
        .ToList();

    return Ok(authors);
  }

  // GET api/<AuthorsController>/5
  [HttpGet("{id}")]
  public async Task<ActionResult<AuthorWithCoursesDTO>> Get(int id)
  {
    var author = await _authorRepository.GetByIdAsync(id);

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

    // example passing an expression
    var steve = (await _authorRepository
                  .ListAsync(a => a.Name == "Steve Smith"))
                  .FirstOrDefault();

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
