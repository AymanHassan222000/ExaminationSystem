using AutoMapper;
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
    private IMapper _mapper;
    public CoursesController(IMapper mapper)
    {
        _courseService = new CourseService(mapper);
        _mapper = mapper;
    }

    //Add Course
    [HttpPost]
    public async Task<ActionResult> AddCourse(CreateCourseViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createCourseDto = _mapper.Map<CreateCourseDTO>(vm);

        var courseDetailsDto = await _courseService.AddCourseAsync(createCourseDto);

        if (courseDetailsDto == null) return BadRequest();

        var courseDetailsVM = _mapper.Map<CourseDetailsViewModel>(courseDetailsDto);

        return StatusCode(StatusCodes.Status201Created, courseDetailsVM);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CourseDetailsViewModel>> GetAllCourses()
    {
        var courseDetailsdto = _courseService.GetAllCourses();
        var courseDetailsVM = _mapper.Map<IEnumerable<CourseDetailsViewModel>>(courseDetailsdto);

        return Ok(courseDetailsVM);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCourseByID(int id)
    {

        var dto = await _courseService.GetCourseByIDAsync(id);

        if (dto == null)
            return BadRequest();

        var courseVM = _mapper.Map<CourseDetailsViewModel>(dto);

        return Ok(courseVM);
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
            ID = dto.ID,
            Name = dto.Name,
            Description = dto.Description,
            Hours = dto.Hours,
            instructorInfo = new GetInstructorInfoViewModel
            {
                ID = dto.instructorInfo.ID,
                FullName = dto.instructorInfo.FullName,
            }
        });

    }

    [HttpDelete("{id}")]
    public async Task DeleteCourseHardAsync(int id)
    {
        await _courseService.DeleteCourse(id);

    }

    [HttpDelete("{id}")]
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
