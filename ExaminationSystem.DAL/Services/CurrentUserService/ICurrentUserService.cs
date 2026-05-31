using ExaminationSystem.Models.Enums;

namespace ExaminationSystem.DAL.Services.Interfaces;

public interface ICurrentUserService
{
    int? UserID { get; }
    UserRoles Role { get; }
}
