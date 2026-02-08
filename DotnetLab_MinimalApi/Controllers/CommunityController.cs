using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotnetLab_MinimalApi.Controllers;

[ApiController]
[Route("controller/communities")]
public class CommunityController(ICommunityMapper mapper, ICommunityGetService getService, ICommunityPostService postService, ICommunityDeleteService service) : ControllerBase
{
    [HttpGet]
    public IEnumerable<CommunityDto> Get() => getService.Get().Select(x => mapper.FromDomain(x)).ToArray();

    [HttpPost]
    public CommunityDto Post([FromBody] CommunityPostParameter community) => mapper.FromDomain(postService.Post(community));

    [HttpDelete("{id}")]
    public void Delete(Guid id) => service.Delete(id);
}
