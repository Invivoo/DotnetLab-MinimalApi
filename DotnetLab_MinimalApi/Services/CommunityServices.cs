namespace DotnetLab_MinimalApi.Services;

public class Community
{
    public Community(Guid id, DateOnly createdDate, string name, string description)
    {
        Id = id;
        CreatedDate = createdDate;
        Name = name;
        Description = description;
    }

    public Guid Id { get; init; }
    public DateOnly CreatedDate { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public record CommunityPostParameter(string Name, string Description);

public interface ICommunityGetService
{
    IEnumerable<Community> Get();
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

public class CommunityGetService(ICommunityRepository repo) : ICommunityGetService
{
    public IEnumerable<Community> Get() => repo.GetAll();
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