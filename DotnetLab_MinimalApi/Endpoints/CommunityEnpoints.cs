using DotnetLab_MinimalApi.Dto;
using DotnetLab_MinimalApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DotnetLab_MinimalApi.Endpoints;

public static class CommunityEnpoints
{
    public static void AddCommunityEndpoints(this WebApplication app)
    {
        //var group = app.MapGroup("minimal-api/communities").WithTags("minimal-api");

        app.MapGet("minimal-api/communities", (ICommunityGetService service, ICommunityMapper mapper) => TypedResults.Ok(service.Get().Select(x => mapper.FromDomain(x)).ToArray()));
        app.MapPost("minimal-api/communities", (ICommunityPostService service, ICommunityMapper mapper, CommunityPostParameter forecast) => mapper.FromDomain(service.Post(forecast))).Produces<CommunityDto>(StatusCodes.Status201Created);
        app.MapDelete("minimal-api/communities/{id}", Results<NotFound, NoContent> (ICommunityDeleteService service, Guid id) =>
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
        });
    }
}