using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebDataDemo.Data;
using WebDataDemo.Dtos;
using WebDataDemo.Model;

namespace WebDataDemo.Option_09_Repo_Spec
{
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
                .Select(a => new AuthorDTO { Id = a.Id, Name = a.Name}) // projection/mapping in memory
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

    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> List(AuthorSpecification spec = null);
        Task<Author> GetBySpecAsync(AuthorSpecification spec);
        Task CreateAsync(Author newAuthor);
        Task UpdateAsync(Author author);
        Task DeleteAsync(Author author);
    }

    public abstract class AuthorSpecification
    {
        public Expression<Func<Author, bool>> Predicate { get; protected set; }
    }

    public class AuthorByIdSpecification : AuthorSpecification
    {
        public AuthorByIdSpecification(int id)
        {
            Predicate = Author => Author.Id == id;
        }
    }

    public class EfAuthorRepository : IAuthorRepository
    {
        private readonly AppDbContext _dbContext;

        public EfAuthorRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Author> GetBySpecAsync(AuthorSpecification spec)
        {
            return _dbContext.Authors
                .FirstOrDefaultAsync(spec.Predicate);
        }

        public async Task<IEnumerable<Author>> List(AuthorSpecification spec)
        {
            return await _dbContext.Set<Author>()
                .Where(spec.Predicate)
                .ToListAsync();
        }


        public IQueryable<Author> List()
        {
            return _dbContext.Authors.AsQueryable();
        }

        public async Task CreateAsync(Author newAuthor)
        {
            await _dbContext.Authors.AddAsync(newAuthor);
            await _dbContext.SaveChangesAsync();
        }

        public Task UpdateAsync(Author author)
        {
            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteAsync(Author author)
        {
            _dbContext.Remove(author);
            return _dbContext.SaveChangesAsync();
        }
    }
}
