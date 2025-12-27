using AutoMapper;
using ExaminationSystem.DTOs.QuestionDTOs;
using ExaminationSystem.Services;
using ExaminationSystem.ViewModels.QuestionViewModel;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAllQuestions()
    {
        var qustionsDto = _questionService.GetAllQuestions();

        var questionsVM = _mapper.Map<IEnumerable<QuestionDetailsDTO>>(qustionsDto);

        return Ok(questionsVM);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuestionByIDAsync(int id)
    {

        var questionDetailsDto = await _questionService.GetQuestionByIDAsync(id);

        var questionDetailsVM = _mapper.Map<QuestionDetailsViewModel>(questionDetailsDto);

        return Ok(questionDetailsVM);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateQuestionAsync(int id, UpdateQuestionViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updateQuestionDto = _mapper.Map<UpdateQuestionDTO>(vm);

        var questionDetailsDto = await _questionService.UpdateQuestionAsync(id,updateQuestionDto);

        var questionDetailsVM = _mapper.Map<QuestionDetailsViewModel>(questionDetailsDto);

        return Ok(questionDetailsVM);
    }

    [HttpDelete("{id}")]
    public async Task DeleteQuestionHardAsync(int id)
    {
        await _questionService.DeleteQuestionAsync(id);
    }

}
