using ExaminationSystem.DAL.Models;

namespace ExaminationSystem.Helpers.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(int userID, string email, int roleID);
}
