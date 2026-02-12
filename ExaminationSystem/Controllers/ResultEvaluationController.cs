using ExaminationSystem.ViewModels.ResultEvaluationViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class ResultEvaluationController : ControllerBase
{
    private readonly IResultEvaluationService _resultEvaluationService;
    private readonly IMapper _mapper;
    public ResultEvaluationController(IMapper mapper, IResultEvaluationService resultEvaluationService)
    {
        _resultEvaluationService = resultEvaluationService;
        _mapper = mapper;
    }

    [HttpPost("{examAttemptId}")]
    public async Task<IActionResult> EvaluateExamAsync(int examAttemptId)
    {
        var evaluateExamResponseDto = await _resultEvaluationService.EvaluateExamAsync(examAttemptId);

        var evaluateExamResponseVM = _mapper.Map<EvaluateExamResponseViewModel>(evaluateExamResponseDto);

        return Ok(evaluateExamResponseVM);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStudentResultsAsync(int examId, int instructorId)
    {
        var result = await _resultEvaluationService.GetAllStudentResultAsync(examId, instructorId);

        return Ok(result);
    }

    [HttpGet("examAttemptId")]
    public async Task<IActionResult> GetStudentResultAsync(int examAttemptId, int studentID)
    {
        var studentExamResultDto = await _resultEvaluationService.GetStudentExamResultAsync(examAttemptId, studentID);

        var studentExamResultVM = _mapper.Map<StudentExamResultViewModel>(studentExamResultDto);

        return Ok(studentExamResultVM);
    }
}
