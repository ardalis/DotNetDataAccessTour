using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebDataDemo.Data;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic
{
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
            var spec = new AuthorsWithCoursesByIdAsDTOsSpec(id);
            var authorDTO = (await _authorRepository.ListAsync(spec)).SingleOrDefault();

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

    public interface IRepository<T> : IRepositoryBase<T>, IReadRepositoryBase<T> where T : class
    {
    }

    public class EfRepository<T> : RepositoryBase<T>, IRepository<T> where T : class
    {
        public EfRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class AllAuthorsAsDTOsSpec : Specification<Author, AuthorDTO>
    {
        public AllAuthorsAsDTOsSpec()
        {
            Query
                .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name });

            Query.EnableCache(nameof(AllAuthorsAsDTOsSpec));
        }
    }
    public class AllAuthorsAsDTOsSpec2 : Specification<Author>
    {
        public AllAuthorsAsDTOsSpec2()
        {
            Query.EnableCache(nameof(AllAuthorsAsDTOsSpec2));
        }
    }

    public class AuthorsWithCoursesByIdAsDTOsSpec : Specification<Author, AuthorWithCoursesDTO>, ISingleResultSpecification
    {
        public AuthorsWithCoursesByIdAsDTOsSpec(int id)
        {
            Query
                .Where(a => a.Id == id)
                .Include(author => author.Courses)
                .ThenInclude(ca => ca.Course);
            Query
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
                });

            Query.EnableCache(nameof(AuthorsWithCoursesByIdAsDTOsSpec), id);
        }
    }
}
