using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.DTOs.StudentDTOs;
using ExaminationSystem.ViewModels.StudentViewModels;

namespace ExaminationSystem.Controllers;

[Authorize(Roles = "Student")]
[ApiController]
[Route("api/[Controller]/[Action]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }


}
