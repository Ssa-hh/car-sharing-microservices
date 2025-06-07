using Microsoft.EntityFrameworkCore;
using Ssa.CarSharing.Users.Domain.Users;
using System.Drawing;

namespace Ssa.CarSharing.Users.infrastructure.Database.Configurations;

internal class CarConfiguration : IEntityTypeConfiguration<Domain.Users.Cars.Car>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Users.Cars.Car> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Brand).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Model).HasMaxLength(100).IsRequired();
        builder.Property(c => c.ColorHexCode).HasMaxLength(7);
        // builder.Property(c => c.Color).HasConversion(a => a.ToArgb(), b => Color.FromArgb(b));
        builder.Ignore(c => c.Color);

        builder.HasOne(c=>c.Owner).WithMany(u => u.Cars).HasForeignKey(c => c.OwnerId).IsRequired();
    }
}
