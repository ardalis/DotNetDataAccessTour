using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebDataDemo.Data;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_05_Ef_Core
{
    [Route("api/v05/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "5. Authors - EF Core")]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public AuthorsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/<AuthorsController>
        [HttpGet]
        public ActionResult<AuthorDTO> Get()
        {
            var authors = _dbContext.Authors
                .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name})
                .ToList();

            return Ok(authors);
        }

        // GET api/<AuthorsController>/5
        [HttpGet("{id}")]
        public ActionResult<AuthorWithCoursesDTO> Get(int id)
        {
            // notes on Find vs. SingleOrDefault
            // https://stackoverflow.com/questions/7348663/c-sharp-entity-framework-how-can-i-combine-a-find-and-include-on-a-model-obje
            var author = _dbContext.Authors
                .Include(author => author.Courses)
                .ThenInclude(ca => ca.Course)
                .Select(a => new AuthorWithCoursesDTO
                {
                    Id = a.Id,
                    Name = a.Name,
                    Courses = a.Courses.Select(c => new CourseDTO
                    {
                        Id = c.Id,
                        Title = c.Course.Title,
                        AuthorId = a.Id,
                        RoyaltyPercentage = c.RoyaltyPercentage
                    }).ToList()
                })
                .SingleOrDefault(a => a.Id == id);
            return author;
        }

        // POST api/<AuthorsController>
        [HttpPost]
        public ActionResult<AuthorDTO> Post([FromBody] CreateAuthorRequest newAuthor)
        {
            var author = new Author
            {
                Name = newAuthor.Name
            };
            _dbContext.Authors.Add(author);
            _dbContext.SaveChanges();

            var authorDto = new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name
            };

            return Ok(authorDto);
        }

        // PUT api/<AuthorsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuthorsController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var authorToDelete = _dbContext.Authors.Find(id);

            if (authorToDelete == null) return NotFound();

            _dbContext.Remove(authorToDelete);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
