using AutoMapper;
using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.DTOs.IntructorDTOs;
using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.CourseViewModels;
using ExaminationSystem.ViewModels.InstructorViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    [HttpPost]
    public async Task<IActionResult> AddCourseAsync(CreateCourseViewModel vm, int instructorID)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (instructorID != vm.InstructorID)
            return BadRequest("You can not add course for another instructor.");

        var createCourseDto = _mapper.Map<CreateCourseDTO>(vm);

        var courseDetailsDto = await _courseService.AddCourseAsync(createCourseDto, instructorID);

        if (courseDetailsDto == null) return BadRequest();

        var courseDetailsVM = _mapper.Map<CourseDetailsViewModel>(courseDetailsDto);

        return StatusCode(StatusCodes.Status201Created, courseDetailsVM);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCoursesAsync(int instructorId)
    {
        var courseDetailsdto = await _courseService.GetAllCoursesAsync(instructorId);
        var courseDetailsVM = _mapper.Map<IEnumerable<CourseDetailsViewModel>>(courseDetailsdto);

        return Ok(courseDetailsVM);
    }

    [HttpGet("{courseID}")]
    public async Task<IActionResult> GetCourseByID(int courseID, int instructorId)
    {

        var dto = await _courseService.GetCourseByIDAsync(courseID, instructorId);

        var courseVM = _mapper.Map<CourseDetailsViewModel>(dto);

        return Ok(courseVM);
    }

    [HttpPut("{courseID}")]
    public async Task<IActionResult> UpdateCourseAsync(int courseID, int instructorID, UpdateCourseViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateCourseDto = _mapper.Map<UpdateCourseDTO>(vm);

        var courseDetailsDto = await _courseService.UpdateCourseAsync(courseID, instructorID, updateCourseDto);

        var courseDetailsVM = _mapper.Map<CourseDetailsViewModel>(courseDetailsDto);

        return Ok(courseDetailsVM);
    }

    [HttpDelete("{courseID}")]
    public async Task<IActionResult> DeleteCourseHardAsync(int courseID,int instructorID)
    {
        await _courseService.DeleteCourse(courseID,instructorID);

        return Ok();
    }

    [HttpDelete("{courseID}")]
    public async Task<IActionResult> DeleteCourseSoftAsync(int courseID,int instructorID)
    {
        await _courseService.DeleteCourseSoftAsync(courseID, instructorID);

        return Ok();
    }


}
