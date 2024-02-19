namespace EventManagement.Domain;

public abstract class EntityBase : IId
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public interface IId
{
    Guid Id { get; set; }
}