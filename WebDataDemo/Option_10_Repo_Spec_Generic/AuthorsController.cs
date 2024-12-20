﻿using Microsoft.AspNetCore.Mvc;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

[Route("api/v10/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "10. Authors - Generic Repo + Specs")]
public class AuthorsController : ControllerBase
{
  private readonly IRepository<Author> _authorRepository;
  private readonly ILogger<AuthorsController> _logger;

  public AuthorsController(IRepository<Author> authorRepository,
    ILogger<AuthorsController> logger)
  {
    _authorRepository = authorRepository;
    _logger = logger;

    _logger.LogDebug($"Repo type: {authorRepository.GetType().Name}");  
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
    // option 1 - projection is used for the query
    var spec = new AuthorByIdWithCoursesAsDTOsSpec(id);
    AuthorWithCoursesDTO authorDTO = await _authorRepository.SingleOrDefaultAsync(spec);
    _logger.LogInformation("Got authorDTO", authorDTO);

    // option 2 - mapping is done in memory after fetching full entity
    var spec2 = new AuthorByIdWithCoursesSpec(id);
    var author = await _authorRepository.SingleOrDefaultAsync(spec2);

    var authorDTO2 = new AuthorWithCoursesDTO
    {
      Name = author.Name,
      Id = author.Id,
      Courses = new List<CourseDTO>()
    };
    authorDTO2.Courses.AddRange(author.Courses
        .Select(ca => new CourseDTO
        {
          Id = ca.Course.Id,
          AuthorId = ca.AuthorId,
          RoyaltyPercentage = ca.RoyaltyPercentage,
          Title = ca.Course.Title
        }));
    _logger.LogInformation("Got authorDTO2 and mapped it in memory", authorDTO2);

    return authorDTO2;
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
