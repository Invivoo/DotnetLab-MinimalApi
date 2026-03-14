using System.ComponentModel;
using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Services;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using Microsoft.AspNetCore.Mvc;

namespace DotnetLab_MinimalApi.Controllers;

[ApiController]
[Tags("Controller")]
[Route("controller/communities")]
public class CommunityController(ICommunityMapper mapper) : ControllerBase
{
    /// <summary>
    /// ¤ Get all communities ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the GetAll method.¤</description>
    /// <param name="getService"></param>
    /// <returns> The list of all the communities defined in Invivoo</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<CommunityDto[]> GetAll([FromServices] ICommunityGetService getService) => Ok(getService.Get().Select(x => mapper.FromDomain(x)).ToArray());

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
    /// <param name="postService">¤ This is not something that should appear in swagger¤</param>
    /// <param name="parameter"> ¤ Set the definition of the new community you whish to add ¤</param>
    /// <returns> The newly created community</returns>
    [HttpPost]
    [ProducesResponseType<CommunityDto>(StatusCodes.Status201Created)]
    public ActionResult<CommunityDto> Post([FromServices] ICommunityPostService postService, [FromBody] CommunityPostParameter parameter)
    {
        var result = mapper.FromDomain(postService.Post(parameter));
        return CreatedAtRoute("Controller.GetCommunityById", new { id = result.Id }, result);
    }

    /// <summary>
    /// ¤ Get a community by its identifier ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the Get method.¤</description>
    /// <param name="getService"></param>
    /// <param name="id"></param>
    /// <returns>The requested community details.</returns>
    [HttpGet("{id}", Name = "Controller.GetCommunityById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<CommunityDto> Get([FromServices] ICommunityGetService getService, Guid id)
    {
        try
        {
            return Ok(mapper.FromDomain(getService.Get(id)));
        }
        catch
        {
            return NotFound();
        }
    }

    /// <summary>
    /// ¤ Delete a community by its id ¤
    /// </summary>
    /// <description>¤ Here we can add additional information for the Delete method.¤</description>
    /// <param name="service"></param>
    /// <param name="id"></param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete([FromServices] ICommunityDeleteService service, Guid id)
    {
        try
        {
            service.Delete(id);
            return NoContent();
        }
        catch
        {
            return NotFound();
        }
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
    [HttpPatch("{id}/json-patch")]
    [Consumes("application/json-patch+json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Community> JsonPatch([FromServices] ICommunityPatchService patchService, Guid id, [FromBody] JsonPatchDocument<Community> patchRequest)
    {
        try
        {
            return Ok(patchService.JsonPatch(id, patchRequest));
        }
        catch
        {
            return NotFound();
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
    [HttpPatch("{id}/json-merge-patch")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Community> JsonMergePatch([FromServices] ICommunityPatchService patchService, Guid id, [FromBody] CommunityPatchParameter patchRequest)
    {
        try
        {
            return Ok(patchService.JsonMergePatch(id, patchRequest)); 
        }
        catch
        {
            return NotFound();
        }
    }
}
