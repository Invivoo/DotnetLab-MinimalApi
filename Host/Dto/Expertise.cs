namespace Host.Dto;

public record Expertise(Guid Id, string Name, string Description, Person[] Managers, Person[] Champions, Person[] Members);
