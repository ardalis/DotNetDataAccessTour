using WebDataDemo.Model;

namespace WebDataDemo.Option_07_Repo;

public interface IAuthorRepository
{
    IQueryable<Author> List();
    Task<Author> GetByIdAsync(int id);
    Task CreateAsync(Author newAuthor);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Author author);

}
