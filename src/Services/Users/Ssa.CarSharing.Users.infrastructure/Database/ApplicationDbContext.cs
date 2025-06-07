using Microsoft.EntityFrameworkCore;
using Ssa.CarSharing.Users.Application.Data;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.Domain.Users.Cars;
using Ssa.CarSharing.Users.infrastructure.Database.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.infrastructure.Database
{
    internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
    {
        internal DbSet<User> Users { get; set; }

        internal DbSet<Car> Cars { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Todo Schema configuration

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
