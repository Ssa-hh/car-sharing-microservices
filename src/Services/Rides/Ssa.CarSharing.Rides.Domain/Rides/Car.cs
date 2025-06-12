using System.Drawing;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ssa.CarSharing.Rides.Domain.Rides;

public record Car
{
    public Car(string brand, string model, string colorHexCode, short numberOfSeats)
    {
        if (!string.IsNullOrWhiteSpace(colorHexCode) && !IsValidHexColor(colorHexCode))
            throw new ArgumentException($"The car color \"{colorHexCode}\" is not a valid hexadecimal color code");

        Brand = brand;
        Model = model;
        ColorHexCode = colorHexCode;
        NumberOfSeats = numberOfSeats;
    }

    public string Brand { get;}

    public string Model { get;}

    public string ColorHexCode { get;}

    public short NumberOfSeats { get;}

    private static bool IsValidHexColor(string input)
    {
        return Regex.IsMatch(input, @"^#(?:[0-9a-fA-F]{3}){1,2}$");
    }
}
