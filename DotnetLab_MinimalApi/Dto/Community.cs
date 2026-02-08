using DotnetLab_MinimalApi.Services;

namespace DotnetLab_MinimalApi.Dto;

public class CommunityDto
{
    public CommunityDto(Guid id, DateOnly createdDate, string name, string description)
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

public interface ICommunityMapper
{
    CommunityDto FromDomain(Community community);
}

public class CommunityMapper : ICommunityMapper
{
    public CommunityDto FromDomain(Community community) => new(community.Id, community.CreatedDate, community.Name, community.Description);
}