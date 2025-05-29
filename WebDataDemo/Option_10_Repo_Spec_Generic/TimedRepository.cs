using System.Diagnostics;
using System.Runtime.CompilerServices;
using Ardalis.Specification;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;
public class TimedRepository<T> : IRepository<T> where T : class
{
  private readonly ILogger<TimedRepository<T>> _logger;
  private readonly IRepository<T> _wrappedRepository;

  public TimedRepository(ILogger<TimedRepository<T>> logger,
    IRepository<T> wrappedRepository)
  {
    _logger = logger;
    _wrappedRepository = wrappedRepository;
  }

  private void LogExecutionTime(long milliseconds, string methodName)
  {
    _logger.LogInformation($"Method {methodName} took {milliseconds}ms to execute.");
  }

  private async Task ExecuteAndLogTime(Func<Task> asyncAction,
    [CallerMemberName]string methodName = "")
  {
    var watch = System.Diagnostics.Stopwatch.StartNew();
    try
    {
      await asyncAction();
    }
    finally
    {
      watch.Stop();
      LogExecutionTime(watch.ElapsedMilliseconds, methodName);
    }
  }

  private async Task<T> ExecuteWithTimingAsync<T>(Func<Task<T>> asyncFunc,
    [CallerMemberName] string methodName = "")
  {
    Stopwatch stopwatch = Stopwatch.StartNew();

    T result;
    try
    {
      result = await asyncFunc();
    }
    finally
    {
      stopwatch.Stop();
      LogExecutionTime(stopwatch.ElapsedMilliseconds, methodName);
    }

    return result;
  }

  public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() => 
    _wrappedRepository.AddAsync(entity, cancellationToken));
  }

  public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.AddRangeAsync(entities, cancellationToken));

  }

  public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
  {
    throw new NotImplementedException();
  }

  public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<int> CountAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<int> DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task<T> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.FirstOrDefaultAsync(specification, cancellationToken));
  }

  public async Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.FirstOrDefaultAsync(specification, cancellationToken));
  }

  public async Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.GetByIdAsync(id, cancellationToken));
  }

  public Task<T> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.ListAsync(cancellationToken));
  }

  public async Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.ListAsync(specification, cancellationToken));
  }

  public async Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.ListAsync(specification,cancellationToken));
  }

  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public async Task<T> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.SingleOrDefaultAsync(specification, cancellationToken));
  }

  public async Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    return await ExecuteWithTimingAsync(() =>
_wrappedRepository.SingleOrDefaultAsync(specification, cancellationToken));
  }

  public Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task<int> UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}
