using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using DotnetLab_MinimalApi.Dto;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;

namespace DotnetLab_MinimalApi.Services;

public record Person(Guid Id, string FirstName, string LastName)
{
  public Person(string firstName, string lastName) : this(Guid.CreateVersion7(), firstName, lastName)
  {
  }
}

public record Expertise(Guid Id, string Name, string Description, Person[] Managers, Person[] Champions, Person[] Members)
{
    [JsonConstructor]
    public Expertise(string name, string description) : this(Guid.CreateVersion7(), name, description, [], [], [])
    {
    }
}

public class Community(Guid id, DateOnly createdDate, string name, string description)
{
    public Guid Id { get; init; } = id;
    public DateOnly CreatedDate { get; set; } = createdDate;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public Person Manager { get; set; } = new Person("Manager", "Example");
    public Expertise[] Expertises { get; set; } = [];
}

/// <summary>
/// $$ A post parameter $$
/// </summary>
/// <param name="Name"> $$ The name of the new Community.$$<default> test</default></param>
/// <param name="Description">$$The description of the new community$$</param>
public record CommunityPostParameter(
    [property: Required, MinLength(5), DefaultValue("NewCommunityName")] string Name,
    [property: Required, MinLength(5), DefaultValue("NewCommunityDescription")] string Description);

public interface ICommunityGetService
{
    IEnumerable<Community> Get();
    Community Get(Guid id);
}
public interface ICommunityPostService
{
    Community Post(CommunityPostParameter community);
}
public interface ICommunityDeleteService
{
    void Delete(Guid id);
}

public interface ICommunityRepository
{
    IEnumerable<Community> GetAll();
    void Add(Community community);
    Community Get(Guid id);
    void Delete(Guid id);
}
public interface ICommunityPatchService
{
    Community JsonPatch(Guid id, JsonPatchDocument<Community> community);
    Community JsonMergePatch(Guid id, CommunityPatchParameter community);
}

public class CommunityGetService(ICommunityRepository repo) : ICommunityGetService
{
    public IEnumerable<Community> Get() => repo.GetAll();
    public Community Get(Guid id) => repo.Get(id);
}

public class CommunityPostService(ICommunityRepository repo) : ICommunityPostService
{
    public Community Post(CommunityPostParameter community)
    {
        var result = new Community(Guid.CreateVersion7(), DateOnly.FromDateTime(DateTime.Now), community.Name, community.Description);
        repo.Add(result);
        return result;
    }
}

public class CommunityDeleteService(ICommunityRepository repo) : ICommunityDeleteService
{
    public void Delete(Guid id) =>  repo.Delete(id);
}

public class InMemoryCommunityRepository : ICommunityRepository
{
    private static readonly DateOnly OriginalCreationDate = new(2016, 1, 1);
    private static readonly DateTimeOffset OriginalCreationDateTime = new(OriginalCreationDate, new TimeOnly(11, 15), new TimeSpan(1, 0, 0));
    private static Community DefaultCommunity(string name, string description) =>
        new (Guid.CreateVersion7(OriginalCreationDateTime), OriginalCreationDate, name, description);

    private readonly IList<Community> _communities =
    [
        DefaultCommunity("Design & Code", "Java, Python, C++, C#, Front End"),
        DefaultCommunity("Deliver & Run", "DevOps, Support & Production Applicative"),
        DefaultCommunity("Data & IA", "Big Data Engineering, Data Science, Analystices & BI"),
        DefaultCommunity("Be & Do Agile !", "Agilité, Craftsmanship, Product management"),
        DefaultCommunity("Beyond Finance", "Post-trade, Risk & Regulatory Analytics, Front Office")
    ];
    
    public IEnumerable<Community> GetAll() => _communities;
    public void Add(Community community) => _communities.Add(community);

    public Community Get(Guid id) => _communities.Single(x => x.Id == id);

    public void Delete(Guid id)
    {
        var community = Get(id);
        _communities.Remove(community);
    }
}

public class CommunityPatchService(ICommunityRepository repo) : ICommunityPatchService
{
    public Community JsonPatch(Guid id, JsonPatchDocument<Community> community)
    {
        var existingCommunity = repo.Get(id);
        community.ApplyTo(existingCommunity, jsonPatchError =>
        {
            throw new Exception(jsonPatchError.ErrorMessage);
        });
        return existingCommunity;
    }

    public Community JsonMergePatch(Guid id, CommunityPatchParameter community)
    {
        var existingCommunity = repo.Get(id);
        community.ApplyTo(existingCommunity);
        return existingCommunity;
    }
}