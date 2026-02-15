using DotnetLab_MinimalApi.Services;

namespace DotnetLab_MinimalApi.Dto;

public class CommunityDto(Guid id, DateOnly createdDate, string name, string description)
{
    public Guid Id { get; init; } = id;
    public DateOnly CreatedDate { get; set; } = createdDate;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
}

public interface ICommunityMapper
{
    CommunityDto FromDomain(Community community);
}

public class CommunityMapper : ICommunityMapper
{
    public CommunityDto FromDomain(Community community) => new(community.Id, community.CreatedDate, community.Name, community.Description);
}