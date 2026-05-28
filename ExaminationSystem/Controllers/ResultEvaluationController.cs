using ExaminationSystem.API.ViewModels.ResultEvaluationViewModels;
using ExaminationSystem.BLL.Interfaces;
using ExaminationSystem.DTOs;
using ExaminationSystem.DTOs.ResultEvaluationDTOs;
using ExaminationSystem.Helpers.Mapping;
using ExaminationSystem.ViewModels.ResponseViewModels;
using ExaminationSystem.ViewModels.ResultEvaluationViewModels;
using System.Collections.Generic;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public sealed class ResultEvaluationController : ControllerBase
{
    private readonly IResultEvaluationService _resultEvaluationService;
    public ResultEvaluationController(IResultEvaluationService resultEvaluationService)
    {
        _resultEvaluationService = resultEvaluationService;
    }

    [HttpPost("{examID}")]
    [Authorize(Roles = "Student")]
    public async Task<ResponseViewModel<EvaluateExamResponseViewModel>> EvaluateExamAsync(int examID)
    {
        var response = await _resultEvaluationService.EvaluateExamAsync(examID);

        if (!response.IsSuccess)
            return ResponseViewModel<EvaluateExamResponseViewModel>.Failure(response.ErrorCode, response.Message);

        var evaluateExamResponseVM = AutoMapperHelper.Map<EvaluateExamResponseViewModel>(response.Data);

        return ResponseViewModel<EvaluateExamResponseViewModel>.Success(evaluateExamResponseVM, response.Message);
    }

    [HttpGet("{examID}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<ResponseViewModel<IEnumerable<ExamResultSummaryViewModel>>> GetAllStudentResultsAsync(int examID)
    {
        var result = await _resultEvaluationService.GetAllStudentResultAsync(examID);

        if (!result.IsSuccess)
            return ResponseViewModel<IEnumerable<ExamResultSummaryViewModel>>.Failure(result.ErrorCode, result.Message);

        var data = AutoMapperHelper.Map<IEnumerable<ExamResultSummaryViewModel>>(result.Data);

        return ResponseViewModel<IEnumerable<ExamResultSummaryViewModel>>.Success(data, result.Message);
    }

}
