using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.CourseViewModels;
using ExaminationSystem.ViewModels.InstructorViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class CoursesController : ControllerBase
{
    private CourseService _courseService;
    public CoursesController()
    {
        _courseService = new CourseService();
    }

    //Add Course
    [HttpPost]
    public async Task<ActionResult> AddCourse(CreateCourseViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var courseDto = new CreateCourseDTO
        {
            Name = vm.Name,
            Description = vm.Description,
            Hours = vm.Hours,
            InstructorID = vm.InstructorID
        };

        var result = await _courseService.AddCourseAsync(courseDto);

        if (result == null) return BadRequest();

        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public IEnumerable<CourseDetailsViewModel> GetAllCourses()
    {
        return _courseService.GetAllCourses()
            .Select(m => new CourseDetailsViewModel
            {
                CourseID = m.CourseID,
                CourseName = m.CourseName,
                Hours = m.Hours,
                Description = m.Description,
                instructorInfo = new GetInstructorInfoViewModel
                {
                    InstructorID = m.instructorInfo.InstructorID,
                    FirstName = m.instructorInfo.FirstName,
                    LastName = m.instructorInfo.LastName
                }
            });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCourseByID(int id)
    {

        var dto = await _courseService.GetCourseByIDAsync(id);

        if (dto == null)
            return BadRequest();

        return Ok(new CourseDetailsViewModel
        {
            CourseID = dto.CourseID,
            CourseName = dto.CourseName,
            Description = dto.Description,
            Hours = dto.Hours,
            instructorInfo = new GetInstructorInfoViewModel
            {
                InstructorID = dto.instructorInfo.InstructorID,
                FirstName = dto.instructorInfo.FirstName,
                LastName = dto.instructorInfo.LastName
            }
        });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CourseDetailsViewModel>> UpdateCourseAsync(int id, UpdateCourseViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _courseService.UpdateCourseAsync(id, new UpdateCourseDTO
        {
            Name = vm.Name,
            Description = vm.Description,
            Hours = vm.Hours,
            InstructorID = vm.InstructorID
        });

        if (dto == null)
            return BadRequest();

        return Ok(new CourseDetailsViewModel
        {
            CourseID = dto.CourseID,
            CourseName = dto.CourseName,
            Description = dto.Description,
            Hours = dto.Hours,
            instructorInfo = new GetInstructorInfoViewModel
            {
                InstructorID = dto.instructorInfo.InstructorID,
                FirstName = dto.instructorInfo.FirstName,
                LastName = dto.instructorInfo.LastName
            }
        });

    }

    [HttpDelete("{id}")]
    public async Task DeleteCourseHardAsync(int id)
    {
        await _courseService.DeleteCourse(id);

    }

    [HttpPut("{id}")]
    public async Task DeleteCourseSoftAsync(int id)
    {
        await _courseService.DeleteCourseSoftAsync(id);
    }

    [HttpPost]
    public async Task EnrollInCourse(EnrollInCourseViewModel vm)
    {
        await _courseService.AssignStudentToCourse(new EnrollInCourseDTO
        {
            StudentID = vm.StudentID,
            CourseID = vm.CourseID
        });
    }

}
