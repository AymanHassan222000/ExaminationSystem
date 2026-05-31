using ExaminationSystem.DAL.Services.Interfaces;
using ExaminationSystem.Models.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ExaminationSystem.DAL.Services.Implementations;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserID
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return int.TryParse(userId, out var id)
                ? id
                : null;
        }
    }

    public UserRoles Role => Enum.Parse<UserRoles>(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value);
}
