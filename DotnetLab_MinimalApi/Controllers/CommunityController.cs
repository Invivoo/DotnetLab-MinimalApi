using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Services;
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
        service.Delete(id);
        return NoContent();
    }
}
