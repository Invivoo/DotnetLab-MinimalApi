using System.Runtime.CompilerServices;

namespace Domain.Models;

public record Community(Guid Id, DateOnly CreatedDate, string Name, string Description, Person Manager, Expertise[] Expertises);
