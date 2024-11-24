using Ardalis.Specification;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;
public class CachedRepository<T> : IRepository<T> where T : class
{
  private readonly HybridCache _cache;
  private readonly ILogger<CachedRepository<T>> _logger;
  private readonly EfRepository<T> _sourceRepository;

  public CachedRepository(HybridCache cache,
      ILogger<CachedRepository<T>> logger,
      EfRepository<T> sourceRepository)
  {
    _cache = cache;
    _logger = logger;
    _sourceRepository = sourceRepository;
  }

  public Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.AddAsync(entity, cancellationToken);
  }

  public Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.AddRangeAsync(entities, cancellationToken);
  }

  public Task<bool> AnyAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.AnyAsync(specification, cancellationToken);
  }

  public Task<bool> AnyAsync(CancellationToken cancellationToken = default)
  {
    return _sourceRepository.AnyAsync(cancellationToken);
  }

  public IAsyncEnumerable<T> AsAsyncEnumerable(ISpecification<T> specification)
  {
    return _sourceRepository.AsAsyncEnumerable(specification);
  }

  public Task<int> CountAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.CountAsync(specification, cancellationToken);
  }

  public Task<int> CountAsync(CancellationToken cancellationToken = default)
  {
    return _sourceRepository.CountAsync(cancellationToken);
  }

  public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.DeleteAsync(entity, cancellationToken);
  }

  public Task DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.DeleteRangeAsync(entities, cancellationToken);
  }

  public Task DeleteRangeAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.DeleteRangeAsync(specification, cancellationToken);
  }

  public Task<T> FirstOrDefaultAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-FirstOrDefaultAsync";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
  }

  public Task<TResult?> FirstOrDefaultAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-FirstOrDefaultAsync";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.FirstOrDefaultAsync(specification, cancellationToken);
  }

  public Task<T> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
  {
    return _sourceRepository.GetByIdAsync(id, cancellationToken);
  }

  public Task<T> GetBySpecAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-GetBySpecAsync";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.GetBySpecAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.GetBySpecAsync(specification, cancellationToken);
  }

  public Task<TResult?> GetBySpecAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-GetBySpecAsync";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.GetBySpecAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.GetBySpecAsync(specification, cancellationToken);
  }

  public Task<List<T>> ListAsync(CancellationToken cancellationToken = default)
  {
    string key = $"{nameof(T)}-ListAsync";
    _logger.LogInformation("Checking cache for " + key);
    return _cache.GetOrCreateAsync(key, async entry =>
    {
      _logger.LogWarning("Fetching source data for " + key);
      return await _sourceRepository.ListAsync(cancellationToken);
    }).AsTask();
  }

  public Task<List<T>> ListAsync(ISpecification<T> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-ListAsync";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.ListAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.ListAsync(specification, cancellationToken);
  }

  public Task<List<TResult>> ListAsync<TResult>(ISpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-ListAsync";
      _logger.LogInformation("Checking cache for " + key);

      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.ListAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.ListAsync(specification, cancellationToken);
  }

  public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    return _sourceRepository.SaveChangesAsync(cancellationToken);
  }

  public Task<T> SingleOrDefaultAsync(ISingleResultSpecification<T> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-SingleOrDefaultAsync";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
  }

  public Task<TResult?> SingleOrDefaultAsync<TResult>(ISingleResultSpecification<T, TResult> specification, CancellationToken cancellationToken = default)
  {
    if (specification.CacheEnabled)
    {
      string key = $"{specification.CacheKey}-SingleOrDefaultAsync";
      _logger.LogInformation("Checking cache for " + key);
      return _cache.GetOrCreateAsync(key, async entry =>
      {
        _logger.LogWarning("Fetching source data for " + key);
        return await _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
      }).AsTask();
    }
    return _sourceRepository.SingleOrDefaultAsync(specification, cancellationToken);
  }

  public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.UpdateAsync(entity, cancellationToken);
  }

  public Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
  {
    return _sourceRepository.UpdateRangeAsync(entities, cancellationToken);
  }
}
