using Domain.Models;
using Source = DataAccess.Entities;
namespace Domain.Helpers;

internal static class EntityToModelMapper
{
    public static Community ToModel(this Source.Community source)
    => new(source.Id, source.CreatedDate, source.Name, source.Description, source.Manager.ToModel(), source.Expertises.ToModel());

    public static Person[] ToModel(this IList<Source.Person> source)
        => source.Select(model => model.ToModel()).ToArray();

    public static Person ToModel(this Source.Person source)
        => new(source.Id, source.FirstName, source.LastName);


    public static Expertise[] ToModel(this IList<Source.Expertise> source)
        => source.Select(model => model.ToModel()).ToArray();

    public static Expertise ToModel(this Source.Expertise source)
        => new(source.Id, source.Name, source.Description, source.Managers.ToModel(), source.Champions.ToModel(),source.Members.ToModel());

}

