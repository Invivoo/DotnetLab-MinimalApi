using System.Runtime.CompilerServices;
using DotnetLab_MinimalApi.Services;

namespace DotnetLab_MinimalApi.Dto;


/// <summary>
/// $ A community in Invivoo $ 
/// </summary>
/// <param name="id"> $ The Id of the community $</param>
/// <param name="createdDate"> $ The created date of the community $</param>
/// <param name="name">$ The name of the community$</param>
/// <param name="description">$ The description of the community$</param>
public class CommunityDto(Guid id, DateOnly createdDate, string name, string description)
{
    /// <summary>
    /// $ The Id of the community $
    /// </summary>
    /// <example> 3fa85f64-5717-4562-b3fc-2c963f66afa6 </example>
    public Guid Id { get; init; } = id;
    /// <summary>
    /// $ The created date of the community $
    /// </summary>
    /// <example> 2024-06-01 </example>
    public DateOnly CreatedDate { get; set; } = createdDate;
    /// <summary>
    /// $ The name of the community$
    /// </summary>
    /// <example> A nice community name </example>
    public string Name { get; set; } = name;
    /// <summary>
    /// $ The description of the community$
    /// </summary>
    /// <example> A description presenting the community in a few words </example>
    public string Description { get; set; } = description;
    /// <summary>
    /// $ The manager off the community$
    /// </summary>
    /// <example> { 'LastName' : 'Manager', 'FirstName' : 'Example' } </example>
    public required Person Manager { get; set; }

    /// <summary>
    /// $ The expertises in this community$
    /// </summary>
    public required Expertise[] Expertises { get; set; }
}


public interface ICommunityMapper
{
    CommunityDto FromDomain(Community community);
}

public class CommunityMapper : ICommunityMapper
{
    public CommunityDto FromDomain(Community community) => new(community.Id, community.CreatedDate, community.Name, community.Description)
    {
        Manager = community.Manager,
        Expertises = community.Expertises
    };
}

public class CommunityPatchParameter : PatchRequestBase
{
    public string? Name { get => GetProperty<string>(); set => SetProperty(value); }
    public string? Description { get => GetProperty<string>(); set => SetProperty(value); }
}

public abstract class PatchRequestBase
{
    public void ApplyTo<T>(T target)
    {
        var targetType = typeof(T);
        foreach (var property in PropertiesToUpdate)
        {
            var targetProperty = targetType.GetProperty(property.Key);
            if (targetProperty != null && targetProperty.CanWrite)
            {
                targetProperty.SetValue(target, property.Value);
            }
        }
    }

    protected T? GetStructProperty<T>([CallerMemberName] string propertyName = null!)
        where T : struct
        => PropertiesToUpdate.TryGetValue(propertyName, out var value) ? value as T? : null;

    protected T? GetProperty<T>([CallerMemberName] string propertyName = null!) where T : class
        => PropertiesToUpdate.TryGetValue(propertyName, out var value) ? value as T : null;
    protected void SetProperty<T>(T value, [CallerMemberName] string propertyName = null!)
        => PropertiesToUpdate[propertyName] = value;

    private Dictionary<string, object?> PropertiesToUpdate { get; } = [];
}