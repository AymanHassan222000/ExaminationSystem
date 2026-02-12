using ExaminationSystem.DTOs.StudentDTO;
using ExaminationSystem.DTOs.StudentDTOs;
using ExaminationSystem.ViewModels.StudentViewModel;
using ExaminationSystem.ViewModels.StudentViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;
    public StudentsController(IMapper mapper, IStudentService studentService)
    {
        _mapper = mapper;
        _studentService = studentService;
    }

    [HttpPost]
    public async Task<IActionResult> EnrollInCourse(EnrollInCourseRequestViewModel vm)
    {
        var enrollInCourseDto = _mapper.Map<EnrollInCourseRequestDTO>(vm);

        await _studentService.EnrollInCourseAsync(enrollInCourseDto);

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> TakeExamAsync(TakeExamRequestViewModel vm)
    {
        var takeExamRequestDto = _mapper.Map<TakeExamRequestDTO>(vm);

        var studentID = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var takeExamResponseDto = await _studentService.TakeExamAsync(takeExamRequestDto, studentID);

        var takeExamResponseVM = _mapper.Map<TakeExamResponseViewModel>(takeExamResponseDto);

        return Ok(takeExamResponseVM);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitExamAsync(SubmitExamRequestViewModel vm)
    {
        var submitExamResquestDto = _mapper.Map<SubmitExamRequestDTO>(vm);

        var submitExamResponseDto = await _studentService.SubmitExamAsync(submitExamResquestDto);

        var submitExamResponseVM = _mapper.Map<SubmitExamResponseViewModel>(submitExamResponseDto);

        return Ok(submitExamResponseVM);
    }

}
