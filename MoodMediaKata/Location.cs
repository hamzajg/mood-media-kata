namespace MoodMediaKata;

public class Location(string name, string address, Device device)
{
    public string Name { get; private set; } = name;
    public string Address { get; private set; } = address;
    public Device Device { get; private set; } = device;
    public long Id { get; } = IdGenerator.NextId(nameof(Location));
}