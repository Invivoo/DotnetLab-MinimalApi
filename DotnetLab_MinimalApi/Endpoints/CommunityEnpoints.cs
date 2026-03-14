using System.ComponentModel;
using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace DotnetLab_MinimalApi.Endpoints;

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
    public static Ok<CommunityDto[]> GetAll(ICommunityGetService service, ICommunityMapper mapper)
    {
        return TypedResults.Ok(service.Get().Select(x => mapper.FromDomain(x)).ToArray());
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
    /// <param name="mapper"></param>
    /// <param name="parameter"> ¤ Set the definition of the new community you whish to add ¤</param>
    /// <returns> The newly created community</returns>
    public static CreatedAtRoute<CommunityDto> Post(ICommunityPostService service, ICommunityMapper mapper, CommunityPostParameter parameter) 
    {
        var result = mapper.FromDomain(service.Post(parameter));
        return TypedResults.CreatedAtRoute(result, "MinimalApi.GetCommunityById", new { id = result.Id });
    }

    /// <summary>
    /// ¤ Get a community by its identifier ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the Get method.¤</description>
    /// <param name="service"></param>
    /// <param name="mapper"></param>
    /// <param name="id"></param>
    /// <returns>The requested community details.</returns>
    public static Results<Ok<CommunityDto>, NotFound> Get(ICommunityGetService service, ICommunityMapper mapper, Guid id)
    {
        try
        {
            return TypedResults.Ok(mapper.FromDomain(service.Get(id)));
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
    public static Results<NotFound, NoContent> Delete(ICommunityDeleteService service, Guid id)
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
    /// <param name="patchService"></param>
    /// <param name="id"></param>
    /// <param name="patchRequest"></param>
    /// <returns></returns>
    public static Results<Ok<Community>, NotFound> JsonPatch(ICommunityPatchService patchService, Guid id, JsonPatchDocument<Community> patchRequest)
    {
        try
        {
            return TypedResults.Ok(patchService.JsonPatch(id, patchRequest));
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
    /// <param name="patchService"></param>
    /// <param name="id"></param>
    /// <param name="patchRequest"></param>
    /// <returns></returns>
    public static Results<Ok<Community>, NotFound> JsonMergePatch(ICommunityPatchService patchService, Guid id, CommunityPatchParameter patchRequest)
    {
        try
        {
            return TypedResults.Ok(patchService.JsonMergePatch(id, patchRequest));
        }
        catch
        {
            return TypedResults.NotFound();
        }
    }
}