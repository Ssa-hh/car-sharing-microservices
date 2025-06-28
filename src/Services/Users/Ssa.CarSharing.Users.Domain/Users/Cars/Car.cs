using Ssa.CarSharing.Common.Domain;
using System.Drawing;
using System.Text.RegularExpressions;
// using System.Drawing;

namespace Ssa.CarSharing.Users.Domain.Users.Cars;

public class Car : Entity
{
    // private int _colorRgb;
    public Car(Guid id, string brand, string model, short numberOfSeats, string colorHexCode, Guid ownerId) : base(id)
    {
        if (!string.IsNullOrWhiteSpace(colorHexCode) && !IsValidHexColor(colorHexCode))
            throw new ArgumentException($"The car color \"{colorHexCode}\" is not a valid hexadecimal color code");
        
        Brand = brand;
        Model = model;
        ColorHexCode = colorHexCode;
        OwnerId = ownerId;
        NumberOfSeats = numberOfSeats;
    }

    public string Brand { get; set; }

    public string Model { get; set; }

    public short NumberOfSeats { get; set; }

    // Added fro EF to map color c property to database column
    public string ColorHexCode { get; private set; }

    public Color Color  => string.IsNullOrWhiteSpace(ColorHexCode) ? Color.Empty : ColorTranslator.FromHtml(ColorHexCode);

    public Guid OwnerId { get; private set; }

    public User? Owner { get; private set; }

    public void SetColor(Color color)
    {
        ColorHexCode = ColorTranslator.ToHtml(color);
    }
    internal static Car Create(string brand, string model, short numberOfSeats, Color color, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentNullException(nameof(brand));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentNullException(nameof(model));
        if(numberOfSeats < 2) throw new ArgumentOutOfRangeException(nameof(numberOfSeats));
        if (ownerId == Guid.Empty) throw new ArgumentNullException(nameof(ownerId));

        return new Car(Guid.NewGuid(), brand, model, numberOfSeats, ColorTranslator.ToHtml(color), ownerId);
    }

    private static bool IsValidHexColor(string input)
    {
        return Regex.IsMatch(input, @"^#(?:[0-9a-fA-F]{3}){1,2}$");
    }
}