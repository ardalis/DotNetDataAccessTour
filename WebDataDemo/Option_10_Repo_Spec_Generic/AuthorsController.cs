using Microsoft.AspNetCore.Mvc;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

[Route("api/v10/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "10. Authors - Generic Repo + Specs")]
public class AuthorsController : ControllerBase
{
  private readonly IRepository<Author> _authorRepository;

  public AuthorsController(IRepository<Author> authorRepository)
  {
    _authorRepository = authorRepository;
  }

  // GET: api/<AuthorsController>
  [HttpGet]
  public async Task<ActionResult<AuthorDTO>> Get()
  {
    var spec = new AllAuthorsAsDTOsSpec();
    var authors = await _authorRepository.ListAsync(spec);

    return Ok(authors);
  }

  // GET api/<AuthorsController>/5
  [HttpGet("{id}")]
  public async Task<ActionResult<AuthorWithCoursesDTO>> Get(int id)
  {
    // option 1
    //var spec = new AuthorByIdWithCoursesAsDTOsSpec(id);
    //var authorDTO = (await _authorRepository.ListAsync(spec)).SingleOrDefault();

    // option 2
    var spec = new AuthorByIdWithCoursesSpec(id);
    var author = await _authorRepository.SingleOrDefaultAsync(spec);

    var authorDTO = new AuthorWithCoursesDTO
    {
      Name = author.Name,
      Id = author.Id,
      Courses = new List<CourseDTO>()
    };
    authorDTO.Courses = author.Courses
        .Select(ca => new CourseDTO
        {
          Id = ca.Course.Id,
          AuthorId = ca.AuthorId,
          RoyaltyPercentage = ca.RoyaltyPercentage,
          Title = ca.Course.Title
        }).ToList();

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
    await _authorRepository.AddAsync(author);

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
