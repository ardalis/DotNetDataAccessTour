using System.Linq.Expressions;
using WebDataDemo.Model;

namespace WebDataDemo.Option_08_Repo;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> ListAsync(Expression<Func<Author, bool>> predicate);
    Task<Author> GetByIdAsync(int id);
    Task CreateAsync(Author newAuthor);
    Task UpdateAsync(Author author);
    Task DeleteAsync(Author author);

}
