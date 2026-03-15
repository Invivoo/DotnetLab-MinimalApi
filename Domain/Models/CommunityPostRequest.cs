using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public record CommunityPostRequest(string Name, string Description);
