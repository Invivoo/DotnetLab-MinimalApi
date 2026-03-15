using DataAccess.Contracts;
using DataAccess.Entities;
using DataAccess.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DataAccess.Repositories.InMemory
{
    public class InMemoryCommunityRepository : ICommunityRepository
    {
        public IEnumerable<Community> GetAll() => _communities;
        public void Add(Community community) => _communities.Add(community);

        public Community Get(Guid id) => _communities.Single(x => x.Id == id);

        public void Delete(Guid id)
        {
            var community = Get(id);
            _communities.Remove(community);
        }

        public Community JsonPatch(Guid id, JsonPatchDocument<Community> patchRequest)
        {
            var result = Get(id);
            patchRequest.ApplyTo(result, jsonPatchError => throw new Exception(jsonPatchError.ErrorMessage));
            return result;
        }

        public Community JsonMergePatch(Guid id, IDictionary<string, object?> patchRequest)
        {
            var result = Get(id);
            patchRequest.ApplyTo(result);
            throw new NotImplementedException();
        }

        private readonly IList<Community> _communities =
        [
            DefaultCommunity("Design & Code", "Java, Python, C++, C#, Front End"),
            DefaultCommunity("Deliver & Run", "DevOps, Support & Production Applicative"),
            DefaultCommunity("Data & IA", "Big Data Engineering, Data Science, Analystices & BI"),
            DefaultCommunity("Be & Do Agile !", "Agilité, Craftsmanship, Product management"),
            DefaultCommunity("Beyond Finance", "Post-trade, Risk & Regulatory Analytics, Front Office")
        ];

        private static readonly DateOnly OriginalCreationDate = new(2016, 1, 1);
        private static readonly DateTimeOffset OriginalCreationDateTime = new(OriginalCreationDate, new TimeOnly(11, 15), new TimeSpan(1, 0, 0));
        private static Community DefaultCommunity(string name, string description) =>
            new(Guid.CreateVersion7(OriginalCreationDateTime), OriginalCreationDate, name, description, new Person($"{name} Manager", "Example"), []);
    }
}
