using Host.Dto.Base;

namespace Host.Dto;

public class CommunityPatchRequest : PatchRequestBase
{
    public string? Name { get => GetProperty<string>(); set => SetProperty(value); }
    public string? Description { get => GetProperty<string>(); set => SetProperty(value); }
}
