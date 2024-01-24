using Ardalis.Specification;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

public interface IRepository<T> : IRepositoryBase<T>, IReadRepositoryBase<T>
  where T : class
{
}
