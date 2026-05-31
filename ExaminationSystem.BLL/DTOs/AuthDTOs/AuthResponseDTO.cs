using System.Text.Json.Serialization;

namespace ExaminationSystem.DTOs.AuthDTOs;

public sealed class AuthResponseDTO
{
    public string Token { get; set; }
    public string Role { get; set; }

    [JsonIgnore]
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}