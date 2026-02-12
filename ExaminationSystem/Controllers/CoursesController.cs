using ExaminationSystem.DTOs.CourseDTOs;
using ExaminationSystem.ViewModels.CourseViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class CoursesController : ControllerBase
{
    private ICourseService _courseService;
    private IMapper _mapper;
    public CoursesController(IMapper mapper,ICourseService courseService)
    {
        _courseService = courseService;
        _mapper = mapper;
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPost]
    public async Task<ResponseViewModel<CourseDetailsViewModel>> AddCourseAsync(CreateCourseViewModel vm)
    {
        if (!ModelState.IsValid)
            return new FailureResponseViewModel<CourseDetailsViewModel>(ErrorCode.InvalidModelState, ModelState.GetErrorMessages());

        var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var createCourseDto = _mapper.Map<CreateCourseDTO>(vm);

        var responseDto = await _courseService.AddCourseAsync(createCourseDto, userID);

        var responseVM = _mapper.Map<ResponseViewModel<CourseDetailsViewModel>>(responseDto);

        return responseVM;
    }

    [Authorize]
    [HttpGet]
    public async Task<ResponseViewModel<IEnumerable<CourseDetailsViewModel>>> GetAllCoursesAsync()
    {
        var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var userRole = Enum.Parse<UserRoles>(User.FindFirstValue(ClaimTypes.Role)) ;

        var responseDto = await _courseService.GetAllCoursesAsync(new UserContextDTO(userID,userRole));

        var responseVM = _mapper.Map<ResponseViewModel<IEnumerable<CourseDetailsViewModel>>>(responseDto);

        return responseVM;
    }

    [Authorize]
    [HttpGet("{courseID}")]
    public async Task<ResponseViewModel<CourseDetailsViewModel>> GetCourseByIDAsync(int courseID)
    {
        var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var userRole = Enum.Parse<UserRoles>(User.FindFirstValue(ClaimTypes.Role));

        var responseDto = await _courseService.GetCourseByIDAsync(courseID, new UserContextDTO(userID, userRole));

        var responseVM = _mapper.Map<ResponseViewModel<CourseDetailsViewModel>>(responseDto);

        return responseVM;
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpPut("{courseID}")]
    public async Task<ResponseDTO<CourseDetailsDTO>> UpdateCourseAsync(int courseID,[FromBody] UpdateCourseDTO dto)
    {
        if (courseID != dto.ID)
            return new FailureResponseDTO<CourseDetailsDTO>(ErrorCode.InvalidCourseID,"Invalid Course ID");

        var userID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var userRole = Enum.Parse<UserRoles>(User.FindFirstValue(ClaimTypes.Role));

        var responseDto = await _courseService.UpdateCourseAsync(courseID, dto, new UserContextDTO(userID, userRole));

        return responseDto;
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpDelete("{courseID}")]
    public async Task<IActionResult> DeleteCourseHardAsync(int courseID, int instructorID)
    {
        await _courseService.DeleteCourse(courseID, instructorID);

        return Ok();
    }

    [Authorize(Roles = "Admin,Instructor")]
    [HttpDelete("{courseID}")]
    public async Task<IActionResult> DeleteCourseSoftAsync(int courseID, int instructorID)
    {
        await _courseService.DeleteCourseSoftAsync(courseID, instructorID);

        return Ok();
    }

}
