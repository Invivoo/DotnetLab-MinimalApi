using DataAccess.Entities;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DataAccess.Contracts;

public interface ICommunityRepository
{
    IEnumerable<Community> GetAll();
    void Add(Community community);
    Community Get(Guid id);
    void Delete(Guid id);

    Community JsonPatch(Guid id, JsonPatchDocument<Community> patchRequest);
    Community JsonMergePatch(Guid id, IDictionary<string, object?> patchRequest);
}
