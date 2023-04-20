using WebDataDemo.Model;

namespace WebDataDemo.Option_06_Repo;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> ListAsync();
    Task<Author> GetByIdAsync(int id);
    Task<Author> GetByIdAsyncWithCourses(int id);

    Task CreateAsync(Author newAuthor);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Author author);

}
