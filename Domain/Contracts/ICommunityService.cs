using Domain.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Domain.Contracts;

public interface ICommunityService
{
    IEnumerable<Community> Get();
    Community Get(Guid id);
    Community Post(CommunityPostRequest community);
    void Delete(Guid id);
    Community JsonPatch(Guid id, JsonPatchDocument<Community> community);
    Community JsonMergePatch(Guid id, IDictionary<string, object?> community);
}
