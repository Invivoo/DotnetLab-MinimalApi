using DataAccess.Contracts;
using Domain.Contracts;
using Domain.Helpers;
using Domain.Models;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace Domain.Services
{
    public class CommunityService(ICommunityRepository repo) : ICommunityService
    {
        public void Delete(Guid id) => repo.Delete(id);

        public IEnumerable<Community> Get() => repo.GetAll().Select(EntityToModelMapper.ToModel);
        public Community Get(Guid id) => repo.Get(id).ToModel();

        public Community JsonPatch(Guid id, JsonPatchDocument<Community> patchRequest) => repo.JsonPatch(id, patchRequest.ToEntity()).ToModel();

        public Community JsonMergePatch(Guid id, IDictionary<string, object?> patchRequest) => repo.JsonMergePatch(id, patchRequest.ToEntity()).ToModel();

        public Community Post(CommunityPostRequest community)
        {
            var result = new Community(Guid.CreateVersion7(), DateOnly.FromDateTime(DateTime.Now), community.Name, community.Description, DefaultManager(community.Name), []);
            repo.Add(result.ToEntity());
            return result;
        }

        private static Person DefaultManager(string communityName) => new($"{communityName} Manager", "Example");
    }
}
