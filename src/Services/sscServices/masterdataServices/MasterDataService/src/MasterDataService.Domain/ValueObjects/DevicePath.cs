namespace MasterDataService.Domain.ValueObjects;

public sealed record DevicePath
{
    public string Value { get; }

    private DevicePath(string value) => Value = value;

    public static DevicePath Create(string path)
    {
        if (path?.Length > 1000)
            throw new ArgumentException("Device path cannot exceed 1000 characters.", nameof(path));

        return new DevicePath(path ?? string.Empty);
    }

    public static DevicePath Empty() => new(string.Empty);
}
