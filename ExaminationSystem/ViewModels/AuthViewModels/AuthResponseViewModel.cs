using System.Text.Json.Serialization;

namespace ExaminationSystem.ViewModels.AuthViewModels;

public sealed class AuthResponseViewModel 
{
    public string Token { get; init; }
    public string Role { get; init; }

    [JsonIgnore]
    public string? RefreshToken { get; init; }
    public DateTime RefreshTokenExpiration { get; init; }

};
