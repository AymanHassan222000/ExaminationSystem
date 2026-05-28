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

    public int UserID
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?
                                                  .User?
                                                  .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
                return userId;

            throw new UnauthorizedAccessException("Invalid or missing User ID claim");
        }
    }

    public UserRoles Role => Enum.Parse<UserRoles>(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value);
}
