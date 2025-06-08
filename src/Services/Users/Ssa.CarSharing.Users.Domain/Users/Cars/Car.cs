using Ssa.CarSharing.Common.Domain;
using System.Drawing;
// using System.Drawing;

namespace Ssa.CarSharing.Users.Domain.Users.Cars;

public class Car : Entity
{
    // private int _colorRgb;
    public Car(Guid id, string brand, string model, string colorHexCode, Guid ownerId) : base(id)
    {
        Brand = brand;
        Model = model;
        ColorHexCode = colorHexCode;
        OwnerId = ownerId;
    }

    public string Brand { get; set; }

    public string Model { get; set; }

    // Added fro EF to map color c property to database column
    public string ColorHexCode { get; private set; }

    public Color Color  => string.IsNullOrWhiteSpace(ColorHexCode) ? Color.Empty : ColorTranslator.FromHtml(ColorHexCode);

    public Guid OwnerId { get; private set; }

    public User? Owner { get; private set; }

    public void SetColor(Color color)
    {
        ColorHexCode = ColorTranslator.ToHtml(color);
    }
    internal static Car Create(string brand, string model, Color color, Guid ownerId)
    {
        if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentNullException(nameof(brand));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentNullException(nameof(model));
        if (ownerId == Guid.Empty) throw new ArgumentNullException(nameof(ownerId));

        return new Car(Guid.NewGuid(), brand, model, ColorTranslator.ToHtml(color), ownerId);
    }
}