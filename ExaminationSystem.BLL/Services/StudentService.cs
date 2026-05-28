using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Repositories;

namespace ExaminationSystem.BLL.Services;

public class StudentService : IStudentService
{
    private readonly IGeneralRepository<Student> _studentRepo;
    public StudentService(IGeneralRepository<Student> studentRepo)
    {
        _studentRepo = studentRepo;
    }

    public async Task<bool> IsStudentExist(int studentID)
    {
        var isExist = await _studentRepo.AnyAsync(s => s.UserID == studentID);

        return isExist;
    }


}
