namespace Domain.Models;

public record Person(Guid Id, string FirstName, string LastName)
{
    public Person(string firstName, string lastName) : this(Guid.CreateVersion7(), firstName, lastName)
    {
    }
}
