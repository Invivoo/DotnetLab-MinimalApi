using Domain.Contracts;
using Domain.Helpers;
using Host.Dto;
using Host.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Host.Endpoints;

internal static class CommunityEnpoints
{
    public static void AddCommunityEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("minimal-api/communities").WithTags("Minimal Api");

        group.MapGet("", GetAll);

        group.MapPost("", Post);

        group.MapGet("{id}", Get).WithName("MinimalApi.GetCommunityById");

        group.MapDelete("{id}", Delete);

        group.MapPatch("{id}/json-patch", JsonPatch);

        group.MapPatch("{id}/json-merge-patch", JsonMergePatch);
    }

    /// <summary>
    /// ¤ Get all communities ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the GetAll method.¤</description>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    /// <returns> The list of all the communities defined in Invivoo</returns>
    public static Ok<Community[]> GetAll(ICommunityService service)
    {
        return TypedResults.Ok(service.Get().Select(ModelToDtoMapper.ToDto).ToArray());
    }

    /// <summary>
    /// ¤ Create a new community ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the Post method.¤
    /// <br /><br /> 
    /// For example we might want to warn that: 
    /// <ul>
    /// <li>Name should have at least 5 characters </li>
    /// <li>Description should have at least 5 characters </li>
    /// </ul>
    /// </description>
    /// <param name="service">¤ This is not something that should appear in swagger¤</param>
    /// <param name="parameter"> ¤ Set the definition of the new community you whish to add ¤</param>
    /// <returns> The newly created community</returns>
    public static CreatedAtRoute<Community> Post(ICommunityService service, CommunityPostRequest parameter) 
    {
        var result = service.Post(parameter.ToModel()).ToDto();
        return TypedResults.CreatedAtRoute(result, "MinimalApi.GetCommunityById", new { id = result.Id });
    }

    /// <summary>
    /// ¤ Get a community by its identifier ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the Get method.¤</description>
    /// <param name="service"></param>
    /// <param name="id"></param>
    /// <returns>The requested community details.</returns>
    public static Results<Ok<Community>, NotFound> Get(ICommunityService service, Guid id)
    {
        try
        {
            return TypedResults.Ok(service.Get(id).ToDto());
        }
        catch
        {
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// ¤ Delete a community by its id ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the Delete method.¤</description>
    /// <param name="service"></param>
    /// <param name="id"></param>
    /// <returns>No content</returns>
    public static Results<NotFound, NoContent> Delete(ICommunityService service, Guid id)
    {
        try
        {
            service.Delete(id);
        }
        catch
        {
            return TypedResults.NotFound();
        }
        return TypedResults.NoContent();
    }


    /// <summary>
    /// ¤ Patch a community respecting the JSON Patch - RFC 6902 ¤
    /// </summary>
    /// <remarks> 
    /// Patch a community respecting the respecting the [JSON Patch - RFC 6902](https://datatracker.ietf.org/doc/html/rfc6902) ¤
    /// 
    /// Example of patch request:
    /// 
    ///     [
    ///        {
    ///             "op": "replace", 
    ///             "path": "/Name",
    ///             "value": "patched community name"
    ///         },
    ///         {
    ///             "op": "add",
    ///             "path": "/Manager/FirstName",
    ///             "value": "Patched manager first name"
    ///         },
    ///         {
    ///             "op": "add",
    ///             "path": "/Expertises",
    ///             "value": [{ "Name": "New Expertise from patch" }]
    ///         }
    ///     ]
    /// </remarks>
    /// <param name="service"></param>
    /// <param name="id"></param>
    /// <param name="patchRequest"></param>
    /// <returns></returns>
    public static Results<Ok<Community>, NotFound> JsonPatch(ICommunityService service, Guid id, JsonPatchDocument<Community> patchRequest)
    {
        try
        {
            return TypedResults.Ok(service.JsonPatch(id, patchRequest.ToModel()).ToDto());
        }
        catch
        {
            return TypedResults.NotFound();
        }
    }

    /// <summary>
    /// ¤ Patch a community respecting the JSON Merge Patch - RFC 7386 ¤
    /// </summary>
    /// <remarks>
    /// Patch a community respecting the [JSON Merge Patch - RFC 7386](https://datatracker.ietf.org/doc/html/rfc7386) ¤
    /// </remarks>
    /// <param name="service"></param>
    /// <param name="id"></param>
    /// <param name="patchRequest"></param>
    /// <returns></returns>
    public static Results<Ok<Community>, NotFound> JsonMergePatch(ICommunityService service, Guid id, CommunityPatchRequest patchRequest)
    {
        try
        {
            return TypedResults.Ok(service.JsonMergePatch(id, patchRequest.GetPropertiesToUpdate().ToModel()).ToDto());
        }
        catch
        {
            return TypedResults.NotFound();
        }
    }
}