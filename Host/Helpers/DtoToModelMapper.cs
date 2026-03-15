

using Domain.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations;
using Source = Host.Dto;

namespace Host.Helpers;

internal static class DtoToModelMapper
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
        => new(source.Id, source.Name, source.Description, source.Managers.ToModel(), source.Champions.ToModel(), source.Members.ToModel());

    public static JsonPatchDocument<Community> ToModel(this JsonPatchDocument<Source.Community> source)
    {
        var result = new JsonPatchDocument<Community>();
        foreach (var operation in source.Operations)
        {
            result.Operations.Add(new Operation<Community>(operation.op, operation.path, operation.from, operation.value));
        }
        return result;
    }

    public static IDictionary<string, object?> ToModel(this IDictionary<string, object?> source)
        => source.ToDictionary(kv => kv.Key, kv => kv.Value);

    public static CommunityPostRequest ToModel(this Source.CommunityPostRequest source)
        => new(source.Name, source.Description);
}


