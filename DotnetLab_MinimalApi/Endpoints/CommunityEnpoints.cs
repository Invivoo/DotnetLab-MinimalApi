using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Services;

namespace DotnetLab_MinimalApi.Endpoints;

public static class CommunityEnpoints
{
    public static void AddCommunityEndpoints(this WebApplication app)
    {
        app.MapGet("minimal-api/communities", (ICommunityGetService service, ICommunityMapper mapper) => service.Get().Select(x => mapper.FromDomain(x)).ToArray());
        app.MapPost("minimal-api/communities", (ICommunityPostService service, ICommunityMapper mapper, CommunityPostParameter forecast) => mapper.FromDomain(service.Post(forecast)));
        app.MapDelete("minimal-api/communities/{id}", (ICommunityDeleteService service, Guid id) => service.Delete(id));
    }
}