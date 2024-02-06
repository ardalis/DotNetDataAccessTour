
using Microsoft.Extensions.Caching.Memory;
using WebDataDemo.Model;

namespace WebDataDemo.Option_10_Repo_Spec_Generic;

public static class Option10Extensions
{
  public static IServiceCollection AddBasicRepoNoCaching(
  this IServiceCollection services,
  Serilog.ILogger logger)
  {
    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

    logger.Information("Registered Option 10 {option}.", "Basic Repo No Caching");

    return services;
  }

  public static IServiceCollection AddCachedRepository(
  this IServiceCollection services,
  Serilog.ILogger logger)
  {
    services.AddScoped<EfRepository<Author>>();
    services.AddScoped<IRepository<Author>>(provider =>
       new CachedRepository<Author>(
          provider.GetRequiredService<IMemoryCache>(),
          provider.GetRequiredService<ILogger<CachedRepository<Author>>>(),
          provider.GetRequiredService<EfRepository<Author>>()));

    logger.Information("Registered Option 10 {option}.", "Cached Repository");

    return services;
  }

  public static IServiceCollection AddTimedCachedRepository(
  this IServiceCollection services,
  Serilog.ILogger logger)
  {
    services.AddScoped<EfRepository<Author>>();
    services.AddScoped<CachedRepository<Author>>(provider =>
      new CachedRepository<Author>(
          provider.GetRequiredService<IMemoryCache>(),
          provider.GetRequiredService<ILogger<CachedRepository<Author>>>(),
          provider.GetRequiredService<EfRepository<Author>>()));

    services.AddScoped<IRepository<Author>>(provider =>
    new TimedRepository<Author>(
            provider.GetRequiredService<ILogger<TimedRepository<Author>>>(),
                       provider.GetRequiredService<CachedRepository<Author>>()));

    logger.Information("Registered Option 10 {option}.", "Timed and Cached Repository");

    return services;
  }
}
