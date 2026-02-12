using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.ViewModels.QuestionViewModel;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("api/[Controller]/[Action]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;
    private readonly IMapper _mapper;
    public QuestionsController(IMapper mapper, IQuestionService questionService)
    {
        _questionService = questionService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ResponseViewModel<QuestionDetailsViewModel>> AddQuestionAsync(CreateQuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return new FailureResponseViewModel<QuestionDetailsViewModel>(ErrorCode.InvalidModelState,ModelState.GetErrorMessages());

        var createQuestionDto = _mapper.Map<CreateQuestionDTO>(vm);

        var responseDto = await _questionService.AddQuestionAsync(createQuestionDto);

        var responseVM = _mapper.Map<ResponseViewModel<QuestionDetailsViewModel>>(responseDto);


        return responseVM;
    }

    [HttpGet]
    public async Task<ResponseViewModel<IEnumerable<QuestionDetailsViewModel>>> GetAllQuestions(int instructorID)
    {
        var responseDto = await _questionService.GetAllQuestionsAsync(instructorID);

        var responseVM = _mapper.Map<ResponseViewModel<IEnumerable<QuestionDetailsViewModel>>>(responseDto);

        return responseVM;
    }

    [HttpGet("{questionID}")]
    public async Task<ResponseViewModel<QuestionDetailsViewModel>> GetQuestionByIDAsync(int questionID,int instructorID)
    {

        var responseDto = await _questionService.GetQuestionByIDAsync(questionID,instructorID);

        var responseVM = _mapper.Map<ResponseViewModel<QuestionDetailsViewModel>>(responseDto);

        return responseVM;
    }

    [HttpPut("{questionID}")]
    public async Task<ResponseViewModel<QuestionDetailsViewModel>> UpdateQuestionAsync(int questionID,int instructorID, UpdateQuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return new FailureResponseViewModel<QuestionDetailsViewModel>(ErrorCode.InvalidModelState,ModelState.GetErrorMessages());

        var updateQuestionDto = _mapper.Map<UpdateQuestionDTO>(vm);

        var responseDto = await _questionService.UpdateQuestionAsync(questionID,instructorID,updateQuestionDto);

        var responseVM = _mapper.Map<ResponseViewModel<QuestionDetailsViewModel>>(responseDto);

        return responseVM;
    }

    [HttpDelete("{questionID}")]
    public async Task<ResponseViewModel<QuestionDetailsViewModel>> DeleteQuestionHardAsync(int questionID, int instructorID)
    {
        var responseDto = await _questionService.DeleteQuestionAsync(questionID, instructorID);
        var responseVM = _mapper.Map<ResponseViewModel<QuestionDetailsViewModel>>(responseDto);

        return responseVM;
    }

}
