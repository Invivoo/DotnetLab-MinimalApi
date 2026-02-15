using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetLab_MinimalApi.Controllers;

[ApiController]
[Route("controller/communities")]
public class CommunityController(ICommunityMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<CommunityDto>(StatusCodes.Status200OK)]
    public IActionResult Get([FromServices] ICommunityGetService getService) => Ok(getService.Get().Select(x => mapper.FromDomain(x)).ToArray());

    [HttpPost]
    [ProducesResponseType<CommunityDto>(StatusCodes.Status201Created)]
    public ActionResult<CommunityDto> Post([FromServices] ICommunityPostService postService, [FromBody] CommunityPostParameter community) => mapper.FromDomain(postService.Post(community));

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public void Delete([FromServices] ICommunityDeleteService service, Guid id) => service.Delete(id);
}
