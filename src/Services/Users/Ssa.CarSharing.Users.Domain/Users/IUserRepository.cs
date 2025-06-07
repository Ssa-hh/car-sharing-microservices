using Ssa.CarSharing.Users.Domain.Users.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Domain.Users
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id, bool withCars = false);
        Task<User?> GetByEmailAsync(string email, bool withCars = false);
        Task AddAsync(User user);
        Task<bool> ExistsAsync(string email);

        Task AddCarAsync(Car car);

        /*
        Task DeleteAsync(Guid id);
        Task<bool> ExistsByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        */
    }
}
