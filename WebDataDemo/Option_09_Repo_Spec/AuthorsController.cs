using Microsoft.AspNetCore.Mvc;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_09_Repo_Spec;

/// <summary>
/// Using a specification with the repo, with support for predicates and includes
/// </summary>
[Route("api/v09/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "9. Authors - AuthorRepo (w/Specs)")]
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
    var authors = (await _authorRepository.List())
        .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name }) // projection/mapping in memory
        .ToList();

    return Ok(authors);
  }

  // GET api/<AuthorsController>/5
  [HttpGet("{id}")]
  public async Task<ActionResult<AuthorWithCoursesDTO>> Get(int id)
  {
    var spec = new AuthorByIdSpecification(id);
    var author = await _authorRepository.GetBySpecAsync(spec);

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
    var spec = new AuthorByIdSpecification(id);
    var authorToUpdate = await _authorRepository.GetBySpecAsync(spec);

    authorToUpdate.Name = value;

    await _authorRepository.UpdateAsync(authorToUpdate);
  }

  // DELETE api/<AuthorsController>/5
  [HttpDelete("{id}")]
  public async Task<ActionResult> Delete(int id)
  {
    var spec = new AuthorByIdSpecification(id);
    var authorToDelete = await _authorRepository.GetBySpecAsync(spec);
    await _authorRepository.DeleteAsync(authorToDelete);
    return NoContent();
  }
}
