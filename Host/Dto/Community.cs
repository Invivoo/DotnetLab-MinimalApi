namespace Host.Dto;


/// <summary>
/// $ A community in Invivoo $ 
/// </summary>
public class Community
{
    /// <summary>
    /// $ The Id of the community $
    /// </summary>
    /// <example> 3fa85f64-5717-4562-b3fc-2c963f66afa6 </example>
    public Guid Id { get; init; }
    /// <summary>
    /// $ The created date of the community $
    /// </summary>
    /// <example> 2024-06-01 </example>
    public DateOnly CreatedDate { get; init; }
    /// <summary>
    /// $ The name of the community$
    /// </summary>
    /// <example> A nice community name </example>
    public string Name { get; init; }
    /// <summary>
    /// $ The description of the community$
    /// </summary>
    /// <example> A description presenting the community in a few words </example>
    public string Description { get; init; }
    /// <summary>
    /// $ The manager off the community$
    /// </summary>
    /// <example> { 'LastName' : 'Manager', 'FirstName' : 'Example' } </example>
    public required Person Manager { get; init; }

    /// <summary>
    /// $ The expertises in this community$
    /// </summary>
    public required Expertise[] Expertises { get; init; }
}
