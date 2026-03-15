using System.Text.Json.Serialization;

namespace Domain.Models;

public record Expertise(Guid Id, string Name, string Description, Person[] Managers, Person[] Champions, Person[] Members)
{
    [JsonConstructor]
    public Expertise(string name, string description) : this(Guid.CreateVersion7(), name, description, [], [], [])
    {
    }
}