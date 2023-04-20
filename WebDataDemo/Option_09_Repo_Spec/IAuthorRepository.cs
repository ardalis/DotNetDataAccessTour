using WebDataDemo.Model;

namespace WebDataDemo.Option_09_Repo_Spec;

public interface IAuthorRepository
{
  Task<IEnumerable<Author>> List(AuthorSpecification spec = null);
  Task<Author> GetBySpecAsync(AuthorSpecification spec);
  Task CreateAsync(Author newAuthor);
  Task UpdateAsync(Author author);
  Task DeleteAsync(Author author);
}
