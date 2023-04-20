using Dapper.FluentMap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using WebDataDemo.DapperMapping;
using WebDataDemo.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<WebDataDemo.Option_06_Repo.IAuthorRepository, WebDataDemo.Option_06_Repo.EfAuthorRepository>();
builder.Services.AddScoped<WebDataDemo.Option_07_Repo.IAuthorRepository, WebDataDemo.Option_07_Repo.EfAuthorRepository>();
builder.Services.AddScoped<WebDataDemo.Option_08_Repo.IAuthorRepository, WebDataDemo.Option_08_Repo.EfAuthorRepository>();
builder.Services.AddScoped<WebDataDemo.Option_09_Repo_Spec.IAuthorRepository, WebDataDemo.Option_09_Repo_Spec.EfAuthorRepository>();
builder.Services.AddScoped(typeof(WebDataDemo.Option_10_Repo_Spec_Generic.IRepository<>), typeof(WebDataDemo.Option_10_Repo_Spec_Generic.EfRepository<>));

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

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

FluentMapper.Initialize(config =>
{
    config.AddMap(new CourseDtoMap());
});
