using Microsoft.EntityFrameworkCore;
using Ssa.CarSharing.Users.Domain.Users;
using Ssa.CarSharing.Users.Domain.Users.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.infrastructure.Database.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Users.User?> GetByIdAsync(Guid id, bool withCars = false)
        {
            var query = _context.Users.AsQueryable();
            if (withCars)
                query = query.Include(u => u.Cars);

            return await query.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Domain.Users.User?> GetByEmailAsync(string email, bool withCars = false)
        {
            var query = _context.Users.AsQueryable();
            if (withCars)
                query = query.Include(u => u.Cars);

            return await query.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(Domain.Users.User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> ExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public Task AddCarAsync(Car car)
        {
            _context.Entry(car).State = EntityState.Added;

            return Task.CompletedTask;
        }
    }
}
