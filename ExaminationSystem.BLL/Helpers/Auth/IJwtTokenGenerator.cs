using ExaminationSystem.DAL.Models;

namespace ExaminationSystem.Helpers.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
