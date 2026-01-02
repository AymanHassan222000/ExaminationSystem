using AutoMapper;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.QuestionViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ExaminationSystem.Controllers;

[ApiController]
[Route("[Controller]/[Action]")]
public class QuestionsController : ControllerBase
{
    QuestionService _questionService;
    IMapper _mapper;
    public QuestionsController(IMapper mapper)
    {
        _questionService = new QuestionService(mapper);
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> AddQuestionAsync(CreateQuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createQuestionDto = _mapper.Map<CreateQuestionDTO>(vm);

        var questionDetailsDto = await _questionService.AddQuestionAsync(createQuestionDto);

        var questionDetailsVM = _mapper.Map<QuestionDetailsViewModel>(questionDetailsDto);

        return StatusCode(StatusCodes.Status201Created, questionDetailsVM);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllQuestions(int instructorID)
    {
        var qustionsDto = await _questionService.GetAllQuestionsAsync(instructorID);

        var questionsVM = _mapper.Map<IEnumerable<QuestionDetailsDTO>>(qustionsDto);

        return Ok(questionsVM);
    }

    [HttpGet("{questionID}")]
    public async Task<IActionResult> GetQuestionByIDAsync(int questionID,int instructorID)
    {

        var questionDetailsDto = await _questionService.GetQuestionByIDAsync(questionID,instructorID);

        var questionDetailsVM = _mapper.Map<QuestionDetailsViewModel>(questionDetailsDto);

        return Ok(questionDetailsVM);
    }

    [HttpPut("{questionID}")]
    public async Task<IActionResult> UpdateQuestionAsync(int questionID,int instructorID, UpdateQuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateQuestionDto = _mapper.Map<UpdateQuestionDTO>(vm);

        var questionDetailsDto = await _questionService.UpdateQuestionAsync(questionID,instructorID,updateQuestionDto);

        var questionDetailsVM = _mapper.Map<QuestionDetailsViewModel>(questionDetailsDto);

        return Ok(questionDetailsVM);
    }

    [HttpDelete("{questionID}")]
    public async Task DeleteQuestionHardAsync(int questionID, int instructorID)
    {
        await _questionService.DeleteQuestionAsync(questionID, instructorID);
    }

}
