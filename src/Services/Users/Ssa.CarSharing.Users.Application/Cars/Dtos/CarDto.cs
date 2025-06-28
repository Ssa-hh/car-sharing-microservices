using Ssa.CarSharing.Users.Domain.Users.Cars;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ssa.CarSharing.Users.Application.Cars.Dtos
{
    public class CarDto
    {
        public Guid Id { get; set; }
        public required string Brand { get; set; }

        public required string Model { get; set; }

        public required string ColorHexCode { get; set; }

        public short NumberOfSeats { get; set; }
        public static CarDto FromCar(Car car)
        {
            return new CarDto { Id = car.Id, Brand = car.Brand, Model = car.Model, ColorHexCode = car.ColorHexCode, NumberOfSeats = car.NumberOfSeats };
        }
    }
}
