namespace ECommerce.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedAt { get; protected set; }

    public DateTimeOffset? UpdatedAt { get; protected set; }

    public string? CreatedBy { get; protected set; }

    public string? UpdatedBy { get; protected set; }

    public bool IsDeleted { get; private set; }

    public void MarkCreated(string? user)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        CreatedBy = user;
    }

    public void MarkUpdated(string? user)
    {
        UpdatedAt = DateTimeOffset.UtcNow;
        UpdatedBy = user;
    }

    public void MarkAsDeleted(string? user)
    {
        IsDeleted = true;
        MarkUpdated(user);
    }

    public void Restore(string? user)
    {
        IsDeleted = false;
        MarkUpdated(user);
    }
}