using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_07_Repo;

/// <summary>
/// Using IQueryable at the repo level
/// </summary>
[Route("api/v07/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "7. Authors - AuthorRepo (IQueryable)")]
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
    var authors = await _authorRepository.List()
        .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name })
        .ToListAsync();

    return Ok(authors);
  }

  // GET api/<AuthorsController>/5
  [HttpGet("{id}")]
  public async Task<ActionResult<AuthorWithCoursesDTO>> Get(int id)
  {
    // doesn't work - courses will be empty
    //var author = await _authorRepository.GetByIdAsync(id);
    //var authorDTO = new AuthorWithCoursesDTO
    //{
    //  Id = author.Id,
    //  Name = author.Name,
    //  Courses = author.Courses.Select(c => new CourseDTO
    //  {
    //    Id = c.Id,
    //    Title = c.Course.Title,
    //    AuthorId = author.Id,
    //    RoyaltyPercentage = c.RoyaltyPercentage
    //  }).ToList()
    //};

    var authorsWithCourses = _authorRepository.List()
      .Include(a => a.Courses)
      .Select(author => new AuthorWithCoursesDTO
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
      })
      .FirstOrDefault(a => a.Id == id);

    return authorsWithCourses;
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
