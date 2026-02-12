using ExaminationSystem.DTOs.ExamDTOs;
using ExaminationSystem.ViewModels.ExamViewModels;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class ExamsController : ControllerBase
{
    private readonly IExamService _examService;
    private readonly IMapper _mapper;
    public ExamsController(IMapper mapper, IExamService examService)
    {
        _examService = examService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ResponseViewModel<IEnumerable<ExamDetailsViewModel>>> GetAllExamsAsync(int instructorID)
    {
        var responseDto = await _examService.GetAllExamsAsync(instructorID);

        var responseVM = _mapper.Map<ResponseViewModel<IEnumerable<ExamDetailsViewModel>>>(responseDto);

        return responseVM;
    }

    [HttpGet("{examID}")]
    public async Task<ResponseViewModel<ExamDetailsViewModel>> GetExamByIDAsync(int examID, int instructorID)
    {

        var responseDto = await _examService.GetExamByIDAsync(examID, instructorID);

        var responseVM = _mapper.Map<ResponseViewModel<ExamDetailsViewModel>>(responseDto);

        return responseVM;
    }

    [HttpPut("{examID}")]
    public async Task<ResponseViewModel<ExamDetailsViewModel>> UpdateExamAsync(int examID, int instructorID, UpdateExamViewModel vm)
    {
        if (!ModelState.IsValid)
            return new FailureResponseViewModel<ExamDetailsViewModel>(ErrorCode.InvalidModelState, ModelState.GetErrorMessages());

        var updateExamDto = _mapper.Map<UpdateExamDTO>(vm);

        var responseDto = await _examService.UpdateExamAsync(examID, instructorID, updateExamDto);

        var responseVM = _mapper.Map<ResponseViewModel<ExamDetailsViewModel>>(responseDto);

        return responseVM;
    }

    [HttpDelete("{examID}")]
    public async Task DeleteExamHardAsync(int examID, int instructorID)
    {
        await _examService.DeleteExamAsync(examID, instructorID);
    }

}
