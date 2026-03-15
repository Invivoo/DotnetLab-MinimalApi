namespace DataAccess.Entities
{
    public class Person(Guid id, string firstName, string lastName)
    {
        public Person() : this(Guid.CreateVersion7(), string.Empty, string.Empty)
        {
        }

        public Person(string firstName, string lastName) : this(Guid.CreateVersion7(), firstName, lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id { get; init; } = id;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;
    }
}
