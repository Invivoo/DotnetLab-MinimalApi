using Host.Dto;
using Source = Domain.Models;

namespace Host.Helpers;

internal static class ModelToDtoMapper
{
    public static Community ToDto(this Source.Community source)
        => new() 
        { 
            Id = source.Id, 
            CreatedDate = source.CreatedDate,
            Name = source.Name,
            Manager = source.Manager.ToDto(),
            Expertises = source.Expertises.ToDto(), 
            Description = source.Description
        };

    public static Person[] ToDto(this IList<Source.Person> source)
        => source.Select(model => model.ToDto()).ToArray();

    public static Person ToDto(this Source.Person source)
        => new(source.Id, source.FirstName, source.LastName);


    public static Expertise[] ToDto(this IList<Source.Expertise> source)
        => source.Select(model => model.ToDto()).ToArray();

    public static Expertise ToDto(this Source.Expertise source)
        => new(source.Id, source.Name, source.Description, source.Managers.ToDto(), source.Champions.ToDto(),source.Members.ToDto());

}

