using System.Drawing;
using Ssa.CarSharing.Common.Domain;
using Ssa.CarSharing.Users.Domain.Users.Cars;
using Ssa.CarSharing.Users.Domain.Users.Events;

namespace Ssa.CarSharing.Users.Domain.Users
{
    public class User : AggregateRoot
    {
        private readonly List<Car> _cars = new List<Car>();
        private User(Guid id, string firstName, string lastName, string email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IdentityId = Guid.NewGuid().ToString();
            _cars = new List<Car>();
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Email { get; private set; }

        public string IdentityId { get; set; }

        public IReadOnlyList<Car> Cars => _cars.AsReadOnly();

        public static User Create(string firstName, string lastName, string email)
        {

            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName));
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            User user = new User(Guid.NewGuid(), firstName, lastName, email);

            user.AddDomainEvent(new UserCreatedDomainEvent(user.Id));

            return user;
        }

        public Car AddCar(string brand, string model, Color color, Guid ownerId)
        {
            if (ownerId != Id) throw new InvalidOperationException("Car does not belong to this user.");

            Car car = Car.Create(brand, model, color, ownerId);
            _cars.Add(car);

            return car;
        }

        public Result RemoveCar(Guid carId)
        {
            var carToRemove = Cars.FirstOrDefault( c=> c.Id == carId);

            if (carToRemove == null)
                return Result.Failure<Car>(Error.NotFound("Cars.NotFound", $"The car with id equal to {carId} does not exist or does not belong to this user."));

            _cars.Remove(carToRemove);

            return Result.Success();
        }

        public Result UpdateCar(Guid carId, string brand, string model, Color color)
        {
            var carToUpdate = Cars.FirstOrDefault(c=>  c.Id == carId);

            if (carToUpdate == null)
                return Result.Failure(Error.NotFound("Cars.NotFound", $"The car with id equal to {carId} does not exist or does not belong to this user."));

            carToUpdate.Brand = brand;
            carToUpdate.Model = model;
            carToUpdate.SetColor(color);

            return Result.Success();
        }
    }
}
