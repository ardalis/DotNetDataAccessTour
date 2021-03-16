using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebDataDemo.Model;

namespace WebDataDemo.Data.Config
{
    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
              .HasMaxLength(ColumnConstants.DEFAULT_NAME_LENGTH);
            builder.Property(x => x.Slug)
              .HasMaxLength(ColumnConstants.DEFAULT_NAME_LENGTH);

            builder.HasData(new { Id = 1,
                Title = "SOLID Principles for C# Developers",
                Slug = "csharp-solid-principles"
                });
            builder.HasData(new
            {
                Id = 2,
                Title = "Design Patterns Overview",
                Slug = "design-patterns-overview"
            });
            builder.HasData(new
            {
                Id = 3,
                Title = "Domain-Driven Design Fundamentals",
                Slug = "domain-driven-design-fundamentals"
            });
        }
    }
}
