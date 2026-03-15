using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Host.Dto;

/// <summary>
/// $$ A post parameter $$
/// </summary>
public class CommunityPostRequest
{
    /// <summary>
    /// $$ The name of the new Community.$$
    /// </summary>
    [Required, MinLength(5), DefaultValue("NewCommunityName")]
    public required string Name { get; init; }
    /// <summary>
    /// $$The description of the new community$$
    /// </summary>
    [Required, MinLength(5), DefaultValue("NewCommunityDescription")] 
    public required string Description { get; init; }
}
