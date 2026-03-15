namespace DataAccess.Entities
{
    public class Community(Guid id, DateOnly createdDate, string name, string description, Person manager, Expertise[] expertises)
    {
        public Community() : this(Guid.CreateVersion7(), DateOnly.FromDateTime(DateTime.Now), string.Empty, string.Empty, new Person(), []) { }
        public Guid Id { get; init; } = id;
        public DateOnly CreatedDate { get; set; } = createdDate;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public Person Manager { get; set; } = manager;
        public IList<Expertise> Expertises { get; set; } = expertises;
    }
}
