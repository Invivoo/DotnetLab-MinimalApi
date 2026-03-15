
using DataAccess.Entities;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson.Operations;
using Source = Domain.Models;

namespace Domain.Helpers;

internal static class ModelToEntityMapper
{
    public static Community ToEntity(this Source.Community source)
    => new(source.Id, source.CreatedDate, source.Name, source.Description, source.Manager.ToEntity(), source.Expertises.ToEntity());

    public static Person[] ToEntity(this IList<Source.Person> source)
        => source.Select(model => model.ToEntity()).ToArray();

    public static Person ToEntity(this Source.Person source)
        => new(source.Id, source.FirstName, source.LastName);


    public static Expertise[] ToEntity(this IList<Source.Expertise> source)
        => source.Select(model => model.ToEntity()).ToArray();

    public static Expertise ToEntity(this Source.Expertise source)
        => new(source.Id, source.Name, source.Description, source.Managers.ToEntity(), source.Champions.ToEntity(), source.Members.ToEntity());

    public static JsonPatchDocument<Community> ToEntity(this JsonPatchDocument<Source.Community> source)
    {
        var result = new JsonPatchDocument<Community>();
        foreach (var operation in source.Operations)
        {
            result.Operations.Add(new Operation<Community>(operation.op, operation.path, operation.from, operation.value));
        }
        return result;
    }

    public static IDictionary<string, object?> ToEntity(this IDictionary<string, object?> source)
        => source.ToDictionary(kv => kv.Key, kv => kv.Value);
}

