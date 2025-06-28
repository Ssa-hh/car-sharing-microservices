using Ssa.CarSharing.Users.Application.Cars.Dtos;
using Ssa.CarSharing.Users.Domain.Users;

namespace Ssa.CarSharing.Users.Application.Users.Dtos;

public class UserDto
{
    public Guid Id { get; init; }

    public required string Email { get; init; }

    public required string FirstName { get; init; }

    public required string LastName { get; init; }

    public List<CarDto> Cars { get; set; }

    public static UserDto FromUser(User user)
    {
        return new UserDto { Id = user.Id, Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Cars = user.Cars.Select(c => CarDto.FromCar(c)).ToList() };
    }
}
