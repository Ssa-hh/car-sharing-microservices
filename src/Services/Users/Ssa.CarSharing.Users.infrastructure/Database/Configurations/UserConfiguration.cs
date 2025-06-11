using Microsoft.EntityFrameworkCore;
using Ssa.CarSharing.Users.Domain.Users;

namespace Ssa.CarSharing.Users.infrastructure.Database.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<Domain.Users.User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Users.User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName).HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.LastName).HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            // builder.Property(u => u.IdentityId).IsRequired();

            builder.HasMany(u => u.Cars)
                .WithOne(c => c.Owner)
                .HasForeignKey(c => c.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
