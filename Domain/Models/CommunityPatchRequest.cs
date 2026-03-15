using Domain.Models.Base;

namespace Domain.Models
{
    public class CommunityPatchRequest : PatchRequestBase
    {
        public string? Name { get => GetProperty<string>(); set => SetProperty(value); }
        public string? Description { get => GetProperty<string>(); set => SetProperty(value); }
    }
}
