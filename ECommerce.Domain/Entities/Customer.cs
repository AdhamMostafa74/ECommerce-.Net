namespace ECommerce.Domain.Entities;

public class Customer : BaseEntity
{
    public Guid ApplicationUserId { get; private set; }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public DateOnly DateOfBirth { get; private set; }
    public string Gender { get; private set; } = string.Empty;

    private Customer() { }

    private Customer(
        Guid applicationUserId,
        string firstName,
        string lastName,
        string phoneNumber,
        DateOnly dateOfBirth,
        string gender)
    {
        ApplicationUserId = applicationUserId;
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber.Trim();
        DateOfBirth = dateOfBirth;
        Gender = gender.Trim();
    }

    public static Customer Create(
        Guid applicationUserId,
        string firstName,
        string lastName,
        string phoneNumber,
        DateOnly dateOfBirth,
        string gender)
    {
        return new Customer(
            applicationUserId,
            firstName,
            lastName,
            phoneNumber,
            dateOfBirth,
            gender);
    }
}

