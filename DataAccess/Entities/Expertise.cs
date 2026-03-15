using System.Text.Json.Serialization;

namespace DataAccess.Entities
{
    public class Expertise(Guid id, string name, string description, Person[] managers, Person[] champions, Person[] members)
    {
        public Expertise() : this(Guid.CreateVersion7(), string.Empty, string.Empty, [], [], [])
        {
        }

        [JsonConstructor]
        public Expertise(string name, string description) : this(Guid.CreateVersion7(), name, description, [], [], [])
        {
        }

        public Guid Id { get; } = id;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public IList<Person> Managers { get; set; } = managers;
        public IList<Person> Champions { get; set; } = champions;
        public IList<Person> Members { get; set; } = members;
    }
}