using Dapper.FluentMap;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using WebDataDemo.DapperMapping;
using WebDataDemo.Data;
using WebDataDemo.Model;
using WebDataDemo.Option_10_Repo_Spec_Generic;

var logger = Log.Logger = new LoggerConfiguration()
  .Enrich.FromLogContext()
  .WriteTo.Console()
  .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) =>
    config
        .ReadFrom.Configuration(builder.Configuration)
        .WriteTo.Console()
        .WriteTo.Seq("http://localhost:5341"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
        .LogTo(Log.Logger.Warning, LogLevel.Warning, null));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<WebDataDemo.Option_06_Repo.IAuthorRepository, WebDataDemo.Option_06_Repo.EfAuthorRepository>();
builder.Services.AddScoped<WebDataDemo.Option_07_Repo.IAuthorRepository, WebDataDemo.Option_07_Repo.EfAuthorRepository>();
builder.Services.AddScoped<WebDataDemo.Option_08_Repo.IAuthorRepository, WebDataDemo.Option_08_Repo.EfAuthorRepository>();
builder.Services.AddScoped<WebDataDemo.Option_09_Repo_Spec.IAuthorRepository, WebDataDemo.Option_09_Repo_Spec.EfAuthorRepository>();

// Option 10 - Choose one of the options below and comment the others
// A. Just the generic repo
//builder.Services.AddBasicRepoNoCaching(logger);

// B. Add CachedRepo decorator
//builder.Services.AddCachedRepository(logger);

// C. Add CachedRepo decorator and TimedRepo decorator
builder.Services.AddTimedCachedRepository(logger);

builder.Services.AddSwaggerGen(options =>
{
  options.EnableAnnotations();
  options.TagActionsBy(api => new[] { api.GroupName });
  options.DocInclusionPredicate((name, api) => true);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseMigrationsEndPoint();
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sample v1");
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

FluentMapper.Initialize(config =>
{
  config.AddMap(new CourseDtoMap());
});

app.Run();
