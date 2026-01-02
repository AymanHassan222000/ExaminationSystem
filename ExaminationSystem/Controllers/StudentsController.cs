using AutoMapper;
using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.StudentViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class StudentsController : ControllerBase
{
    IMapper _mapper;
    StudentService _studentService;
    public StudentsController(IMapper mapper)
    {
        _mapper = mapper;
        _studentService = new StudentService(mapper);
    }

    [HttpPost]
    public async Task<IActionResult> EnrollInCourse(AssignStudentToCourseRequestViewModel vm)
    {
        var enrollInCourseDto = _mapper.Map<AssignStudentToCourseRequestDTO>(vm);

        await _studentService.AssignStudentToCourse(enrollInCourseDto);

        return Ok();
    }
}
