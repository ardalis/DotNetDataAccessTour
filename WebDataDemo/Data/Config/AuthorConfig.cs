using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebDataDemo.Model;

namespace WebDataDemo.Data.Config;

public class AuthorConfig : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
          .HasMaxLength(ColumnConstants.DEFAULT_NAME_LENGTH);

        builder.HasData(new { Id = 1, Name = "Steve Smith" });
        builder.HasData(new { Id = 2, Name = "Julia Lerman" });
    }
}
