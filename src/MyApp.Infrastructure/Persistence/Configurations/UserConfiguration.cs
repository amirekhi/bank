// MyApp.Infrastructure/Persistence/Configurations/UserConfiguration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Users;

namespace MyApp.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name).IsRequired().HasMaxLength(200);

        builder.OwnsOne(u => u.Email, e =>
        {
            e.Property(p => p.Value).HasColumnName("Email").IsRequired().HasMaxLength(320);
        });
    }
}